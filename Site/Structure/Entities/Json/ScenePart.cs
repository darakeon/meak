using System;
using System.Collections.Generic;

namespace Structure.Entities.Json
{
	public class ScenePart
	{
		public String Season { get; set; }
		public String Episode { get; set; }
		public String Scene { get; set; }
		public IList<Paragraph> Paragraphs { get; set; }
	}
}