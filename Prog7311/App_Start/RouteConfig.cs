using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Prog7311
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "AddFarmer",
            url: "Farmer/AddFarmer",
            defaults: new { controller = "Farmer", action = "AddFarmer", id = UrlParameter.Optional }
        );
            // Add route for product form
            routes.MapRoute(
                name: "AddProduct",
                url: "Farmer/{farmerId}/addproduct",
                defaults: new { controller = "Farmer", action = "AddProduct", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LogOut",
                url: "account/logout",
                defaults: new { controller = "AccountController",action = "Logout", }
            );
            routes.MapRoute(
            name: "Index",
            url: "User/Index",
            defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
            name: "User",
            url: "User",
             defaults: new { controller = "User", action = "Index" }
);
            routes.MapRoute(
            name: "Farmer",
            url: "Farmer",
             defaults: new { controller = "Farmer", action = "AddFarmer" }
);
        }
    }
}
