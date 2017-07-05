using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Structure.Data;
using Structure.Entities;
using Structure.Enums;

namespace Presentation.Models
{
	public class SeasonSeasonModel : BaseModel
	{
		public SeasonSeasonModel(String season)
		{
			var pathXml = Paths.SeasonPath(Paths.Xml, season); 
			
			Season = new Season(pathXml, OpenEpisodeOption.GetTitle);
		    
            setEpisodeList(Paths, season);
		}

	    private void setEpisodeList(Paths paths, String season)
	    {
	        EpisodeList = Season.EpisodeList;

            if (EpisodeList.Any(e => e.Summary == ""))
            {
                EpisodeList = EpisodeList
                    .Where(e => e.Summary != "")
                    .ToList();

                return;
            }

	        var currentLetter = Encoding.UTF8.GetBytes(season);
	        var nextLetter = currentLetter[0];
	        nextLetter++;
	        var next = Encoding.UTF8.GetString(new [] { nextLetter });
	        var nextPathXml = Paths.SeasonPath(paths.Xml, next);

	        if (!Directory.Exists(nextPathXml))
	        {
	            EpisodeList = EpisodeList
	                .Take(EpisodeList.Count - 1)
	                .ToList();
	        }
	    }

	    public Season Season { get; set; }
        public IList<Episode> EpisodeList { get; set; }

    }
}