using System;
using System.IO;
using System.Xml;
using DK.XML;
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

        public const String FIRST_SCENE = "a";



        public SceneXML(String folderPath, String season, String episode)
            : this(folderPath, season, episode, FIRST_SCENE) { }

        public SceneXML(String folderPath, String season, String episode, String scene)
            : this(folderPath, season, episode, scene, OpenEpisodeOption.GetCode) { }

        public SceneXML(String folderPath, String seasonID, String episodeID, String sceneID, OpenEpisodeOption get)
        {
            this.folderPath = folderPath;
            this.seasonID = seasonID;
            this.episodeID = episodeID;

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
                throw new XmlException($"Node {nodeName} ({Scene.ParagraphCount}º) not recognized.");

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
            FileInfo.CreateIfNotExists("<story></story>");

            MainInfoXML.Save(title, "Story summary.", folderPath, seasonID, episodeID);

            fakeStory();

            var storyXML = makeStoryXML();

            storyXML.Overwrite();
        }



        public void WriteStory()
        {
            var xml = makeStoryXML();
            xml.Overwrite();
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
                        throw new Exception($"Not recognized Paragraph [{paragraph}].");
                }

                if (child.HasChilds())
                    xml.Add(child);
            }
            return xml;
        }



        private void verifyXmlAttributes(Node xml)
        {
            if (xml["scene"] != Scene.ID)
                throw new Exception($"Scene [{xml["scene"]}] and file name [{Scene.ID}] doesn't match.");

            if (xml["episode"] != Scene.Episode.ID)
                throw new Exception($"Episode [{xml["episode"]}] and file path [{Scene.Episode.ID}] doesn't match.");

            if (xml["season"] != Scene.Episode.Season.ID)
                throw new Exception($"Season [{xml["season"]}] and file path [{Scene.Episode.Season.ID}] doesn't match.");
        }




    }
}
