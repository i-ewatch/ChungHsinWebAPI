using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChungHsinWebAPI.Enums
{
    /// <summary>
    /// 分析類型
    /// </summary>
    public enum AnalysisTypeEnum
    {
        /// <summary>
        /// 不解析
        /// </summary>
        None,
        /// <summary>
        /// ( L * 1 ) + ( H * 65536 )
        /// </summary>
        LH
    }
}