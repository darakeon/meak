using System;
using System.IO;
using System.Linq;

namespace Structure.Data
{
    public class Paths
    {
        public Paths(String xmlPath, String cssPath)
        {
            Xml = xmlPath;
            Css = cssPath;
        }

        public String Xml { get; private set; }
        public String Css { get; private set; }


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

        public static String SceneFilePath(String folderPath, String seasonID, String episodeID, String sceneID)
        {
            return Path.Combine(folderPath, "_" + seasonID, episodeID, sceneID + ".xml");
        }

		public static String FtpDirectoryPath(String folderPath, String seasonID)
		{
			return SeasonPath(folderPath, seasonID).Replace(@"\", "/");
		}

		public static String FtpDirectoryPath(String folderPath, String seasonID, String episodeID)
		{
			return EpisodePath(folderPath, seasonID, episodeID).Replace(@"\", "/");
		}

		public static String FtpFilePath(String folderPath, String seasonID, String episodeID, String sceneID)
		{
			return SceneFilePath(folderPath, seasonID, episodeID, sceneID).Replace(@"\", "/");
		}



        internal static String[] SceneLetters(String folderPath, String seasonID, String episodeID)
        {
            var episodePath = EpisodePath(folderPath, seasonID, episodeID);

            var sceneFiles = Directory.GetFiles(episodePath)
                .Where(sf => !sf.EndsWith("_.xml"))
                .ToList();

            for (var sf = 0; sf < sceneFiles.Count; sf++)
            {
                sceneFiles[sf] = SceneLetter(sceneFiles[sf], episodePath);
            }

            return sceneFiles.ToArray();
        }
        
        internal static String SceneLetter(String sceneFile, String episodePath)
        {
            return sceneFile
                .Replace(episodePath + @"\", "")
                .Replace(".xml", "");
        }

    }
}
