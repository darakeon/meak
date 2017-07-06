using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Authentication;
using Structure.Data;
using Structure.Entities;

namespace Presentation.Models
{
	public class SeasonSeasonModel : BaseModel
	{
		public SeasonSeasonModel(String season)
		{
			var pathXml = Paths.SeasonPath(Paths.Xml, season); 
			
			Season = new Season(pathXml);
	        EpisodeList = Season.EpisodeList;

			if (!Authenticate.IsAuthenticated && EpisodeList.Any())
			{
				var publishedEpisodes = EpisodeList
					.Where(e => e.IsPublished()).ToList();

				var lastEpisode = publishedEpisodes.Last();

				if (lastEpisode.Publish.Year == DateTime.Today.Year)
				{
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