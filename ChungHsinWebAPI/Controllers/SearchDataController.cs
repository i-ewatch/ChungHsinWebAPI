using ChungHsinWebAPI.Methods;
using ChungHsinWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChungHsinWebAPI.Controllers
{
    /// <summary>
    /// 查詢資料
    /// </summary>
    public class SearchDataController : ApiController
    {
        private SQLMethod SQLMethod = new SQLMethod();
        #region 查詢Receive_Log
        /// <summary>
        /// 查詢Receive_Log
        /// </summary>
        /// <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <returns></returns>
        [Route("api/ReceiveLog/{CaseNo}/{ReceiveNo}/{StartTime}/{EndTime}")]
        public IQueryable GetRecive_Log(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            var data = SQLMethod.Search_Receive_Log(CaseNo, ReceiveNo, StartTime, EndTime);
            return data.AsQueryable();
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
        [Route("api/AI64Log/{CaseNo}/{ReceiveNo}/{StartTime}/{EndTime}")]
        public IQueryable GetAI64_Log(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            var data = SQLMethod.Search_AI64Log(CaseNo, ReceiveNo, StartTime, EndTime);
            return data.AsQueryable();
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
        [Route("api/StateLog/{CaseNo}/{ReceiveNo}/{StartTime}/{EndTime}")]
        public IQueryable GetState_Log(string CaseNo, int ReceiveNo, string StartTime, string EndTime)
        {
            var data = SQLMethod.Search_StateLog(CaseNo, ReceiveNo, StartTime, EndTime);
            return data.AsQueryable();
        }
        #endregion

        #region 查詢AI64_Web
        /// <summary>
        /// 查詢AI64_Web
        /// </summary>
        /// <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <returns></returns>
        [Route("api/AI64Web/{CaseNo}/{ReceiveNo}")]
        public IQueryable GetAI64_Web(string CaseNo, int ReceiveNo)
        {
            var data = SQLMethod.Search_AI64Web(CaseNo, ReceiveNo);
            return data.AsQueryable();
        }
        #endregion

        #region 查詢State_Web
        /// <summary>
        /// 查詢State_Web
        /// </summary>
        ///  <param name="CaseNo">案場編號</param>
        /// <param name="ReceiveNo">接收編號</param>
        /// <returns></returns>
        [Route("api/StateWeb/{CaseNo}/{ReceiveNo}")]
        public IQueryable GetState_Web(string CaseNo, int ReceiveNo)
        {
            var data = SQLMethod.Search_StateWeb(CaseNo, ReceiveNo);
            return data.AsQueryable();
        }
        #endregion
    }
}
