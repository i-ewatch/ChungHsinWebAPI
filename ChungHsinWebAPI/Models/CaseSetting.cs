using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Models
{
    /// <summary>
    /// 案場資訊
    /// </summary>
    public class CaseSetting
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
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 聯絡人
        /// </summary>
        public string Contacter { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 案場名稱
        /// </summary>
        public string TitleName { get; set; }
        /// <summary>
        /// 經度
        /// </summary>
        public float Longitude { get; set; }
        /// <summary>
        /// 緯度
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// 地圖放大倍率
        /// </summary>
        public int zoom { get; set; }
        /// <summary>
        /// 告警發報類型 0 = 不使用,1 = LineNotify，2 = Telegrambot
        /// </summary>
        public int NotifyTypeEnum { get; set; }
        /// <summary>
        /// 告警發報網址  LineNotify不需輸入，Telegrambot API網址
        /// </summary>
        public string NotifyApi { get; set; }
        /// <summary>
        /// 告警發報權杖 Line的Token，Telegrambot的Chat_ID
        /// </summary>
        public string NotifyToken { get; set; }
    }
}