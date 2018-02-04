using System;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Entities.Json
{
	public class Piece
	{
		public TalkStyle TalkStyle
		{
			get => Type.GetEnum<TalkStyle>();
			set => Type = value.ToString();
		}

		public TellerStyle TellerStyle
		{
			get => Type.GetEnum<TellerStyle>();
			set => Type = value.ToString();
		}

		public String Type { get; set; }
		public String Text { get; set; }
	}
}