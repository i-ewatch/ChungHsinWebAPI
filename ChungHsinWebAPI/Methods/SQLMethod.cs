using ChungHsinWebAPI.Enums;
using ChungHsinWebAPI.Models;
using Dapper;
using MathLibrary;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace ChungHsinWebAPI.Methods
{
    /// <summary>
    /// 資料庫方法
    /// </summary>
    public class SQLMethod
    {
        /// <summary>
        /// 初始建立
        /// </summary>
        public SQLMethod()
        {
            #region Serilog initial
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\log\\log-.txt",
                                      rollingInterval: RollingInterval.Day,
                                      outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();        //宣告Serilog初始化
            #endregion
        }
        private string ConnStr = WebConfigurationManager.ConnectionStrings["EwatchDBConnectionString"].ConnectionString.ToString();
        private string DataDB = HttpContext.Current.Application["DB"].ToString();
        private string DataLog = HttpContext.Current.Application["Log"].ToString();
        private string DataWeb = HttpContext.Current.Application["Web"].ToString();
        /// <summary>
        /// LOG錯誤字串
        /// </summary>
        public string Receive_Procedure_ErrorStr { get; set; }
        /// <summary>
        /// 數值錯誤字串
        /// </summary>
        public string Value_Log_Insert_ErrorStr { get; set; }
        /// <summary>
        /// 狀態錯誤字串
        /// </summary>
        public string State_Log_Insert_ErrorStr { get; set; }
        /// <summary>
        /// 數學公式
        /// </summary>
        public MathClass MathClass { get; set; } = new MathClass();

        #region Log儲存
        /// <summary>
        /// 紀錄Log
        /// </summary>
        /// <param name="receiveData"></param>
        /// <returns></returns>
        public int Receive_Procedure(ReceiveData receiveData)
        {
            using (var connection = new MySqlConnection(ConnStr))
            {
                try
                {
                    Receive recive = new Receive() { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo };
                    recive.data_array = receiveData.Ai[0].ToString();
                    for (int i = 1; i < receiveData.Ai.Length; i++)
                    {
                        recive.data_array += $",{receiveData.Ai[i]}";
                    }
                    for (int i = receiveData.Ai.Length; i < 128; i++)
                    {
                        recive.data_array += $",0";
                    }
                    ReceiveData data = new ReceiveData()
                    {
                        CaseNo = receiveData.CaseNo,
                        RecNo = receiveData.RecNo,
                        ttime = receiveData.ttime,
                    };
                    for (int i = 0; i < receiveData.Ai.Length; i++)
                    {
                        data.Ai[i] = receiveData.Ai[i];
                    }
                    for (int i = receiveData.Ai.Length; i < data.Ai.Length; i++)
                    {
                        data.Ai[i] = 0;
                    }
                   
                    string InsertLogsql = $"CALL {DataLog}.ReceiveProcedure(@ttime,@CaseNo,@ReceiveNo,@data_array)";
                    var LogCounter = connection.Execute(InsertLogsql, recive);
                    if ((LogCounter / 3) > 0)
                    {
                        Value_Log_Insert(data);
                        State_Log_Insert(data);
                        Receive_Procedure_ErrorStr = ("上傳成功，異動筆數共" + (LogCounter / 3).ToString()) + "筆" + Value_Log_Insert_ErrorStr + State_Log_Insert_ErrorStr;
                        return 0;
                    }
                    else
                    {
                        Log.Information(JsonConvert.SerializeObject(data));
                        Receive_Procedure_ErrorStr = ("上傳成功，案場未建立、設備已上傳");
                        return 1;
                    }

                }
                catch (Exception ex)
                {
                    Receive_Procedure_ErrorStr = (ex.Message + "\r\n" + ex.ToString());       // 400 Bad Request
                    return 2;
                }
            }
        }
        #endregion

        #region 更新案場數值歷史資料與網頁
        /// <summary>
        /// 更新案場數值歷史資料與網頁
        /// </summary>
        /// <param name="receiveData"></param>
        private void Value_Log_Insert(ReceiveData receiveData)
        {
            using (var connection = new MySqlConnection(ConnStr))
            {
                try
                {
                    string search_ReceiveSetting = $"SELECT * FROM {DataDB}.ReceiveSetting WHERE CaseNo = @CaseNo AND ReceiveNo = @ReceiveNo";
                    var RecevieSetting = connection.QuerySingleOrDefault<RecevieSetting>(search_ReceiveSetting, new { CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo });
                    if (RecevieSetting != null)
                    {
                        string search_Aiconfig = $"SELECT * FROM {DataDB}.AiConfig WHERE DeviceTypeEnum = @DeviceTypeEnum";
                        var Aiconfig = connection.Query<AiConfig>(search_Aiconfig, new { DeviceTypeEnum = RecevieSetting.DeviceTypeEnum }).ToList();
                        if (Aiconfig.Count > 0)
                        {
                            List<decimal> Value = new List<decimal>();
                            for (int i = 0; i < Aiconfig.Count; i++)
                            {
                                AnalysisTypeEnum analysisTypeEnum = (AnalysisTypeEnum)Aiconfig[i].AnalysisMothed;
                                switch (analysisTypeEnum)
                                {
                                    case AnalysisTypeEnum.None:
                                        {
                                            var ai = Convert.ToDecimal(receiveData.Ai[Aiconfig[i].CharAddress]) * Convert.ToDecimal(Aiconfig[i].Ratio);
                                            Value.Add(ai);
                                        }
                                        break;
                                    case AnalysisTypeEnum.LH:
                                        {
                                            var ai = Convert.ToDecimal(receiveData.Ai[Aiconfig[i].CharAddress]) * 1 + Convert.ToDecimal(receiveData.Ai[Aiconfig[i].CharAddress + 1]) * 65536;
                                            Value.Add(ai);
                                            DeviceTypeEnum deviceTypeEnum = (DeviceTypeEnum)RecevieSetting.DeviceTypeEnum;
                                            switch (deviceTypeEnum)
                                            {
                                                case DeviceTypeEnum.RT80:
                                                    {
                                                        //if (Aiconfig[i].CharAddress == 34)//KWH
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 90)//總時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if(Aiconfig[i].CharAddress == 92)//1機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 96)//2機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        if (Aiconfig[i].ChartTypeEnum == 1)
                                                        {
                                                            string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                            connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        }
                                                    }
                                                    break;
                                                case DeviceTypeEnum.RT40_50_60:
                                                    {
                                                        //if (Aiconfig[i].CharAddress == 34)//KWH
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 38)//總時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 55)//1機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 75)//2機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 94)//3機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        //else if (Aiconfig[i].CharAddress == 114)//4機運轉時數
                                                        //{
                                                        //    string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                        //    connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        //}
                                                        if (Aiconfig[i].ChartTypeEnum == 1)
                                                        {
                                                            string AIDailyStr = $"CALL {DataLog}.AiDailyProcedure(@ttime,@CaseNo,@ReceiveNo,@AINo,@nowAi)";
                                                            connection.Execute(AIDailyStr, new { ttime = receiveData.ttime, CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo, AINo = Aiconfig[i].AINo, nowAi = ai });
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (Value.Count < 64)
                            {
                                for (int i = Value.Count; i < 64; i++)
                                {
                                    Value.Add(0);
                                }
                            }
                            AI64 aI64 = new AI64()
                            {
                                CaseNo = receiveData.CaseNo,
                                ttime = receiveData.ttime,
                                ReceiveNo = receiveData.RecNo,
                            };
                            int index = 0;
                            aI64.Ai1 = Value[index]; index++;
                            aI64.Ai2 = Value[index]; index++;
                            aI64.Ai3 = Value[index]; index++;
                            aI64.Ai4 = Value[index]; index++;
                            aI64.Ai5 = Value[index]; index++;
                            aI64.Ai6 = Value[index]; index++;
                            aI64.Ai7 = Value[index]; index++;
                            aI64.Ai8 = Value[index]; index++;
                            aI64.Ai9 = Value[index]; index++;
                            aI64.Ai10 = Value[index]; index++;
                            aI64.Ai12 = Value[index]; index++;
                            aI64.Ai13 = Value[index]; index++;
                            aI64.Ai14 = Value[index]; index++;
                            aI64.Ai15 = Value[index]; index++;
                            aI64.Ai16 = Value[index]; index++;
                            aI64.Ai17 = Value[index]; index++;
                            aI64.Ai18 = Value[index]; index++;
                            aI64.Ai19 = Value[index]; index++;
                            aI64.Ai20 = Value[index]; index++;
                            aI64.Ai22 = Value[index]; index++;
                            aI64.Ai23 = Value[index]; index++;
                            aI64.Ai24 = Value[index]; index++;
                            aI64.Ai25 = Value[index]; index++;
                            aI64.Ai26 = Value[index]; index++;
                            aI64.Ai27 = Value[index]; index++;
                            aI64.Ai28 = Value[index]; index++;
                            aI64.Ai29 = Value[index]; index++;
                            aI64.Ai30 = Value[index]; index++;
                            aI64.Ai32 = Value[index]; index++;
                            aI64.Ai33 = Value[index]; index++;
                            aI64.Ai34 = Value[index]; index++;
                            aI64.Ai35 = Value[index]; index++;
                            aI64.Ai36 = Value[index]; index++;
                            aI64.Ai37 = Value[index]; index++;
                            aI64.Ai38 = Value[index]; index++;
                            aI64.Ai39 = Value[index]; index++;
                            aI64.Ai40 = Value[index]; index++;
                            aI64.Ai42 = Value[index]; index++;
                            aI64.Ai43 = Value[index]; index++;
                            aI64.Ai44 = Value[index]; index++;
                            aI64.Ai45 = Value[index]; index++;
                            aI64.Ai46 = Value[index]; index++;
                            aI64.Ai47 = Value[index]; index++;
                            aI64.Ai48 = Value[index]; index++;
                            aI64.Ai49 = Value[index]; index++;
                            aI64.Ai50 = Value[index]; index++;
                            aI64.Ai52 = Value[index]; index++;
                            aI64.Ai53 = Value[index]; index++;
                            aI64.Ai54 = Value[index]; index++;
                            aI64.Ai55 = Value[index]; index++;
                            aI64.Ai56 = Value[index]; index++;
                            aI64.Ai57 = Value[index]; index++;
                            aI64.Ai58 = Value[index]; index++;
                            aI64.Ai59 = Value[index]; index++;
                            aI64.Ai60 = Value[index]; index++;
                            aI64.Ai62 = Value[index]; index++;
                            aI64.Ai63 = Value[index]; index++;
                            aI64.Ai64 = Value[index];
                            string InsertLogsql = $"CALL {DataLog}.Ai64LogProcedure(@CaseNo,@ttime,@ReceiveNo,@Ai1,@Ai2,@Ai3,@Ai4,@Ai5,@Ai6,@Ai7,@Ai8,@Ai9,@Ai10," +
                                   $"@Ai11,@Ai12,@Ai13,@Ai14,@Ai15,@Ai16,@Ai17,@Ai18,@Ai19,@Ai20," +
                                   $"@Ai21,@Ai22,@Ai23,@Ai24,@Ai25,@Ai26,@Ai27,@Ai28,@Ai29,@Ai30," +
                                   $"@Ai31,@Ai32,@Ai33,@Ai34,@Ai35,@Ai36,@Ai37,@Ai38,@Ai39,@Ai40," +
                                   $"@Ai41,@Ai42,@Ai43,@Ai44,@Ai45,@Ai46,@Ai47,@Ai48,@Ai49,@Ai50," +
                                   $"@Ai51,@Ai52,@Ai53,@Ai54,@Ai55,@Ai56,@Ai57,@Ai58,@Ai59,@Ai60," +
                                   $"@Ai61,@Ai62,@Ai63,@Ai64)";
                            string InsertWebsql = $"CALL {DataWeb}.Ai64WebProcedure(@CaseNo,@ttime,@ReceiveNo,@Ai1,@Ai2,@Ai3,@Ai4,@Ai5,@Ai6,@Ai7,@Ai8,@Ai9,@Ai10," +
                                                $"@Ai11,@Ai12,@Ai13,@Ai14,@Ai15,@Ai16,@Ai17,@Ai18,@Ai19,@Ai20," +
                                                $"@Ai21,@Ai22,@Ai23,@Ai24,@Ai25,@Ai26,@Ai27,@Ai28,@Ai29,@Ai30," +
                                                $"@Ai31,@Ai32,@Ai33,@Ai34,@Ai35,@Ai36,@Ai37,@Ai38,@Ai39,@Ai40," +
                                                $"@Ai41,@Ai42,@Ai43,@Ai44,@Ai45,@Ai46,@Ai47,@Ai48,@Ai49,@Ai50," +
                                                $"@Ai51,@Ai52,@Ai53,@Ai54,@Ai55,@Ai56,@Ai57,@Ai58,@Ai59,@Ai60," +
                                                $"@Ai61,@Ai62,@Ai63,@Ai64)";
                            int LogCounter = connection.Execute(InsertLogsql, aI64);
                            int WebCounter = connection.Execute(InsertWebsql, aI64);

                            if ((LogCounter / 3) > 0)
                            {
                                Value_Log_Insert_ErrorStr = (" Value_Log異動筆數共" + (LogCounter / 3).ToString() + "筆" + " Value_Web異動筆數共" + (WebCounter / 3).ToString() + "筆");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    Value_Log_Insert_ErrorStr = (ex.Message + "\r\n" + ex.ToString());       // 400 Bad Request
                }
            }
        }
        #endregion

        #region 更新案場狀態歷史資料與網頁
        /// <summary>
        /// 更新案場狀態歷史資料與網頁
        /// </summary>
        /// <param name="receiveData"></param>
        private void State_Log_Insert(ReceiveData receiveData)
        {
            using (var connection = new MySqlConnection(ConnStr))
            {
                try
                {
                    string search_ReceiveSetting = $"SELECT * FROM {DataDB}.ReceiveSetting WHERE CaseNo = @CaseNo AND ReceiveNo = @ReceiveNo";
                    var RecevieSetting = connection.QuerySingleOrDefault<RecevieSetting>(search_ReceiveSetting, new { CaseNo = receiveData.CaseNo, ReceiveNo = receiveData.RecNo });
                    if (RecevieSetting != null)
                    {
                        string search_StateConfig = $"SELECT * FROM {DataDB}.StateConfig WHERE  DeviceTypeEnum = @DeviceTypeEnum";
                        var Stateconfig = connection.Query<StateConfig>(search_StateConfig, new { DeviceTypeEnum = RecevieSetting.DeviceTypeEnum }).ToList();
                        int LogCounter = 0;
                        int WebCounter = 0;
                        foreach (var Stateconfigitem in Stateconfig)
                        {
                            State state = new State()
                            {
                                CaseNo = receiveData.CaseNo,
                                ReceiveNo = receiveData.RecNo,
                                ttime = receiveData.ttime,
                                StateNo = Stateconfigitem.StateNo,
                            };
                            state.state = Convert.ToBoolean(Convert.ToInt32(MathClass.work10to2(Convert.ToUInt16(receiveData.Ai[Stateconfigitem.CharAddress])).Substring(15 - Stateconfigitem.StateBitAddress, 1)));
                            string InsertLogsql = $"CALL {DataLog}.StateLogProcedure(@CaseNo,@ttime,@ReceiveNo,@StateNo,@state)";
                            string InsertWebsql = $"CALL {DataWeb}.StateWebProcedure(@CaseNo,@ttime,@ReceiveNo,@StateNo,@state)";
                            try//ChungHsinWeb.State 已建立
                            {
                                int logcounter = connection.Execute(InsertLogsql, state);
                                if (logcounter > 3)
                                {
                                    LogCounter = LogCounter + logcounter;
                                }

                            }
                            catch (Exception)// ChungHsinWeb.State 未建立
                            {
                                WebCounter = connection.Execute(InsertWebsql, state);
                            }
                            WebCounter = connection.Execute(InsertWebsql, state);
                        }
                        if ((WebCounter / 3) > 0)
                        {
                            State_Log_Insert_ErrorStr = (" State_Log異動筆數共" + (LogCounter / 4).ToString() + "筆" + " State_Web異動筆數共" + (WebCounter / 3).ToString() + "筆");
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region 查詢Receive_Log
        /// <summary>
        /// 查詢Receive_Log
        /// </summary>
        /// <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        public List<Receive_Log> Search_Receive_Log(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            List<Receive_Log> logs = new List<Receive_Log>();
            using (IDbConnection connection = new MySqlConnection(ConnStr))
            {
                string sql = $"SELECT * FROM {DataLog}.Receive_Log WHERE CaseNo = @CaseNo AND ReceiveNo = @ReceiveNo AND (ttime >= @StartTtime AND ttime <= @EndTtime )";
                logs = connection.Query<Receive_Log>(sql, new { CaseNo, ReceiveNo, StartTtime = StartTime + "000000", EndTtime = EndTime + "999999" }).ToList();
                return logs;
            }
        }
        #endregion

        #region 查詢AI64_Log
        /// <summary>
        /// 查詢AI64_Log
        /// </summary>
        /// <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        public List<AI64> Search_AI64Log(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            List<AI64> aI64s = new List<AI64>();
            using (IDbConnection connection = new MySqlConnection(ConnStr))
            {
                string sql = $"SELECT * FROM {DataLog}.AI64_{CaseNo} WHERE ReceiveNo = @ReceiveNo AND (ttime >= @StartTtime AND ttime <= @EndTtime )";
                aI64s = connection.Query<AI64>(sql, new { ReceiveNo, StartTtime = StartTime + "000000", EndTtime = EndTime + "999999" }).ToList();
                return aI64s;
            }
        }
        #endregion

        #region 查詢State_Log
        /// <summary>
        /// 查詢State_Log
        /// </summary>
        ///  <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        public List<State> Search_StateLog(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            List<State> states = new List<State>();
            using (IDbConnection connection = new MySqlConnection(ConnStr))
            {
                string sql = $"SELECT * FROM {DataLog}.State_{CaseNo} WHERE ReceiveNo = @ReceiveNo AND (ttime >= @StartTtime AND ttime <= @EndTtime )";
                states = connection.Query<State>(sql, new { ReceiveNo, StartTtime = StartTime + "000000", EndTtime = EndTime + "999999" }).ToList();
                return states;
            }
        }
        #endregion

        #region 查詢AI64_Web
        /// <summary>
        /// 查詢AI64_Web
        /// </summary>
        /// <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <returns></returns>
        public List<AI64> Search_AI64Web(string CaseNo, int ReceiveNo)
        {
            List<AI64> aI64s = new List<AI64>();
            using (IDbConnection connection = new MySqlConnection(ConnStr))
            {
                string sql = $"SELECT * FROM {DataWeb}.AI64 WHERE CaseNo=@CaseNo AND ReceiveNo = @ReceiveNo ";
                aI64s = connection.Query<AI64>(sql, new { CaseNo, ReceiveNo }).ToList();
                return aI64s;
            }
        }
        #endregion

        #region 查詢State_Web
        /// <summary>
        /// 查詢State_Web
        /// </summary>
        ///  <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <returns></returns>
        public List<State> Search_StateWeb(string CaseNo, int ReceiveNo)
        {
            List<State> states = new List<State>();
            using (IDbConnection connection = new MySqlConnection(ConnStr))
            {
                string sql = $"SELECT * FROM {DataWeb}.State WHERE CaseNo=@CaseNo AND ReceiveNo = @ReceiveNo ";
                states = connection.Query<State>(sql, new { CaseNo, ReceiveNo }).ToList();
                return states;
            }
        }
        #endregion
    }
}