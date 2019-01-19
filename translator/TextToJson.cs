using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator
{
	internal class TextToJson
	{
		public IList<Replacer> TextTransform { get; set; }
		public IList<Verification> Verifications { get; set; }

		public List<String> NotFound { get; set; }
		public String Start { get; set; }
		public String End { get; set; }

		public String Transform(String filePath, String originalText)
		{
			var lines = 
				originalText
					.Trim()
					.Replace("\r", "")
					.Split("\n");

			var characters = new List<Character>();
			var storyLines = new List<String> { "" };

			foreach (var line in lines)
			{
				var character = Character.Get(line);

				if (character != null)
					characters.Add(character);
				else
					storyLines.Add(line);
			}

			storyLines.Add("");

			var newText = String.Join("\n", storyLines);

			foreach (var replace in TextTransform)
			{
				newText = replace.Transform(newText);
			}

			foreach (var replace in characters)
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