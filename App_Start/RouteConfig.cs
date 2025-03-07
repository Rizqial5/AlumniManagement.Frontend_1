using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlumniManagement.Frontend
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes    )
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "JobPosting", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "EditJobPosting",
                url: "JobPosting/Edit/{guid}",
                defaults: new { controller = "JobPosting", action = "Edit" },
                constraints: new { guid = @"^(\{?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}\}?)$" }
            );
        }
    }
}
