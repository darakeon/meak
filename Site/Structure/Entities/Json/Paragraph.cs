using System;
using System.Collections.Generic;
using Structure.Enums;

namespace Structure.Entities.Json
{
	public class Paragraph
	{
		public ParagraphType Type { get; set; }
		public String Character { get; set; }
		public IList<Piece> Pieces { get; set; }
	}
}