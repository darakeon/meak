using System;
using System.IO;
using Presentation.Models.General;
using Structure.Data;
using Structure.Helpers;

namespace Presentation.Models
{
	public class BaseModel
	{
		public BaseModel()
		{
			LogOn = new LogOnModel();

			episodeJson = new EpisodeJson();

			var jsonPath = episodeJson.PathJson;

			var cssPath = Path.Combine(
				Directory.GetCurrentDirectory(),
				"Assets",
				"css"
			);

			Paths = new Paths(jsonPath, cssPath);

			Menu = new MenuModel(Paths.Json);
			Css = new CssModel(Paths.Css);
		}

		public MenuModel Menu { get; set; }
		public LogOnModel LogOn { get; set; }
		public CssModel Css { get; set; }

		public Paths Paths { get; }
		protected EpisodeJson episodeJson { get; }

		public String TimeLeft =>
			Countdown.GetTimeLeft(episodeJson, Menu.SeasonList);
	}
}
