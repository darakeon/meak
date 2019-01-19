using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Translator
{
	internal class Character: Replacer
	{
		private Character(String oldValue, String newValue)
			: base(
				$"\"character\": \"{oldValue}\"",
				$"\"character\": \"{newValue}\""
			) {}

		private static readonly Regex characterLine =
			new Regex("([A-Z])=(.*)");

		public static Character Get(String line)
		{
			var match = characterLine.Match(line);

			if (!match.Success)
				return null;

			var groups = 
				match.Groups
					.Select(g => g.Value)
					.ToList();

			return new Character(groups[1], groups[2]);
		}
	}
}