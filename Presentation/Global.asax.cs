using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using Ak.DataAccess.XML;
using Ak.Generic.Exceptions;
using Structure.Data;

namespace Presentation
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
                "LogOn", // Route name
                "Account/{action}", // URL with parameters
                new { controller = "Account", action = "LogOn" } // Parameter defaults
            );



            routes.MapRoute(
                "AuthorScene", // Route name
                "Author/{controller}/{season}/{action}/{episode}/{scene}", // URL with parameters
                new { controller = "Season", season = UrlParameter.Optional, action = "Index", episode = UrlParameter.Optional, scene = SceneXML.FirstScene } // Parameter defaults
            );

            routes.MapRoute(
                "Author", // Route name
                "Author/{controller}/{season}/{action}/{episode}", // URL with parameters
                new { controller = "Season", season = UrlParameter.Optional, action = "Index", episode = UrlParameter.Optional } // Parameter defaults
            );



            routes.MapRoute(
                "Scene", // Route name
                "{controller}/{season}/{action}/{episode}/{scene}", // URL with parameters
                new { controller = "Season", season = UrlParameter.Optional, action = "Index", episode = UrlParameter.Optional, scene = SceneXML.FirstScene } // Parameter defaults
            );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{season}/{action}/{episode}", // URL with parameters
                new { controller = "Season", season = UrlParameter.Optional, action = "Index", episode = UrlParameter.Optional } // Parameter defaults
            );
        }

        // ReSharper disable InconsistentNaming
        protected void Application_Start()
        // ReSharper restore InconsistentNaming
        {
            //Work-a-round suggested in 
            //http://weblogs.asp.net/scottgu/archive/2010/12/14/update-on-asp-net-mvc-3-rc2-and-a-workaround-for-a-bug-in-it.aspx
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

    }
}