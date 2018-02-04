using System;
using System.Collections.Generic;

namespace Structure.Entities.System
{
	public class Message
	{
		public Message()
		{
			Lines = new List<String>();
		}

		public String Title { get; set; }
		public IList<String> Lines { get; set; }
		public DateTime Date { get; set; }
	}
}
