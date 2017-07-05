using System;
using System.IO;
using System.Web;
using Structure.Entities;
using Structure.Enums;
using Structure.Helpers;

namespace Structure.Data
{
    public class EpisodeXML
    {
        private Episode episode { get; set; }
        public String PathXML { get; private set; }


        public EpisodeXML(HttpServerUtilityBase server)
        {
            setXMLPath(server);
        }



        public Episode GetEpisode(String seasonID, String episodeID)
        {
            episode = new Episode(PathXML, seasonID, episodeID);
            
            var sceneLetters = Paths.SceneLetters(PathXML, seasonID, episodeID);

            foreach (var sceneLetter in sceneLetters)
            {
                var xml = getScene(seasonID, episodeID, sceneLetter);

                episode.SceneList.Add(xml.Scene);
            }

            return episode;
        }




        private SceneXML getScene(String seasonID, String episodeID, String sceneID)
        {
            try
            {
                return new SceneXML(PathXML, seasonID, episodeID, sceneID, OpenEpisodeOption.GetStory);
            }
            catch (FileNotFoundException)
            {
                throw new StoriesException("Temporada e/ou Capítulo não encontrado(s).");
            }
            catch (Exception e)
            {
                throw new StoriesException(e.Message);
            }
        }

        
        private void setXMLPath(HttpServerUtilityBase server)
        {
            var folder = Config.StoriesPath;

            if (folder == null)
                throw new Exception("XML Path not configured.");

            PathXML =
                folder.Substring(1, 1) == ":"
                    ? folder
                    : Path.Combine(server.MapPath("~"), folder);

            if (!Directory.Exists(PathXML))
                throw new Exception(String.Format("Path '{0}' doesn't exists.", PathXML));
        }

    }
}
