using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// 設備狀態設定
    /// </summary>
    public class StateSetting
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 案場編號
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// 接收編號
        /// </summary>
        public int ReceiveNo { get; set; }
        /// <summary>
        /// 狀態編號
        /// </summary>
        public int StateNo { get; set; }
        /// <summary>
        /// 狀態改變發送旗標
        /// </summary>
        public bool NotifyFlag { get; set; }
        /// <summary>
        /// 最後狀態 
        /// </summary>
        public bool LastStateFlag { get; set; }
    }
}