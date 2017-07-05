using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Data;

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
            ID = episode;
            Title = TitleXML.Get(path, season, episode);
            Season = new Season { ID = season };
        }



        public String ID { get; set; }

        public String Title { get; set; }

        public List<Scene> SceneList { get; set; }

        public Season Season { get; set; }



        public override String ToString()
        {
            return ID;
        }

        
        public Scene this[String scene]
        {
            get { return SceneList.SingleOrDefault(s => s.ID == scene); }
        }







    }
}
