using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PersonlaFinance
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MFDashboard", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MutualFunds",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MutualFunds", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}