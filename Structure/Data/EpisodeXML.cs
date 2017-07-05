using System;
using System.Linq;
using System.Xml;
using Ak.DataAccess.XML;
using Structure.Entities;
using Structure.Enums;
using System.IO;

namespace Structure.Data
{
    public class EpisodeXML
    {
        readonly String teller = eParagraph.Teller.ToString().ToLower();
        readonly String talk = eParagraph.Talk.ToString().ToLower();

        public Episode Episode { get; set; }
        public FileInfo FileInfo { get; set; }

        public EpisodeXML(String filePath, eOpenEpisodeOption get = eOpenEpisodeOption.getCode)
        {
            FileInfo = new FileInfo(filePath);

            PopulateEpisode(get);
        }

        public EpisodeXML(String folderPath, String season, String episode, eOpenEpisodeOption get = eOpenEpisodeOption.getCode)
            : this(Path.Combine(folderPath, "_" + season, episode + ".xml"), get) { }



        private void PopulateEpisode(eOpenEpisodeOption get)
        {
            Episode = new Episode {
                            ID = FileInfo.NameWithoutExtension(),
                            Season = {ID = FileInfo.Directory.Name.Substring(1)}
                        };

            if (get == eOpenEpisodeOption.getTitle)
                ReadTitle();
            else if (get == eOpenEpisodeOption.getStory)
                ReadStory();
        }

        private void ReadTitle()
        {
            var xml = new Node(FileInfo.FullName, false);
            Episode.Title = xml["title"];
        }

        private void ReadStory()
        {
            var xml = new Node(FileInfo.FullName);

            if (!String.IsNullOrEmpty(xml.Value))
            {
                throw new Exception("Story pieces out of tags: " + xml.Value);
            }


            Episode.Title = xml["title"];

            foreach(var node in xml)
            {
                if (!String.IsNullOrEmpty(node.Value))
                {
                    throw new Exception("Story pieces without style: " + node.Value);
                }


                var paragraph = DefineType(node.Name);

                SetText(paragraph, node);
            }
        }

        private eParagraph DefineType(String nodeName)
        {
            eParagraph paragraph;

            if (nodeName == teller)
                paragraph = eParagraph.Teller;
            else if (nodeName == talk)
                paragraph = eParagraph.Talk;
            else
                throw new XmlException(String.Format("Node {0} ({1}º) not recognized.", nodeName, Episode.ParagraphCount));

            Episode.ParagraphTypeList.Add(paragraph);

            return paragraph;
        }

        private void SetText(eParagraph paragraph, Node node)
        {
            switch(paragraph)
            {
                case eParagraph.Talk:
                    var talk = ParagraphXML.GetTalk(node);
                    Episode.TalkList.Add(talk);
                    break;
                case eParagraph.Teller:
                    var teller = ParagraphXML.GetTeller(node);
                    Episode.TellerList.Add(teller);
                    break;
            }
        }





        public void WriteStory()
        {
            var xml = MakeXml();

            xml.BackUpAndSave();
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
                Episode.ParagraphTypeList.Add(eParagraph.Teller);
                Episode.TellerList.Add(TellerDefault());

                for (var k = 0; k < 20; k++)
                {
                    Episode.ParagraphTypeList.Add(eParagraph.Talk);
                    Episode.TalkList.Add(TalkDefault());
                }
            }

            Episode.Title = title;

            var xml = MakeXml();

            if (episodeExists)
                xml.BackUpAndSave();
            else
                xml.Overwrite();
        }

        private Node MakeXml()
        {
            var xml = new Node(FileInfo.FullName, false);

            xml["title"] = Episode.Title;


            var talkCounter = 0;
            var tellerCounter = 0;


            foreach (var paragraph in Episode.ParagraphTypeList)
            {
                Node child;

                switch (paragraph)
                {
                    case eParagraph.Talk:
                        child = ParagraphXML.SetTalk(Episode.TalkList[talkCounter]);
                        talkCounter++;
                        break;
                    case eParagraph.Teller:
                        child = ParagraphXML.SetTeller(Episode.TellerList[tellerCounter]);
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



        private static Teller TellerDefault()
        {
            return new Teller { Pieces = {
                    new Piece<eTellerStyle> { Style = eTellerStyle.Default, Text = "_" }
                } };
        }

        private static Talk TalkDefault()
        {
            return new Talk { Pieces = {
                    new Piece<eTalkStyle> { Style = eTalkStyle.Default, Text = "_" }
                }, Character = "_" };
        }
    }
}
