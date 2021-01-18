using System;
using System.Collections.Generic;

namespace Structure.Entities.Json
{
	public class BlockPart
	{
		public String Season { get; set; }
		public String Episode { get; set; }
		public String Block { get; set; }
		public IList<Paragraph> Paragraphs { get; set; }
	}
}