using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Enums;

namespace Structure.Entities
{
    public class Season
    {
        public Season() {
            EpisodesList = new List<Episode>();
        }

        public Season(String path, OpenEpisodeOption getEpisode = OpenEpisodeOption.GetCode) : this()
        {
            ID = path[path.Length - 1].ToString();

            var episodeFiles = Directory
                .GetDirectories(path)
                .ToList();

            episodeFiles.ForEach(ef =>
                insertInEpisodeList(ef, getEpisode));
        }

        private void insertInEpisodeList(String file, OpenEpisodeOption getEpisode)
        {
            var dir = new DirectoryInfo(file);

            var episode = dir.Name;
            var season = dir.Parent.Name.Replace("_", "");
            var path = dir.Parent.Parent.FullName;

            EpisodesList.Add(new Episode(path, season, episode));
        }


        public String ID { get; set; }
        public IList<Episode> EpisodesList { get; set; }



        public override String ToString()
        {
            return ID;
        }

    }
}
