using System.IO;
using Presentation.Models.General;
using Structure.Data;

namespace Presentation.Models
{
	public class BaseModel
	{
		public BaseModel()
		{
			LogOn = new LogOnModel();

			episodeJson = new EpisodeJson();
			messageJson = new MessageJson();

			var xmlPath = episodeJson.PathJson;
			var cssPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "css");

			Paths = new Paths(xmlPath, cssPath);

			Menu = new MenuModel(Paths.Json);
			Css = new CssModel(Paths.Css);
		}

		public MenuModel Menu { get; set; }
		public LogOnModel LogOn { get; set; }
		public CssModel Css { get; set; }

		public Paths Paths { get; }
		protected EpisodeJson episodeJson { get; }
		protected MessageJson messageJson { get; }

	}
}
