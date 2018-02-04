using System;

namespace Structure.Entities.Json
{
	public class SummaryPart : Part
	{
		public String Portuguese { get; set; }
		public String English { get; set; }
		public String Title
		{
			get => Portuguese;
			set => Portuguese = value;
		}

		public String Summary { get; set; }
		public DateTime Publish { get; set; }
		public String Last { get; set; }
	}
}