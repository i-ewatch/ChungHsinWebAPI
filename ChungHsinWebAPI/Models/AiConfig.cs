using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// AI資訊
    /// </summary>
    public class AiConfig
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 設備類型
        /// </summary>
        public int DeviceTypeEnum { get; set; }
        /// <summary>
        /// 數值編號
        /// </summary>
        public string AINo { get; set; }
        /// <summary>
        /// 陣列位址
        /// </summary>
        public int CharAddress { get; set; }
        /// <summary>
        /// 數值名稱
        /// </summary>
        public string AIName { get; set; }
        /// <summary>
        /// 分析方式 0=不解析 ， 1 = LH
        /// </summary>
        public int AnalysisMothed { get; set; }
        /// <summary>
        /// 比值(乘)
        /// </summary>
        public float Ratio { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Aiunit { get; set; }
        /// <summary>
        /// (類型名稱)請將每個號碼與名稱輸入並將每個類型用逗號區隔 如:(0=停止,1=啟動)
        /// </summary>
        public string Enum_Array { get; set; }
        /// <summary>
        /// 0 = 曲線圖
        /// 1 = 長條圖(累積用)
        /// </summary>
        public int ChartTypeEnum { get; set; }
    }
}