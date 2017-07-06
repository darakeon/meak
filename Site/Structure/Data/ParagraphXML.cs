using System;
using System.Linq;
using DK.XML;
using Structure.Entities;
using Structure.Enums;
using Structure.Helpers;

namespace Structure.Data
{
    public class ParagraphXML
    {
        public static Talk GetTalk(Node node)
        {
            var talk = getParagraph<Talk ,TalkStyle>(node);

            var character = node["character"];

            var isAuthor = UrlUserType.IsAuthor();

            
            if (String.IsNullOrEmpty(character) && !isAuthor)
                throw new Exception($"No Character: {talk}");
            

            talk.Character = character;

            return talk;
        }

        
        public static Teller GetTeller(Node node)
        {
            return getParagraph<Teller, TellerStyle>(node);
        }


        private static TP getParagraph<TP, TS>(Node node)
            where TP : Paragraph<TS>, new()
            where TS : struct
        {
            var paragraph = new TP();

            foreach (var child in node)
            {
                var piece = PieceXML<TS>.GetPiece(node, child);

                paragraph.Pieces.Add(piece);
            }

            return paragraph;
        }






        public static Node SetTalk(Talk talk)
        {
            var node = setParagraph(talk);
            
            node.Add("character", talk.Character);

            return node;
        }

        public static Node SetTeller(Teller teller)
        {
            return setParagraph(teller);
        }

        private static Node setParagraph<T>(Paragraph<T> paragraph) where T : struct
        {
            var nodeName = typeof(T).Name
                    .Replace("Style", "")
                    .ToLower();

            var node = new Node(nodeName, "");

            paragraph.Pieces
                .Where(p => !String.IsNullOrEmpty(p.Text))
                .ToList()
                .ForEach(
                    p => node.Add(
                        PieceXML<T>.SetPiece(p)
                    )
                );

            return node;
        }
    }
}
