using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Translator
{
	internal class FileToJson
	{
		public String Path { get; set; }

		public TextToJson TextToJson { get; set; }

		public Replacer Start { get; set; }
		public String End { get; set; }
		
		private Action<String> warnStart { get; set; }
		private Action<List<String>> warnIfNotFound { get; set; }

		private static readonly Regex sceneFile = new Regex("[a-g].txt");

		public static FileToJson Get(
			Action<String> warnStart,
			Action<List<String>> warnIfNotFound
		) {
			var textToJsonConfig = File.ReadAllText("config.json");
			var instance = JsonConvert.DeserializeObject<FileToJson>(textToJsonConfig);
			
			instance.warnStart = warnStart;
			instance.warnIfNotFound = warnIfNotFound;
			
			return instance;
		}

		public void Convert()
		{
			convert(Path);
		}

		private void convert(String path)
		{
			TextToJson.End = End;

			Directory.GetFiles(path, "*.txt")
				.Where(f => sceneFile.IsMatch(f))
				.ToList()
				.ForEach(createJsonStory);

			Directory.GetFiles(path, "_.txt")
				.ToList()
				.ForEach(createJsonSummary);

			Directory.GetDirectories(path)
				.ToList()
				.ForEach(convert);
		}

		private void createJsonStory(String filePath)
		{
			warnStart(filePath);

			var content = File.ReadAllText(filePath);

			TextToJson.Start = Start.Transform(filePath);

			var jsonContent = TextToJson.Transform(filePath, content);

			warnIfNotFound(TextToJson.NotFound);

			var jsonFilePath = getNewFilePath(filePath);
			File.WriteAllText(jsonFilePath, jsonContent, Encoding.UTF8);
		}

		private void createJsonSummary(String filePath)
		{
			warnStart(filePath);

			var jsonFilePath = getNewFilePath(filePath);
			var json = File.ReadAllText(jsonFilePath);

			var newText = File.ReadAllText(filePath)
				.Replace(Environment.NewLine, " ")
				.Replace("\"", "'")
				.Trim();

			var regex = new Regex("(\n\t(\"summary\": \")(.*)(\",))?(\n\\})");
			var groups = regex.Match(json).Groups.ToList();

			var item = groups[1].Value;
			var oldText = groups[3].Value;

			if (!String.IsNullOrEmpty(item)
			    && !newText.Contains(oldText))
			{
				newText = $"{oldText} {newText}";
			}

			newText = $"\n\t\"summary\": \"{newText}\",\n}}";
			var newJson = regex.Replace(json, newText);

			File.WriteAllText(jsonFilePath, newJson);
		}

		private string getNewFilePath(string filePath)
		{
			var fileInfo = new FileInfo(filePath);
			var jsonFilePath =
				filePath.Replace(fileInfo.Extension, ".json");
			return jsonFilePath;
		}
	}
}