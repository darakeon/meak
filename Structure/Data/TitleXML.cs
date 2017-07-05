using System;
using System.IO;
using Ak.DataAccess.XML;
using Structure.Extensions;

namespace Structure.Data
{
    public class TitleXML
    {
        public static void Save(String title, String folderPath, String seasonID, String episodeID)
        {
            var path = Paths.SceneFilePath(folderPath, seasonID, episodeID, "_");

            var exists = new FileInfo(path)
                .CreateIfNotExists("<story></story>");

            
            var xml = new Node(path);


            putTitle(xml, title);
            
            putAttributes(xml, seasonID, episodeID);


            var backupPath = Paths.BackupFilePath(folderPath, seasonID, episodeID, "_");


            if (exists)
                xml.BackUpAndSave(backupPath);
            else
                xml.Overwrite();
        }

        private static void putTitle(Node xml, string title)
        {
            if (xml.Childs.Count == 0)
                xml.Add(new Node("portuguese", ""));

            if (xml[0].Attributes.Keys.Contains("title"))
                xml[0]["title"] = title;
            else
                xml[0].Add("title", title);
        }

        private static void putAttributes(Node xml, String seasonID, String episodeID)
        {
            if (xml.Attributes.Keys.Contains("season"))
                xml["season"] = seasonID;
            else
                xml.Add("season", seasonID);

            if (xml.Attributes.Keys.Contains("episode"))
                xml["episode"] = episodeID;
            else
                xml.Add("episode", episodeID);
        }



        internal static string Get(String path, String seasonID, String episodeID)
        {
            var titleXMLName = Paths.SceneFilePath(path, seasonID, episodeID, "_");

            if (!File.Exists(titleXMLName))
                return null;

            var titleXML = new Node(titleXMLName);
            return titleXML[0]["title"];
        }

    }
}
