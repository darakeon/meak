using System;
using Ak.DataAccess.XML;
using Structure.Entities;

namespace Structure.Data
{
    public class PieceXML<TS> where TS : struct
    {
        public static Piece<TS> GetPiece(Node node, Node child)
        {
            var style = getStyle(node, child);

            return new Piece<TS> { Text = child.Value, Style = style };
        }

        private static TS getStyle(Node node, Node child)
        {
            try
            {
                return (TS)Enum.Parse(typeof(TS), child.Name, true);
            }
            catch
            {
                throw new Exception(String.Format("Style {0} not found in {1} (text: '{2}').", child.Name, node.Name, child.Value));
            }
        }

        
        
        public static Node SetPiece(Piece<TS> piece)
        {
            var name = SetStyle(piece.Style);
            var value = piece.Text;

            var node = new Node(name, value);

            return node;
        }
        
        public static String SetStyle(TS style)
        {
            return style.ToString().ToLower();
        }
    }
}
