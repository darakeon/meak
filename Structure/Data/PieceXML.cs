using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ak.DataAccess.XML;
using Structure.Entities;

namespace Structure.Data
{
    public class PieceXML<S> where S : struct
    {
        public static Piece<S> GetPiece(Node node, Node child)
        {
            var style = GetStyle(node, child);

            return new Piece<S> { Text = child.Value, Style = style };
        }

        private static S GetStyle(Node node, Node child)
        {
            try
            {
                return (S)Enum.Parse(typeof(S), child.Name, true);
            }
            catch
            {
                throw new Exception(String.Format("Style {0} not found in {1} (text: '{2}').", child.Name, node.Name, child.Value));
            }
        }

        
        
        public static Node SetPiece(Piece<S> piece)
        {
            var name = SetStyle(piece.Style);
            var value = piece.Text;

            var node = new Node(name, value);

            return node;
        }
        
        public static String SetStyle(S style)
        {
            return style.ToString().ToLower();
        }
    }
}
