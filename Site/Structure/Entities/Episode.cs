using System;
using System.Collections.Generic;
using System.Linq;
using Ak.MVC.Authentication;
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
            var info = MainInfoXML.Get(path, season, episode);

            ID = episode;
			Title = info.Title;
			Publish = info.Publish;
			LastScene = info.LastScene;
			Summary = info.Summary;
			Season = new Season { ID = season };
        }



        public String ID { get; set; }

        public String Title { get; set; }
		public DateTime Publish { get; set; }
		public String LastScene { get; set; }
		public String Summary { get; set; }

        public List<Scene> SceneList { get; set; }

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

        
        public Scene this[String scene]
        {
            get { return SceneList.SingleOrDefault(s => s.ID == scene); }
        }



    }
}
