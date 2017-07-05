using System;
using System.Configuration;
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
            episode = new Episode();
            setXMLPath(server);
        }



        public Episode GetEpisode(String seasonID, String episodeID)
        {
            var xml = getScene(seasonID, episodeID, "");

            episode.SceneList.Add(xml.Scene);

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
                throw new StoriesException("Temporada, Capítulo e/ou Cena não encontrado(s).");
            }
            catch (Exception e)
            {
                throw new StoriesException(e.Message);
            }
        }

        
        private void setXMLPath(HttpServerUtilityBase server)
        {
            var folder = ConfigurationManager.AppSettings["Path"];

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
