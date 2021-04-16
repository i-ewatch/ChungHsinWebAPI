using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// 設備AI設定
    /// </summary>
    public class AISetting
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
        public int RecevieNo { get; set; }
        /// <summary>
        /// 數值編號
        /// </summary>
        public string AINo { get; set; }
        /// <summary>
        /// 比較旗標
        /// </summary>
        public bool CompareFlag { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Max { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Min { get; set; }
        /// <summary>
        /// 紀錄最後發報比較類型 0 = 超過上限,1=正常,2 = 低於下限
        /// </summary>
        public bool CompareType { get; set; }
        /// <summary>
        /// 類型旗標
        /// </summary>
        public bool EnumFlag { get; set; }
        /// <summary>
        /// 紀錄最後發報類型
        /// </summary>
        public int EnumType { get; set; }
    }
}