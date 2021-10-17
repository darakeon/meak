using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Helpers;

namespace Presentation.Models.General
{
	public class CssModel
	{
		public CssModel(String path)
		{
			Files = getFiles(path);

			if (!Config.IsAuthor)
				return;

			var authorDirectory = Path.Combine(path, "Author");
			var authorFiles = getFiles(authorDirectory, "Author");

			Files.AddRange(authorFiles);
		}

		private List<CssFile> getFiles(String directory, String subfolder = null)
		{
			return Directory.GetFiles(directory)
				.OrderBy(f => f)
				.Select(f => new CssFile(f, subfolder))
				.OrderBy(f => f.Local)
				.ToList();
		}

		public List<CssFile> Files { get; set; }


		public class CssFile
		{
			public String Name { get; }
			public String Media { get; }
			public Boolean Local { get; }

			public CssFile(String file, String path = null)
			{
				Name = new FileInfo(file).Name;

				if (path != null)
				{
					Name = Path.Combine(path, Name);
				}

				if (Name.Contains("_"))
				{
					var beforeUnderline = Name.LastIndexOf("_");
					Media = Name.Remove(beforeUnderline).ToLower();
				}

				Local = Name.EndsWith("_Local.css");
			}
		}
	}
}
