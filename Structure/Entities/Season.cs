using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Enums;
using Structure.Data;

namespace Structure.Entities
{
    public class Season
    {
        public Season() {
            this.EpisodesList = new List<Episode>();
        }

        public Season(String Path, eOpenEpisodeOption getEpisode = eOpenEpisodeOption.getCode) : this() {
            
            this.ID = Path[Path.Length - 1].ToString();

            var episodes = Directory
                .GetFiles(Path, "*.xml")
                .ToList();

            episodes.ForEach(
                f => this.EpisodesList.Add(new EpisodeXML(f, getEpisode).Episode)
            );
        }



        public String ID { get; set; }
        public IList<Episode> EpisodesList { get; set; }



        public override String ToString()
        {
            return this.ID.ToString();
        }
    }
}
