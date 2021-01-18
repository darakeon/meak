using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Structure.Enums;

namespace Presentation.Helpers
{
	public class Dic
	{
		public static Dictionary<TalkStyle, String> Talk =
			new Dictionary<TalkStyle, String>
			{
				{ TalkStyle.Arrive, "Chegada" },
				{ TalkStyle.Default, "Normal" },
				{ TalkStyle.Frizz, "Frisado"},
				{ TalkStyle.Ghost, "Fantasma"},
				{ TalkStyle.Hushed, "Sussurrado / Voz Fraca"},
				{ TalkStyle.Irony, "Ironia / Sarcasmo"},
				{ TalkStyle.Out, "Longe"},
				{ TalkStyle.Read, "Lido"},
				{ TalkStyle.Screamed, "Gritado"},
				{ TalkStyle.Teller, "Ação"},
				{ TalkStyle.Thought, "Pensado"},
				{ TalkStyle.Translate, "Tradução"},
			};
	}
}