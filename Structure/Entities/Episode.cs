using System;
using System.Collections.Generic;
using System.IO;
using Ak.DataAccess.XML;

namespace Structure.Entities
{
    public class Episode
    {
        public Episode()
        {
            Season = new Season();
            SceneList = new List<Scene>();
        }

        public Episode(String path, String season, String episode) : this()
        {
            var titleXMLName = Path.Combine(path, "_" + season, episode, "_.xml");

            var titleXML = new Node(titleXMLName);

            Title = titleXML[0]["title"];

            ID = episode;

            Season = new Season { ID = season };
        }



        public String ID { get; set; }

        public String Title { get; set; }

        protected List<Scene> SceneList { get; set; }

        public Season Season { get; set; }



        public override String ToString()
        {
            return ID;
        }

    }
}
