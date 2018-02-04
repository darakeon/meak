using System;

namespace Structure.Entities.Json
{
	public class SummaryPart
	{
		public String Season { get; set; }
		public String Episode { get; set; }

		public String Portuguese { get; set; }
		public String English { get; set; }
		internal String Title
		{
			get => Portuguese;
			set => Portuguese = value;
		}

		public String Summary { get; set; }
		
		public DateTime Publish { get; set; }
		public String Last { get; set; }
	}
}