using System;
using System.Collections.Generic;
using Structure.Enums;

namespace Structure.Entities.System
{
	public class Talk : Paragraph<TalkStyle>
	{
		public IList<Int32> DebugCharacter = new List<Int32>();

		public String Character { get; set; }
	}
}
