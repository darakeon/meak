using System;
using Structure.Entities.System;

namespace Structure.Helpers
{
	public class RouteStories
	{
		internal static RouteStories Empty()
		{
			return new RouteStories(null, null, null);
		}

		private RouteStories(String seasonID, String episodeID, String sceneID)
		{
			SeasonID = seasonID;
			EpisodeID = episodeID;
			SceneID = sceneID;
		}

		public static RouteStories With(String seasonID, String episodeID, String sceneID)
		{
			return new RouteStories(seasonID, episodeID, sceneID);
		}

		public static RouteStories With(String seasonID, String episodeID)
		{
			return RouteStories.With(seasonID, episodeID, null);
		}

		public static RouteStories With(String seasonID)
		{
			return RouteStories.With(seasonID, null);
		}

		public static RouteStories With(Season season, Episode episode, Scene scene)
		{
			return RouteStories.With(season.ID, episode.ID, scene.ID);
		}

		public static RouteStories With(Season season, Episode episode)
		{
			return RouteStories.With(season.ID, episode.ID);
		}

		public static RouteStories With(Season season)
		{
			return RouteStories.With(season.ID);
		}

		public override string ToString()
		{
			return SeasonID + EpisodeID + SceneID + ".meak";
		}

		public String SeasonID { get; }
		public String EpisodeID { get; }
		public String SceneID { get; }
	}
}