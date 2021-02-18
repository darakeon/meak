using System;
using System.Text.RegularExpressions;

namespace GenderFix
{
	class Replacer
	{
		private readonly Regex regex;
		private readonly String replacer;

		public Replacer(String pattern, String replacer)
		{
			regex = new Regex(pattern);
			this.replacer = replacer;
		}

		public String Replace(String input)
		{
			return regex.Replace(input, replacer);
		}
	}
}