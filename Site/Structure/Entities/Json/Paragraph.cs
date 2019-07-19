using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Enums;

namespace Structure.Entities.Json
{
	public class Paragraph
	{
		public ParagraphType Type { get; set; }
		public String Character { get; set; }
		public IList<Piece> Pieces { get; set; }
		public Int32 Breaks { get; set; }

		internal Boolean HasText
		{
			get
			{
				return Type == ParagraphType.Page
					|| Pieces.Any(
						pc => !String.IsNullOrEmpty(pc.Text)
					);
			}
		}
	}
}
