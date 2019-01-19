using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator
{
	internal class TextToJson
	{
		public IList<Replacer> TextTransform { get; set; }
		public IList<Character> Characters { get; set; }
		public IList<Verification> Verifications { get; set; }

		public List<String> NotFound { get; set; }
		public String Start { get; set; }
		public String End { get; set; }

		public String Transform(String filePath, String originalText)
		{
			var newText = $"\n{originalText.Trim()}\n";

			foreach (var replace in TextTransform)
			{
				newText = replace.Transform(newText);
			}

			foreach (var replace in Characters)
			{
				newText = replace.Transform(newText);
			}

			newText = newText.Trim();

			NotFound = Verifications.SelectMany(
				v => v.NotAllowedMatches(newText)
			).Select(t => $"not found: {t}").ToList();

			return Start + newText + End;
		}
	}
}