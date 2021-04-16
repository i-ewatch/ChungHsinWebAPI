using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// 狀態
    /// </summary>
    public class State
    {
        private string _ttime;
        /// <summary>
        /// 時間
        /// </summary>
        public string ttime
        {
            get { return _ttime; }
            set
            {
                if (value.Length > 14)
                    _ttime = value.Substring(0, 14);
                else
                    _ttime = value;
            }
        }
        /// <summary>
        /// 時間字串
        /// </summary>
        public DateTime ttimen { get; set; }
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
        /// 狀態
        /// </summary>
        public bool state { get; set; }
    }
}