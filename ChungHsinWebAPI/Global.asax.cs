using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ChungHsinWebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Application["DB"] = "ChungHsinDB";
            Application["Log"] = "ChungHsinLog";
            Application["Web"] = "ChungHsinWeb";
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
