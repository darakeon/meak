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

		private static readonly String config = File.ReadAllText("config.json");

		private static readonly Regex storyDir = new Regex("(_[A-Z]|\\d{2})$");

		private static readonly Regex jsonFile = new Regex(".\\.json");
		private static readonly Regex sceneFile = new Regex("[a-g]\\..{2,4}");
		private static readonly Regex summaryFile = new Regex("_\\..{2,4}");

		public static FileToJson Get(
			Action<String> warnStart,
			Action<List<String>> warnIfNotFound
		) {
			var instance = deserialize(config);

			instance.warnStart = warnStart;
			instance.warnIfNotFound = warnIfNotFound;

			instance.TextToJson.End = instance.End;

			return instance;
		}

		private static FileToJson deserialize(String json)
		{
			return JsonConvert.DeserializeObject<FileToJson>(json);
		}

		public void Convert()
		{
			convertDirectory(Path);
		}

		private void convertDirectory(String path)
		{
			Directory.GetFiles(path)
				.Where(f => !jsonFile.IsMatch(f))
				.ToList()
				.ForEach(Convert);

			Directory.GetDirectories(path)
				.Where(d => storyDir.IsMatch(d))
				.ToList()
				.ForEach(convertDirectory);
		}

		public void Convert(String filePath)
		{
			if (sceneFile.IsMatch(filePath))
				createJsonStory(filePath);

			if (summaryFile.IsMatch(filePath))
				createJsonSummary(filePath);
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

		private string getNewFilePath(String filePath)
		{
			var fileInfo = new FileInfo(filePath);
			var jsonFilePath =
				filePath.Replace(fileInfo.Extension, ".json");
			return jsonFilePath;
		}
	}
}