using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structure.Entities
{
    public class Paragraph<T> where T : struct
    {
        public Paragraph()
        {
            Pieces = new List<Piece<T>>();
        }


        public IList<Piece<T>> Pieces { get; set; }


        public override String ToString()
        {
            var result = new StringBuilder();

            Pieces
                .ToList()
                .ForEach(p =>
                    result.Append(p.Text)
                );

            return result.ToString();
        }
    }
}
