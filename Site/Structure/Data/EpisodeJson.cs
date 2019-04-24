using System;
using System.IO;
using Structure.Entities.System;
using Structure.Enums;
using Structure.Helpers;

namespace Structure.Data
{
	public class EpisodeJson
	{
		private Episode episode { get; set; }
		public String PathJson { get; private set; }


		public EpisodeJson()
		{
			setJsonPath();
		}



		public Episode GetEpisode(String seasonID, String episodeID)
		{
			episode = new Episode(PathJson, seasonID, episodeID);
			
			var blockLetters = Paths.BlockLetters(PathJson, seasonID, episodeID);

			foreach (var blockLetter in blockLetters)
			{
				var json = getBlock(seasonID, episodeID, blockLetter);

				if (json.Block.ParagraphTypeList.Count > 0)
					episode.BlockList.Add(json.Block);
			}

			return episode;
		}




		private BlockJson getBlock(String seasonID, String episodeID, String blockID)
		{
			try
			{
				return new BlockJson(PathJson, seasonID, episodeID, blockID, OpenEpisodeOption.GetStory);
			}
			catch (FileNotFoundException)
			{
				throw new StoriesException($"Arquivo não encontrado(s):{seasonID}{episodeID}{blockID}.");
			}
			catch (Exception e)
			{
				throw new StoriesException(e.Message);
			}
		}

		
		private void setJsonPath()
		{
			var folder = Config.StoriesPath;

			if (folder == null)
				throw new Exception("Json Path not configured.");

			PathJson =
				folder.Substring(1, 1) == ":"
					? folder
					: Path.Combine(Directory.GetCurrentDirectory(), folder);

			if (!Directory.Exists(PathJson))
				throw new Exception($"Path '{PathJson}' doesn't exists.");
		}

	}
}
