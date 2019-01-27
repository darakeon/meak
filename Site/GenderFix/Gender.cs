using System;
using System.Collections.Generic;

namespace GenderFix
{
	public class Gender
	{
		public String Remove(String text)
		{
			list.ForEach(r =>
			{
				text = r.Replace(text);
			});

			return text;
		}

		public void AddRule(String pattern, String replacer)
		{
			list.Add(new Replacer(pattern, replacer));
		}

		readonly List<Replacer> list = new List<Replacer> {
			new Replacer("\\b[oO] +(\\p{Lu})", "$1"),
			new Replacer("\\b([Dd])[ao] +(\\p{Lu})", "$1e $2"),

			new Replacer("\\bn[ao] +(\\p{Lu})", "em $1"),
			new Replacer("\\bN[ao] +(\\p{Lu})", "Em $1"),

			new Replacer("\\b(à|ao) +(\\p{Lu})", "a $2"),
			new Replacer("\\b(À|Ao) +(\\p{Lu})", "A $2"),

			new Replacer("\\b([Pp])ro +(\\p{Lu})", "$1ra $2"),

			new Replacer("([Tt])odos", "$1odo mundo"),

			new Replacer("\\b[Ee]l[ea]\\b", "=======PRONOME======="),
			new Replacer("\\b([Dd])el[ea]\\b", "$1e =======PRONOME======="),

			new Replacer("\\bm(á|au)\\b", "ruim"),
		};
	}
}