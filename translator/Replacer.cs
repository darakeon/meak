using System;
using System.Text.RegularExpressions;

namespace Translator
{
	internal class Replacer
	{
		public Replacer(String oldValue, String newValue)
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		private String oldValue { get; }
		private String newValue { get; }

		public String Transform(String originalText)
		{
			var regex = new Regex(oldValue);
			return regex.Replace(originalText, newValue);
		}
	}
}