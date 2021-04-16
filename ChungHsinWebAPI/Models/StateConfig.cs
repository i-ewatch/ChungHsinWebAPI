using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// 狀態資訊
    /// </summary>
    public class StateConfig
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
        /// 狀態編號
        /// </summary>
        public int StateNo { get; set; }
        /// <summary>
        /// 陣列位址
        /// </summary>
        public int CharAddress { get; set; }
        /// <summary>
        /// 狀態位元位址
        /// </summary>
        public int StateBitAddress { get; set; }
        /// <summary>
        /// 狀態名稱
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// 狀態旗標 0 = DI狀態 ， 1 =DO狀態
        /// </summary>
        public bool StateFlag { get; set; }
        /// <summary>
        /// 狀態高位元
        /// </summary>
        public string StateHigh { get; set; }
        /// <summary>
        /// 狀態低位元
        /// </summary>
        public string StateLow { get; set; }
    }
}