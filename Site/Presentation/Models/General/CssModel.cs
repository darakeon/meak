using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
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

			var isAuthor = UrlUserType.IsAuthor(HttpContext.Current.Request.Url);

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
				Name = file.Substring(file.LastIndexOf(@"\") + 1);

				if (Name.Contains("_"))
				{
					Media = Name.Remove(Name.LastIndexOf("_")).ToLower();
				}
			}

			public CssFile(String file, String path) : this(file)
			{
				Name = Path.Combine(path, Name);
			}
		}
	}
}