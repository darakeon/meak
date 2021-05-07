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
using Structure.Helpers;

namespace Structure.Printer
{
	public class CharMap : IEnumerable<KeyValuePair<Char, Decimal>>
	{
		public CharMap(ParagraphType type, String style)
		{
			map($"{type}-{style}".ToLower());
		}

		private void map(String fileName)
		{
			var file = $"{fileName}.json";

			var path = Path.Combine("Printer", file);

			if (Directory.Exists("bin"))
				path = Path.Combine(AppContext.BaseDirectory, path);

			var json = File.ReadAllText(path);

			var values = JsonConvert.DeserializeObject
				<Dictionary<String, String>>(json);

			if (values.ContainsKey("copy"))
			{
				map(values["copy"]);
				return;
			}

			characters = JsonConvert.DeserializeObject
				<Dictionary<Char, Decimal>>(json);

			characters = characters.OrderBy(p => p.Key)
				.ToDictionary(p => p.Key, p => p.Value);

			if (Config.IsAuthor)
			{
				structPath = Path.Combine("Printer", file);
				structPath.Write(characters);
			}
		}

		private String structPath;
		private Dictionary<Char, Decimal> characters;

		public Decimal this[Char character] =>
			characters[removeDiacritics(character)];

		public Boolean Contains(Char character) =>
			characters.ContainsKey(removeDiacritics(character));

		public void Add(Char character)
		{
			characters.Add(
				removeDiacritics(character),
				characters.Average(c => c.Value)
			);

			if (Config.IsAuthor)
			{
				structPath.Write(
					characters.OrderBy(p => p.Key)
						.ToDictionary(p => p.Key, p => p.Value)
				);
			}
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
