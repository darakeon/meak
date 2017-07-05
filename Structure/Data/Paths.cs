using System;

namespace Structure.Data
{
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
