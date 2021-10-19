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
			getFiles(path);

			if (Config.IsAuthor)
				getFiles(path, "Author");
		}

		private void getFiles(String main, String subfolder = null)
		{
			if (subfolder != null)
				main = Path.Combine(main, subfolder);

			foreach (var file in Directory.GetFiles(main))
			{
				Files.Add(new CssFile(file, subfolder));
			}
		}

		public List<CssFile> Files = new();

		public class CssFile
		{
			public String Name { get; }
			public String Media { get; }
			public Boolean Local { get; }

			public CssFile(String file, String subfolder)
			{
				Name = new FileInfo(file).Name;

				if (Name.Contains("_"))
				{
					var beforeUnderline = Name.LastIndexOf("_");
					Media = Name.Remove(beforeUnderline).ToLower();
				}

				Local = Name.EndsWith("_Local.css");

				if (subfolder != null)
				{
					Name = Path.Combine(subfolder, Name);
				}
			}
		}
	}
}
