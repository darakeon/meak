using System;
using System.IO;
using System.Linq;
using System.Xml;
using Ak.DataAccess.XML;
using Structure.Entities;
using Structure.Enums;
using FileInfoExtension = Structure.Extensions.FileInfoExtension;

namespace Structure.Data
{
    public class SceneXML
    {
        readonly String tellerEnum = ParagraphType.Teller.ToString().ToLower();
        readonly String talkEnum = ParagraphType.Talk.ToString().ToLower();

        public Scene Scene { get; set; }
        public FileInfo FileInfo { get; set; }

        private String backupFullName { get; set; }

        public const String FirstScene = "a";


        public SceneXML(String folderPath, String season, String episode)
            : this(folderPath, season, episode, FirstScene) { }

        public SceneXML(String folderPath, String season, String episode, String scene)
            : this(folderPath, season, episode, scene, OpenEpisodeOption.GetCode) { }

        public SceneXML(String folderPath, String seasonID, String episodeID, String sceneID, OpenEpisodeOption get)
        {
            var filePath = Path.Combine(folderPath, "_" + seasonID, episodeID, sceneID + ".xml");

            FileInfo = new FileInfo(filePath);


            var episode = new Episode(folderPath, seasonID, episodeID);

            populateScene(get, episode);


            setBackupName(folderPath, seasonID, episodeID, sceneID);

            FileInfo = new FileInfo(filePath);
        }



        private void setBackupName(String folderPath, String season, String episode, String scene)
        {
            var datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var backupFile = String.Format("{0}_{1}{2}-{3}.xml", datetime, season, episode, scene);
            backupFullName = Path.Combine(folderPath, "Backup", backupFile);
        }



        private void populateScene(OpenEpisodeOption get, Episode episode)
        {
            Scene = new Scene {
                            ID = FileInfoExtension.NameWithoutExtension(FileInfo),
                            Episode = episode
                        };

            if (get == OpenEpisodeOption.GetStory)
                readStory();
        }

        private void readStory()
        {
            var xml = new Node(FileInfo.FullName);

            if (!String.IsNullOrEmpty(xml.Value))
            {
                throw new Exception("Story pieces out of tags: " + xml.Value);
            }


            foreach(var node in xml)
            {
                if (!String.IsNullOrEmpty(node.Value))
                {
                    throw new Exception("Story pieces without style: " + node.Value);
                }


                var paragraph = defineType(node.Name);

                setText(paragraph, node);
            }
        }

        private ParagraphType defineType(String nodeName)
        {
            ParagraphType paragraph;

            if (nodeName == tellerEnum)
                paragraph = ParagraphType.Teller;
            else if (nodeName == talkEnum)
                paragraph = ParagraphType.Talk;
            else
                throw new XmlException(String.Format("Node {0} ({1}º) not recognized.", nodeName, Scene.ParagraphCount));

            Scene.ParagraphTypeList.Add(paragraph);

            return paragraph;
        }

        private void setText(ParagraphType paragraph, Node node)
        {
            switch(paragraph)
            {
                case ParagraphType.Talk:
                    var talk = ParagraphXML.GetTalk(node);
                    Scene.TalkList.Add(talk);
                    break;
                case ParagraphType.Teller:
                    var teller = ParagraphXML.GetTeller(node);
                    Scene.TellerList.Add(teller);
                    break;
            }
        }





        public void WriteStory()
        {
            var xml = makeXML();

            xml.BackUpAndSave(backupFullName);
        }

        public void AddNewStory(String title)
        {
            var episodeExists = FileInfo.Exists;

            if (!episodeExists)
            {
                var file = File.Create(FileInfo.FullName);

                var parentNode = "<story></story>".Select(c => (byte) c).ToArray();

                file.Write(parentNode, 0, parentNode.Length);

                file.Flush(); file.Close();
            }

            for (var j = 0; j < 10; j++)
            {
                Scene.ParagraphTypeList.Add(ParagraphType.Teller);
                Scene.TellerList.Add(tellerDefault());

                for (var k = 0; k < 20; k++)
                {
                    Scene.ParagraphTypeList.Add(ParagraphType.Talk);
                    Scene.TalkList.Add(talkDefault());
                }
            }

            Scene.Episode.Title = title;

            var xml = makeXML();

            if (episodeExists)
                xml.BackUpAndSave();
            else
                xml.Overwrite();
        }

        private Node makeXML()
        {
            var xml = new Node(FileInfo.FullName, false);

            xml["title"] = Scene.Episode.Title;


            var talkCounter = 0;
            var tellerCounter = 0;


            foreach (var paragraph in Scene.ParagraphTypeList)
            {
                Node child;

                switch (paragraph)
                {
                    case ParagraphType.Talk:
                        child = ParagraphXML.SetTalk(Scene.TalkList[talkCounter]);
                        talkCounter++;
                        break;
                    case ParagraphType.Teller:
                        child = ParagraphXML.SetTeller(Scene.TellerList[tellerCounter]);
                        tellerCounter++;
                        break;
                    default:
                        throw new Exception(String.Format("Not recognized Paragraph [{0}].", paragraph));
                }

                if (child.HasChilds())
                    xml.Add(child);
            }
            return xml;
        }



        private static Teller tellerDefault()
        {
            return new Teller { Pieces = {
                    new Piece<TellerStyle> { Style = TellerStyle.Default, Text = "_" }
                } };
        }

        private static Talk talkDefault()
        {
            return new Talk { Pieces = {
                    new Piece<TalkStyle> { Style = TalkStyle.Default, Text = "_" }
                }, Character = "_" };
        }
    }
}
