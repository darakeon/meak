using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Translator
{
	internal class FileToJson
	{
		public String Path { get; set; }

		public TextToJson TextToJson { get; set; }

		public Replace Start { get; set; }
		public String End { get; set; }
		
		private Action<String> warnStart { get; set; }
		private Action<List<String>> warnIfNotFound { get; set; }

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
				.ToList()
				.ForEach(createJsonMEAK);

			Directory.GetDirectories(path)
				.ToList()
				.ForEach(convert);
		}

		private void createJsonMEAK(String filePath)
		{
			warnStart(filePath);

			var content = File.ReadAllText(filePath);

			TextToJson.Start = Start.Transform(filePath);

			var jsonContent = TextToJson.Transform(filePath, content);

			warnIfNotFound(TextToJson.NotFound);

			var jsonFilePath = getNewFilePath(filePath);
			File.WriteAllText(jsonFilePath, jsonContent, Encoding.UTF8);
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