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
			
			var sceneLetters = Paths.SceneLetters(PathJson, seasonID, episodeID);

			foreach (var sceneLetter in sceneLetters)
			{
				var xml = getScene(seasonID, episodeID, sceneLetter);

				episode.SceneList.Add(xml.Scene);
			}

			return episode;
		}




		private SceneJson getScene(String seasonID, String episodeID, String sceneID)
		{
			try
			{
				return new SceneJson(PathJson, seasonID, episodeID, sceneID, OpenEpisodeOption.GetStory);
			}
			catch (FileNotFoundException)
			{
				throw new StoriesException($"Arquivo não encontrado(s):{seasonID}{episodeID}{sceneID}.");
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
