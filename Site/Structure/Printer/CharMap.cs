using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Printer
{
	public class CharMap : IEnumerable<KeyValuePair<Char, Decimal>>
	{
		public CharMap(ParagraphType type, String style)
		{
			var file = $"{type}-{style}.json".ToLower();

			var path = Path.Combine("Printer", file);

			if (Directory.Exists("bin"))
				path = Path.Combine("bin", path);

			structPath = Path.Combine("..", "Structure", "Printer", file);

			var json = File.ReadAllText(path);

			characters = JsonConvert.DeserializeObject
				<Dictionary<Char, Decimal>>(json);

			characters = characters.OrderBy(p => p.Key)
				.ToDictionary(p => p.Key, p => p.Value);

			structPath.Write(characters);
		}

		private readonly String structPath;
		private readonly Dictionary<Char, Decimal> characters;

		public Decimal this[Char character] =>
			characters[removeDiacritics(character)];

		public Boolean Contains(Char character) =>
			characters.ContainsKey(removeDiacritics(character));

		public void Add(Char character)
		{
			characters.Add(removeDiacritics(character), 0);

			structPath.Write(
				characters.OrderBy(p => p.Key)
					.ToDictionary(p => p.Key, p => p.Value)
			);
		}

		private Char removeDiacritics(Char text)
		{
			return text.ToString()
				.Normalize(NormalizationForm.FormD)
				.FirstOrDefault(notDiacritic);
		}

		private Boolean notDiacritic(Char character)
		{
			return CharUnicodeInfo.GetUnicodeCategory(character) !=
			       UnicodeCategory.NonSpacingMark;
		}

		public IEnumerator<KeyValuePair<Char, Decimal>> GetEnumerator()
		{
			return characters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
