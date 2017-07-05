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
                    action = "Index",
                    seasonID = UrlParameter.Optional,
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
                    action = "Index",
                    seasonID = UrlParameter.Optional,
                    episodeID = UrlParameter.Optional
                } // Parameter defaults
            );



            routes.MapRoute(
                "Upload", // Route name
                "Upload/{seasonID}/{episodeID}/{sceneID}", // URL with parameters
                new
                {
                    controller = "Upload",
                    action = "Upload",
                }, // Parameter defaults
                new
                {
                    seasonID = "[A-Z]",
                    episodeID = "[0-2][0-9]",
                    sceneID = "[a-z]"
                }
            );


            routes.MapRoute(
                "Scene", // Route name
                "{controller}/{seasonID}/{action}/{episodeID}/{sceneID}", // URL with parameters
                new
                {
                    controller = "Season",
                    action = "Index",
                    seasonID = UrlParameter.Optional,
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
                    action = "Index",
                    seasonID = UrlParameter.Optional,
                    episodeID = UrlParameter.Optional
                } // Parameter defaults
            );
        }
    }
}