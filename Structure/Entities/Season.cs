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

        public Season(String path, eOpenEpisodeOption getEpisode = eOpenEpisodeOption.getCode) : this() {
            
            ID = path[path.Length - 1].ToString();

            var episodeFiles = Directory
                .GetFiles(path, "*.xml")
                .ToList();

            episodeFiles.ForEach(ef =>
                insertInEpisodeList(ef, getEpisode));
        }

        private void insertInEpisodeList(String file, eOpenEpisodeOption getEpisode)
        {
            var fileInfo = new FileInfo(file);
            var episode = fileInfo.NameWithoutExtension();
            var season = fileInfo.Directory.Name.Replace("_","");
            var path = Directory.GetParent(fileInfo.DirectoryName).FullName;

            EpisodesList.Add(new EpisodeXML(path, season, episode, getEpisode).Episode);
        }


        public String ID { get; set; }
        public IList<Episode> EpisodesList { get; set; }



        public override String ToString()
        {
            return ID;
        }

    }
}
