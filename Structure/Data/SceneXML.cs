using System;
using System.IO;
using System.Xml;
using Ak.DataAccess.XML;
using Structure.Entities;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Data
{
    public class SceneXML
    {
        private readonly string episodeID;
        private readonly string folderPath;
        private readonly string seasonID;

        readonly String tellerEnum = ParagraphType.Teller.ToString().ToLower();
        readonly String talkEnum = ParagraphType.Talk.ToString().ToLower();

        public Scene Scene { get; set; }
        public FileInfo FileInfo { get; set; }

        private String backupPath { get; set; }

        public const String FirstScene = "a";



        public SceneXML(String folderPath, String season, String episode)
            : this(folderPath, season, episode, FirstScene) { }

        public SceneXML(String folderPath, String season, String episode, String scene)
            : this(folderPath, season, episode, scene, OpenEpisodeOption.GetCode) { }

        public SceneXML(String folderPath, String seasonID, String episodeID, String sceneID, OpenEpisodeOption get)
        {
            this.folderPath = folderPath;
            this.seasonID = seasonID;
            this.episodeID = episodeID;

            backupPath = Paths.BackupFilePath(folderPath, seasonID, episodeID, sceneID);

            var storyPath = Paths.SceneFilePath(folderPath, seasonID, episodeID, sceneID);
            FileInfo = new FileInfo(storyPath);

            var episode = new Episode(folderPath, seasonID, episodeID);
            populateScene(get, episode);
        }

        #region For Constructor
        private void populateScene(OpenEpisodeOption get, Episode episode)
        {
            Scene = new Scene
            {
                ID = FileInfo.NameWithoutExtension(),
                Episode = episode
            };

            if (get == OpenEpisodeOption.GetStory)
                readStory();
        }

        private void readStory()
        {
            var xml = new Node(FileInfo.FullName);

            verifyXmlAttributes(xml);

            if (!String.IsNullOrEmpty(xml.Value))
                throw new Exception("Story pieces out of tags: " + xml.Value);


            foreach (var node in xml)
            {
                if (!String.IsNullOrEmpty(node.Value))
                    throw new Exception("Story pieces without style: " + node.Value);

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
            switch (paragraph)
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
        #endregion



        public void AddNewStory(String title)
        {
            var sceneExists =
                !FileInfo.CreateIfNotExists("<story></story>");


            MainInfoXML.Save(title, "Story summary.", folderPath, seasonID, episodeID);

            fakeStory();


            var storyXML = makeStoryXML();

            if (sceneExists)
                storyXML.BackUpAndSave(backupPath);
            else
                storyXML.Overwrite();
        }



        public void WriteStory()
        {
            var xml = makeStoryXML();

            xml.BackUpAndSave(backupPath);
        }



        private void fakeStory()
        {
            for (var j = 0; j < 3; j++)
            {
                Scene.ParagraphTypeList.Add(ParagraphType.Teller);
                Scene.TellerList.Add(tellerDefault());

                for (var k = 0; k < 5; k++)
                {
                    Scene.ParagraphTypeList.Add(ParagraphType.Talk);
                    Scene.TalkList.Add(talkDefault());
                }
            }
        }

        private static Teller tellerDefault()
        {
            return new Teller
            {
                Pieces = {
                    new Piece<TellerStyle> { Style = TellerStyle.Default, Text = "_" }
                }
            };
        }

        private static Talk talkDefault()
        {
            return new Talk
            {
                Pieces = {
                    new Piece<TalkStyle> { Style = TalkStyle.Default, Text = "_" }
                },
                Character = "Akeon"
            };
        }



        private Node makeStoryXML()
        {
            var xml = new Node(FileInfo.FullName, false);

            verifyXmlAttributes(xml);

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



        private void verifyXmlAttributes(Node xml)
        {
            if (xml["scene"] != Scene.ID)
                throw new Exception(String.Format("Scene [{0}] and file name [{1}] doesn't match.", xml["scene"], Scene.ID));

            if (xml["episode"] != Scene.Episode.ID)
                throw new Exception(String.Format("Episode [{0}] and file path [{1}] doesn't match.", xml["episode"], Scene.Episode.ID));

            if (xml["season"] != Scene.Episode.Season.ID)
                throw new Exception(String.Format("Season [{0}] and file path [{1}] doesn't match.", xml["season"], Scene.Episode.Season.ID));
        }




    }
}
