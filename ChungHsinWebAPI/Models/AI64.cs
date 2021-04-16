using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// AI64功能
    /// </summary>
    public class AI64
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
        /// 接收編號
        /// </summary>
        public int ReceiveNo { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai1 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai2 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai3 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai4 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai5 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai6 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai7 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai8 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai9 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai10 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai11 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai12 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai13 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai14 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai15 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai16 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai17 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai18 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai19 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai20 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai21 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai22 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai23 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai24 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai25 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai26 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai27 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai28 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai29 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai30 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai31 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai32 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai33 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai34 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai35 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai36 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai37 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai38 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai39 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai40 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai41 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai42 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai43 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai44 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai45 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai46 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai47 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai48 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai49 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai50 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai51 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai52 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai53 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai54 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai55 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai56 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai57 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai58 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai59 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai60 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai61 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai62 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai63 { get; set; }
        /// <summary>
        /// AI點位
        /// </summary>
        public decimal Ai64 { get; set; }

    }
}