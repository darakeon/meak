using System;

namespace Structure.Entities.System
{
    public class Piece<T> where T : struct
    {
        public Piece() { }

        public Piece(T defaultStyle)
        {
            Style = defaultStyle;
        }


        public T Style { get; set; }
        public String Text { get; set; }

        public override String ToString()
        {
            return Text;
        }
    }
}
