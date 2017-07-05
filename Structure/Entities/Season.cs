using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Enums;
using Structure.Data;
using FileInfoExtension = Structure.Extensions.FileInfoExtension;

namespace Structure.Entities
{
    public class Season
    {
        public Season() {
            EpisodesList = new List<Episode>();
        }

        public Season(String path, OpenEpisodeOption getEpisode = OpenEpisodeOption.GetCode) : this() {
            
            ID = path[path.Length - 1].ToString();

            var episodeFiles = Directory
                .GetFiles(path, "*.xml")
                .ToList();

            episodeFiles.ForEach(ef =>
                insertInEpisodeList(ef, getEpisode));
        }

        private void insertInEpisodeList(String file, OpenEpisodeOption getEpisode)
        {
            var fileInfo = new FileInfo(file);
            var episode = FileInfoExtension.NameWithoutExtension(fileInfo);
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
