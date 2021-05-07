using System;
using System.Collections.Generic;
using System.IO;
using Structure.Helpers;

namespace Presentation.Models.General
{
	public class CssModel
	{
		public CssModel(String path)
		{
			Files = new List<CssFile>();

			foreach (var file in Directory.GetFiles(path))
			{
				Files.Add(new CssFile(file));
			}

			var isAuthor = Config.IsAuthor;

			if (!isAuthor)
				return;

			var directory = Path.Combine(path, "Author");

			foreach (var file in Directory.GetFiles(directory))
			{
				Files.Add(new CssFile(file, "Author"));
			}
		}

		public IList<CssFile> Files { get; set; }


		public class CssFile
		{
			public String Name;
			public String Media;

			public CssFile(String file)
			{
				Name = new FileInfo(file).Name;

				if (Name.Contains("_"))
				{
					var beforeUnderline = Name.LastIndexOf("_");
					Media = Name.Remove(beforeUnderline).ToLower();
				}
			}

			public CssFile(String file, String path) : this(file)
			{
				Name = Path.Combine(path, Name);
			}
		}
	}
}
