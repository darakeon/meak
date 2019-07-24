using System;
using System.Collections.Generic;
using System.Linq;
using DK.MVC.Authentication;
using Structure.Data;
using Structure.Helpers;

namespace Structure.Entities.System
{
	public class Episode
	{
		public Episode()
		{
			Season = new Season();
			BlockList = new List<Block>();
		}

		public static Episode Get(String path, String season, String episode)
		{
			var info = MainInfoJson.Get(path, season, episode);

			if (info == null)
				return null;

			return new Episode
			{
				ID = episode,
				Title = info.Title,
				Publish = Countdown.GetDate(season, episode),
				LastBlock = info.Last,
				Summary = info.Summary,
				Breaks = info.Breaks,
				PageStart = info.Page,
				Season = new Season {ID = season},
			};
		}

		public String ID { get; set; }

		public String Title { get; set; }
		public DateTime Publish { get; set; }
		public String LastBlock { get; set; }
		public String Summary { get; set; }
		public Int32? Breaks { get; set; }

		public Int16? PageStart { get; set; }

		public List<Block> BlockList { get; set; }

		public Season Season { get; set; }

		public Boolean IsPublished()
		{
			return Publish < DateTime.Now || Authenticate.IsAuthenticated;
		}

		public bool HasSummary()
		{
			return !String.IsNullOrEmpty(Summary) || Authenticate.IsAuthenticated;
		}

		public override String ToString()
		{
			return ID;
		}
		
		public Block this[String block]
		{
			get { return BlockList.SingleOrDefault(s => s.ID == block); }
		}
	}
}
