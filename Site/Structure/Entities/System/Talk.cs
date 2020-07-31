using System;
using System.Collections.Generic;
using Structure.Enums;

namespace Structure.Entities.System
{
	public class Talk : Paragraph<TalkStyle>
	{
		public IList<Decimal> DebugCharacter = new List<Decimal>();

		public String Character { get; set; }
	}
}
