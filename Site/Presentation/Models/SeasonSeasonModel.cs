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

			if (!Authenticate.IsAuthenticated)
			{
				EpisodeList = EpisodeList
					.Where(e => e.IsPublished())
					.Reverse().Skip(1).Reverse()
                    .Where(e => e.HasSummary())
					.ToList();
			}
	    }

	    public Season Season { get; set; }
        public IList<Episode> EpisodeList { get; set; }

    }
}