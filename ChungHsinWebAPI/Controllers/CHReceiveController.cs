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
    /// 冰水主機接收
    /// </summary>
    public class CHReceiveController : ApiController
    {
        private SQLMethod SQLMethod = new SQLMethod();
        /// <summary>
        /// 接收數值
        /// </summary>
        /// <param name="receiveData"></param>
        /// <returns></returns>
        [Route("api/CHRecive")]
        public IHttpActionResult PostRecive(ReceiveData receiveData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);          // 400 Bad Request
            }
            #region 紀錄Log
            var Receive_Procedure_Respons = SQLMethod.Receive_Procedure(receiveData);
            switch (Receive_Procedure_Respons)
            {
                case 0:
                    {
                        return Ok(SQLMethod.Receive_Procedure_ErrorStr); ;
                    }
                case 1:
                    {
                        return Ok(SQLMethod.Receive_Procedure_ErrorStr);
                    }
                default:
                    {
                        return BadRequest(SQLMethod.Receive_Procedure_ErrorStr);
                    }
            }
            #endregion

        }
    }
}
