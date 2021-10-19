using System;
using System.IO;
using System.Linq;

namespace Structure.Data
{
	public class Paths
	{
		public Paths(String jsonPath, String cssPath)
		{
			Json = jsonPath;
			Css = cssPath;
		}

		public String Json { get; }
		public String Css { get; }


		public static String SeasonPath(String folderPath, String seasonID)
		{
			return Path.Combine(folderPath, "_" + seasonID);
		}

		public static Boolean SeasonPathExists(String folderPath, String seasonID)
		{
			return Directory.Exists(SeasonPath(folderPath, seasonID));
		}

		public static String EpisodePath(String folderPath, String seasonID, String episodeID)
		{
			return Path.Combine(folderPath, "_" + seasonID, episodeID);
		}

        public static String BlockFilePath(String folderPath, String seasonID, String episodeID, String blockID)
        {
            return Path.Combine(folderPath, "_" + seasonID, episodeID, blockID + ".json");
        }

        public static String AudioPath(String folderPath, String seasonID, String episodeID, String audio)
        {
            return Path.Combine(folderPath, "_" + seasonID, episodeID, audio + ".mp3");
        }

        public static String AudioLocalPath(String seasonID, String episodeID, String audio)
        {
            return Path.Combine("songs", "_" + seasonID, episodeID, audio + ".mp3");
        }


		internal static String[] BlockLetters(String folderPath, String seasonID, String episodeID)
		{
			var episodePath = EpisodePath(folderPath, seasonID, episodeID);

			var blockFiles = Directory.GetFiles(episodePath, "*.json")
				.Where(sf => !sf.EndsWith("_.json"))
				.OrderBy(sf => sf)
				.ToList();

			for (var sf = 0; sf < blockFiles.Count; sf++)
			{
				blockFiles[sf] = BlockLetter(blockFiles[sf], episodePath);
			}

			return blockFiles.ToArray();
		}

		internal static String BlockLetter(String blockFile, String episodePath)
		{
			return blockFile
				.Replace(episodePath + @"\", "")
				.Replace(".json", "");
		}

		internal static String NoGenderPath(String folderPath, String seasonID, String episodeID)
		{
			var episodePath = EpisodePath(folderPath, seasonID, episodeID);
			return Path.Combine(episodePath, "nogender.txt");
		}
	}
}
