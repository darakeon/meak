using System;
using System.IO;
using System.Linq;
using DK.XML;
using Structure.Extensions;

namespace Structure.Data
{
    public static class MainInfoXML
    {
        const string title_node_name = "portuguese";
        const string summary_node_name = "summary";

        public static void Save(String title, String summary, String folderPath, String seasonID, String episodeID)
        {
            var path = Paths.SceneFilePath(folderPath, seasonID, episodeID, "_");

            var exists = new FileInfo(path)
                .CreateIfNotExists("<story></story>");

            var xml = new Node(path);

            putTitle(xml, title);
            putSummary(xml, summary);
            
            putAttributes(xml, seasonID, episodeID);

            var backupPath = Paths.BackupFilePath(folderPath, seasonID, episodeID, "_");

            if (exists)
                xml.BackUpAndSave(backupPath);
            else
                xml.Overwrite();
        }

        private static void putTitle(Node xml, String title)
        {
            var titleNode = getNode(xml, title_node_name);

            if (titleNode.Attributes.Keys.Contains("title"))
                titleNode["title"] = title;
            else
                titleNode.Add("title", title);
        }

        private static void putSummary(Node xml, String summary)
        {
            var summaryNode = getNode(xml, summary_node_name);

            summaryNode.Value = summary;
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



        internal static Info Get(String path, String seasonID, String episodeID)
        {
            var mainInfoXmlName = Paths.SceneFilePath(path, seasonID, episodeID, "_");

            if (!File.Exists(mainInfoXmlName))
                return null;

            var mainInfoXml = new Node(mainInfoXmlName);
			var titleNode = getNode(mainInfoXml, title_node_name);
			var summaryNode = getNode(mainInfoXml, summary_node_name);

            var info = new Info
            {
				Title = titleNode["title"],
				Publish = DateTime.Parse(mainInfoXml["publish"]),
                LastScene = mainInfoXml.Attributes["last"],
				Summary = summaryNode.Value,
            };
            
            return info;
        }



        private static Node getNode(Node xml, String nodeName)
        {
            var node = xml.Childs.SingleOrDefault(n => n.Name == nodeName);

            if (node == null)
            {
                xml.Add(new Node(nodeName, ""));
                node = xml.Childs.Single(n => n.Name == nodeName);
            }

            return node;
        }

        public class Info
        {
            public String Title { get; internal set; }
			public DateTime Publish { get; internal set; }
			public String LastScene { get; internal set; }
            public String Summary { get; internal set; }
        }

    }
}
