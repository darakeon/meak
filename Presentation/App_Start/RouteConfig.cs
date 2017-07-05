using System.Web.Mvc;
using System.Web.Routing;
using Structure.Data;

namespace Presentation.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "LogOn", // Route name
                "Account/{action}", // URL with parameters
                new {controller = "Account", action = "LogOn"} // Parameter defaults
            );

            routes.MapRoute(
                "AuthorScene", // Route name
                "Author/{controller}/{seasonID}/{action}/{episodeID}/{sceneID}", // URL with parameters
                new
                    {
                        controller = "Season",
                        seasonID = UrlParameter.Optional,
                        action = "Index",
                        episodeID = UrlParameter.Optional,
                        sceneID = SceneXML.FirstScene
                    } // Parameter defaults
                );

            routes.MapRoute(
                "Author", // Route name
                "Author/{controller}/{seasonID}/{action}/{episodeID}", // URL with parameters
                new
                    {
                        controller = "Season",
                        seasonID = UrlParameter.Optional,
                        action = "Index",
                        episodeID = UrlParameter.Optional
                    } // Parameter defaults
                );



            routes.MapRoute(
                "Scene", // Route name
                "{controller}/{seasonID}/{action}/{episodeID}/{sceneID}", // URL with parameters
                new
                    {
                        controller = "Season",
                        seasonID = UrlParameter.Optional,
                        action = "Index",
                        episodeID = UrlParameter.Optional,
                        sceneID = SceneXML.FirstScene
                    } // Parameter defaults
                );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{seasonID}/{action}/{episodeID}", // URL with parameters
                new
                    {
                        controller = "Season",
                        seasonID = UrlParameter.Optional,
                        action = "Index",
                        episodeID = UrlParameter.Optional
                    } // Parameter defaults
                );
        }
    }
}