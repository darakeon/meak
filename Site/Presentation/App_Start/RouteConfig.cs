using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Scene",
				"{seasonID}-{episodeID}-{sceneID}",
				new { controller = "Season", action = "Episode" },
				new { seasonID = "[A-Z]", episodeID = "\\d{2}", sceneID = "[a-z]" }
			);

			routes.MapRoute(
				"Episode",
				"{seasonID}-{episodeID}",
				new { controller = "Season", action = "Episode" },
				new { seasonID = "[A-Z]", episodeID = "\\d{2}" }
			);

			routes.MapRoute(
				"Season",
				"{seasonID}",
				new { controller = "Season", action = "Index" },
				new { seasonID = "[A-Z]" }
			);

			routes.MapRoute(
				"Others",
				"{controller}/{action}",
				new { controller = "Season", action = "Index" }
			);
		}
	}
}
