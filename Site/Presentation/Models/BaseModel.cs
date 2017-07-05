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

            EpisodeXML = new EpisodeXML();
            MessageXML = new MessageXML();

            var xmlPath = EpisodeXML.PathXML;
            var cssPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "css");

            Paths = new Paths(xmlPath, cssPath);

            Menu = new MenuModel(Paths.Xml);
            Css = new CssModel(Paths.Css);
        }


        public MenuModel Menu { get; set; }
        public LogOnModel LogOn { get; set; }
        public CssModel Css { get; set; }

        public Paths Paths { get; private set; }
        protected EpisodeXML EpisodeXML { get; private set; }
        protected MessageXML MessageXML { get; private set; }



    }

}