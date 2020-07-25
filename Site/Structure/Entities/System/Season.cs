using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Structure.Entities.System
{
	public class Season
	{
		public Season() {
			EpisodeList = new List<Episode>();
		}

		public Season(String path) : this()
		{
			ID = path[path.Length - 1].ToString();

			var json = File.ReadAllText(
				Path.Combine(path, "_.json")
			);
			var info = JsonConvert.DeserializeObject<Season>(json);
			Name = info.Name;

			var episodeFiles = Directory
				.GetDirectories(path)
				.ToList();

			episodeFiles.ForEach(insertInEpisodeList);
		}

		private void insertInEpisodeList(String file)
		{
			var dir = new DirectoryInfo(file);

			if (dir.Parent?.Parent == null)
				return;

			var episodeName = dir.Name;

			var seasonName = dir.Parent.Name.Replace("_", "");
			var path = dir.Parent.Parent.FullName;

			var episode = Episode.Get(path, seasonName, episodeName);

			if (episode != null && episode.IsPublished())
				EpisodeList.Add(episode);
		}


		public String ID { get; set; }
		public String Name { get; set; }
		public IList<Episode> EpisodeList { get; set; }

		public override String ToString()
		{
			return ID;
		}
	}
}
