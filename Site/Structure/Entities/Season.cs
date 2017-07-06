using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Entities
{
    public class Season
    {
        public Season() {
            EpisodeList = new List<Episode>();
        }

        public Season(String path) : this()
        {
            ID = path[path.Length - 1].ToString();

            var episodeFiles = Directory
                .GetDirectories(path)
                .ToList();

            episodeFiles.ForEach(insertInEpisodeList);
        }

        private void insertInEpisodeList(String file)
        {
            var dir = new DirectoryInfo(file);

	        if (dir.Parent == null || dir.Parent.Parent == null)
				return;

	        var episodeName = dir.Name;

	        var seasonName = dir.Parent.Name.Replace("_", "");
	        var path = dir.Parent.Parent.FullName;

	        var episode = new Episode(path, seasonName, episodeName);

	        if (episode.IsPublished())
		        EpisodeList.Add(episode);
        }


        public String ID { get; set; }
        public IList<Episode> EpisodeList { get; set; }



        public override String ToString()
        {
            return ID;
        }

    }
}
