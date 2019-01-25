using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Data;
using Structure.Entities.System;

namespace Structure.Helpers
{
	public class Countdown
	{
		private static readonly 
			IDictionary<String, DateTime> dates = getDates();

		private static readonly String empty = "???";

		private static IDictionary<String, DateTime> getDates()
		{
			var dic = new Dictionary<String, DateTime>();

			for (var s = 'A'; s <= 'Z'; s++)
			{
				for (var e = 1; e <= 20; e++)
				{
					var code = s + e.ToString("00");
					var date = getDate(s, e);

					dic.Add(code, date);
				}
			}

			return dic;
		}

		public static DateTime GetDate(String season, String episode)
		{
			return getDate(season[0], Int32.Parse(episode));
		}

		private static DateTime getDate(Char season, Int32 episode)
		{
			var year = (season - 'A') * 3 / 2 + 2015;

			episode--;
			var day = (episode / 5 * 3 + episode * 2) * 7 + 26;

			return new DateTime(year, 3, 1).AddDays(day);
		}

		public static string GetTimeLeft(EpisodeJson episodeJson, IList<Season> seasonList)
		{
			var lastSeason = seasonList.LastOrDefault();

			if (lastSeason == null) return empty;

			var lastIndex = lastSeason.EpisodeList.Count - 1;

			while (lastIndex >= 0)
			{
				var episode = lastSeason.EpisodeList[lastIndex];

				var seasonId = episode.Season.ID;
				var episodeId = episode.ID;
				episode = episodeJson.GetEpisode(seasonId, episodeId);

				if (isComplete(episode))
					return getTimeLeft(seasonId, episodeId);

				lastIndex--;
			}

			return empty;
		}

		private static Boolean isComplete(Episode episode)
		{
			var lastScene = episode.SceneList.LastOrDefault();

			return episode.LastScene == lastScene?.ID
			       && lastScene?.ParagraphCount
			       >= SceneJson.MINIMUM_PARAGRAPH_COUNT;
		}

		private static String getTimeLeft(String lastSeason, String lastEpisode)
		{
			var code = lastSeason + lastEpisode;

			var episodes = dates.Keys.ToList();
			var lastDateIndex = episodes.IndexOf(code);
			var nextDateIndex = episodes[lastDateIndex + 1];

			var nextDate = dates[nextDateIndex];
			var diff = nextDate - DateTime.Today;

			return diff.TotalDays.ToString("00");
		}
	}
}
