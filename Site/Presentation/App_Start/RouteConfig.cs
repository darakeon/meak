using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Structure.Entities.System;

namespace Presentation
{
	public class RouteConfig
	{
		public enum Names
		{
			Main,

			AuthorScene,
			AuthorEpisode,
			Upload,
			LogOn,

			Scene,
			Episode,
			Season,

			AddEpisode,
            EditTitle,
			EditScene,
			Adder
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(Names.AuthorScene, "Season", "Episode", "_§{seasonID}[{episodeID}][{sceneID}]");
			routes.MapRoute(Names.AuthorEpisode, "Season", "Episode", "_§{seasonID}[{episodeID}]");
			routes.MapRoute(Names.Upload, "Upload", "Upload", "_~§{seasonID}[{episodeID}][{sceneID}]");
			routes.MapRoute(Names.LogOn, "Account", "LogOn", "_~");

			routes.MapRoute(Names.Scene, "Season", "Episode", "§{seasonID}[{episodeID}][{sceneID}]");
			routes.MapRoute(Names.Episode, "Season", "Episode", "§{seasonID}[{episodeID}]");
			routes.MapRoute(Names.Season, "Season", "Index", "§{seasonID}");

			routes.MapRoute(Names.AddEpisode, "Season", "AddEpisode", "_§~");
			routes.MapRoute(Names.EditTitle, "Season", "EditTitle", "_§{seasonID}[{episodeID}]~");
			routes.MapRoute(Names.EditScene, "Season", "EditScene", "_§{seasonID}[{episodeID}][{sceneID}]~");
			routes.MapRoute(Names.Adder, "Season", "Adder", "_[{sceneID}]~");

			routes.MapRoute(Names.Main, "Season", "Index", "{controller}/{action}");
		}


	}

	public static class UrlExtend
	{
		public static Route MapRoute(this RouteCollection collection, RouteConfig.Names name, String controller, String action, String url)
		{
			var constraints = getContraints(url);

			return constraints == null
				? collection.MapRoute(name.ToString(), url, new {controller, action})
				: collection.MapRoute(name.ToString(), url, new {controller, action}, constraints);
		}

		private static object getContraints(string url)
		{
			if (!url.Contains("seasonID"))
				return null;

			if (!url.Contains("episodeID"))
				return new { seasonID = "[A-Z]" };

			if (!url.Contains("sceneID"))
				return new { seasonID = "[A-Z]", episodeID = "[0-2][0-9]" };
			
			return new { seasonID = "[A-Z]", episodeID = "[0-2][0-9]", sceneID = "[a-g_]" };
		}





		public static string Route(this UrlHelper url, RouteConfig.Names name)
		{
			return url.Route(name, null);
		}

		public static string Route(this UrlHelper url, RouteConfig.Names name, String seasonId)
		{
			return url.Route(name, seasonId, null);
		}

		public static string Route(this UrlHelper url, RouteConfig.Names name, String seasonId, String episodeId)
		{
			return url.Route(name, seasonId, episodeId, null);
		}

		public static string Route(this UrlHelper url, RouteConfig.Names name, String seasonId, String episodeId, String sceneId)
		{
			var urlQuery = url.RouteUrl(name.ToString(), new { seasonId, episodeId, sceneId });

			return urlQuery?.Split(new []{'?'}, 2)[0];
		}

		public static string Route(this UrlHelper url, RouteConfig.Names name, Season season, Episode episode)
		{
			return url.Route(name, season.ToString(), episode.ToString());
		}





		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name)
		{
			return html.Link(text, name, null);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, object htmlAttributes)
		{
			return html.RouteLink(text, name.ToString(), htmlAttributes);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId)
		{
			return html.Link(text, name, seasonId, null);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId, object htmlAttributes)
		{
			return html.RouteLink(text, name.ToString(), new { seasonId }, htmlAttributes);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, Season season, Episode episode)
		{
			return html.Link(text, name, season, episode, null);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, Season season, Episode episode, object htmlAttributes)
		{
			return html.Link(text, name, season.ToString(), episode.ToString(), htmlAttributes);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId, String episodeId)
		{
			return html.Link(text, name, seasonId, episodeId, null);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId, String episodeId, object htmlAttributes)
		{
			return html.RouteLink(text, name.ToString(), new { seasonId, episodeId }, htmlAttributes);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId, String episodeId, String sceneId)
		{
			return html.Link(text, name, seasonId, episodeId, sceneId, null);
		}

		public static MvcHtmlString Link(this HtmlHelper html, String text, RouteConfig.Names name, String seasonId, String episodeId, String sceneId, object htmlAttributes)
		{
			return html.RouteLink(text, name.ToString(), new { seasonId, episodeId, sceneId }, htmlAttributes);
		}

	}
}