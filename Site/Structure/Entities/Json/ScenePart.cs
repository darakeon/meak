using System;
using System.Collections.Generic;

namespace Structure.Entities.Json
{
	public class ScenePart : Part
	{
		public String Scene { get; set; }
		public IList<Paragraph> Paragraphs { get; set; }
	}
}