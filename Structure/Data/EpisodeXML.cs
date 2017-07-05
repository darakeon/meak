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
    public class EpisodeXML
    {
        readonly String tellerEnum = ParagraphType.Teller.ToString().ToLower();
        readonly String talkEnum = ParagraphType.Talk.ToString().ToLower();

        public Episode Episode { get; set; }
        public FileInfo FileInfo { get; set; }

        private String backupFullName { get; set; }

        
        public EpisodeXML(String folderPath, String season, String episode, OpenEpisodeOption get = OpenEpisodeOption.GetCode)
        {
            var filePath = Path.Combine(folderPath, "_" + season, episode + ".xml");

            FileInfo = new FileInfo(filePath);

            populateEpisode(get);

            var backupFile = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + season + episode + ".xml";
            backupFullName = Path.Combine(folderPath, "Backup", backupFile);

            FileInfo = new FileInfo(filePath);
        }



        private void populateEpisode(OpenEpisodeOption get)
        {
            Episode = new Episode {
                            ID = FileInfoExtension.NameWithoutExtension(FileInfo),
                            Season = {ID = FileInfo.Directory.Name.Substring(1)}
                        };

            switch (get)
            {
                case OpenEpisodeOption.GetTitle:
                    readTitle();
                    break;
                case OpenEpisodeOption.GetStory:
                    readStory();
                    break;
            }
        }

        private void readTitle()
        {
            var xml = new Node(FileInfo.FullName, false);
            Episode.Title = xml["title"];
        }

        private void readStory()
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
                throw new XmlException(String.Format("Node {0} ({1}º) not recognized.", nodeName, Episode.ParagraphCount));

            Episode.ParagraphTypeList.Add(paragraph);

            return paragraph;
        }

        private void setText(ParagraphType paragraph, Node node)
        {
            switch(paragraph)
            {
                case ParagraphType.Talk:
                    var talk = ParagraphXML.GetTalk(node);
                    Episode.TalkList.Add(talk);
                    break;
                case ParagraphType.Teller:
                    var teller = ParagraphXML.GetTeller(node);
                    Episode.TellerList.Add(teller);
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
                Episode.ParagraphTypeList.Add(ParagraphType.Teller);
                Episode.TellerList.Add(tellerDefault());

                for (var k = 0; k < 20; k++)
                {
                    Episode.ParagraphTypeList.Add(ParagraphType.Talk);
                    Episode.TalkList.Add(talkDefault());
                }
            }

            Episode.Title = title;

            var xml = makeXML();

            if (episodeExists)
                xml.BackUpAndSave();
            else
                xml.Overwrite();
        }

        private Node makeXML()
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
                    case ParagraphType.Talk:
                        child = ParagraphXML.SetTalk(Episode.TalkList[talkCounter]);
                        talkCounter++;
                        break;
                    case ParagraphType.Teller:
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
