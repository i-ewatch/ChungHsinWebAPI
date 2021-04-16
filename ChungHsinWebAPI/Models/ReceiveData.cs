using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    public class ReceiveData
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
        /// 案場編號
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// AI編號
        /// </summary>
        public int RecNo { get; set; }
        /// <summary>
        /// 數值
        /// </summary>
        public int[] Ai { get; set; } = new int[128];
    }
}