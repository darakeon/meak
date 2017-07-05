using System;
using System.Linq;
using System.Web;
using Ak.DataAccess.XML;
using Structure.Entities;
using Structure.Enums;
using Structure.Helpers;

namespace Structure.Data
{
    public class ParagraphXML
    {
        public static Talk GetTalk(Node node)
        {
            Talk talk = GetParagraph<Talk ,eTalkStyle>(node);

            var character = node["character"];

            var isAuthor = UrlUserType.IsAuthor();

            
            if (String.IsNullOrEmpty(character) && !isAuthor)
                throw new Exception(String.Format("No Character: {0}", talk));
            

            talk.Character = character;

            return talk;
        }

        
        public static Teller GetTeller(Node node)
        {
            return GetParagraph<Teller, eTellerStyle>(node);
        }


        private static P GetParagraph<P, S>(Node node)
            where P : Paragraph<S>, new()
            where S : struct
        {
            P paragraph = new P();

            foreach (var child in node)
            {
                var piece = PieceXML<S>.GetPiece(node, child);

                paragraph.Pieces.Add(piece);
            }

            return paragraph;
        }






        public static Node SetTalk(Talk talk)
        {
            var node = SetParagraph(talk);
            
            node.Add("character", talk.Character);

            return node;
        }

        public static Node SetTeller(Teller teller)
        {
            return SetParagraph(teller);
        }

        private static Node SetParagraph<T>(Paragraph<T> paragraph) where T : struct
        {
            var nodeName = typeof(T).Name
                    .Replace("Style", "")
                    .Substring(1)
                    .ToLower();

            var node = new Node(nodeName, "");

            var pieces = paragraph.Pieces
                .Where(p => 
                    !String.IsNullOrEmpty(p.Text));

            foreach (var piece in pieces)
            {
                var child = PieceXML<T>.SetPiece(piece);

                node.Add(child);
            }

            return node;
        }
    }
}
