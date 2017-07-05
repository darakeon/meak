using System;
using Presentation.Models.General;

namespace Presentation.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            LogOn = new LogOnModel();
        }

        public BaseModel(Paths paths) : this()
        {
            Menu = new MenuModel(paths.Xml);
            Css = new CssModel(paths.Css);
        }

        public MenuModel Menu { get; set; }
        public LogOnModel LogOn { get; set; }
        public CssModel Css { get; set; }



        public class Paths
        {
            public Paths(String xmlPath, String cssPath)
            {
                Xml = xmlPath;
                Css = cssPath;
            }

            public String Xml { get; private set; }
            public String Css { get; private set; }
        }
    }

}