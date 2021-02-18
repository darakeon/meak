using Microsoft.AspNetCore.Builder;

namespace Presentation.Startup
{
	static class Route
	{
		public static void Config(IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"Block",
					"{seasonID}-{episodeID}-{blockID}",
					new { controller = "Season", action = "Episode" },
					new { seasonID = "[A-Z]", episodeID = "\\d{2}", blockID = "[a-z]" }
				);

				endpoints.MapControllerRoute(
					"Episode",
					"{seasonID}-{episodeID}",
					new { controller = "Season", action = "Episode" },
					new { seasonID = "[A-Z]", episodeID = "\\d{2}" }
				);

				endpoints.MapControllerRoute(
					"Season",
					"{seasonID}",
					new { controller = "Season", action = "Index" },
					new { seasonID = "[A-Z]" }
				);

				endpoints.MapControllerRoute(
					"Others",
					"{controller}/{action}",
					new { controller = "Season", action = "Index" }
				);
			});
		}
	}
}
