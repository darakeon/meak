using System;
using System.Collections.Generic;
using System.Linq;
using Structure;
using Structure.Data;
using Structure.Entities.System;
using Structure.Helpers;

namespace Presentation.Models
{
	public class SeasonSeasonModel : BaseModel
	{
		public SeasonSeasonModel(String season)
		{
			var seasonPath = Paths.SeasonPath(Paths.Json, season); 
			
			Season = new Season(seasonPath);
			EpisodeList = Season.EpisodeList;

			if (!Config.IsAuthor && EpisodeList.Any())
			{
				var publishedEpisodes = EpisodeList
					.Where(e => e.IsPublished()).ToList();

				var nextSeason = ((char)(season[0] + 1)).ToString();
				var nextSeasonExists = Paths.SeasonPathExists(Paths.Json, nextSeason);

				if (!nextSeasonExists)
				{
					var lastEpisode = publishedEpisodes.Last();
					publishedEpisodes.Remove(lastEpisode);
				}

				EpisodeList = publishedEpisodes
					.Where(e => e.HasSummary())
					.ToList();
			}
		}

		public Season Season { get; set; }
		public IList<Episode> EpisodeList { get; set; }
	}
}
