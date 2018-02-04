using System;
using Structure.Enums;

namespace Structure.Entities.System
{
	public class Talk : Paragraph<TalkStyle>
	{
		public String Character { get; set; }
	}
}
