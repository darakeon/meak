using System;
using System.Collections.Generic;
using Structure.Data;
using Structure.Entities;
using Structure.Enums;

namespace Presentation.Models
{
	public class SeasonSeasonModel : BaseModel
	{
		public SeasonSeasonModel(Paths paths, String season) : base(paths)
		{
			var pathXml = Paths.SeasonPath(paths.Xml, season); 
			
			Season = new Season(pathXml, OpenEpisodeOption.GetTitle);
			EpisodeList = Season.EpisodesList;
		}

		public Season Season { get; set; }

		public IList<Episode> EpisodeList { get; set; }
	}
}