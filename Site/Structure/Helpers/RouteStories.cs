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

		private RouteStories(String seasonID, String episodeID, String blockID)
		{
			SeasonID = seasonID;
			EpisodeID = episodeID;
			BlockID = blockID;
		}

		public static RouteStories With(String seasonID, String episodeID, String blockID)
		{
			return new RouteStories(seasonID, episodeID, blockID);
		}

		public static RouteStories With(String seasonID, String episodeID)
		{
			return RouteStories.With(seasonID, episodeID, null);
		}

		public static RouteStories With(String seasonID)
		{
			return RouteStories.With(seasonID, null);
		}

		public static RouteStories With(Season season, Episode episode, Block block)
		{
			return RouteStories.With(season.ID, episode.ID, block.ID);
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
			return SeasonID + EpisodeID + BlockID + ".meak";
		}

		public String SeasonID { get; }
		public String EpisodeID { get; }
		public String BlockID { get; }
	}
}