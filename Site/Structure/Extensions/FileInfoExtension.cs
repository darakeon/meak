using System;
using System.IO;

namespace Structure.Extensions
{
	public static class FileInfoExtension
	{
		public static String NameWithoutExtension(this FileInfo fileInfo)
		{
			var name = fileInfo.Name;
			var extension = fileInfo.Extension;

			return name.Substring(0, name.LastIndexOf(extension));
		}

		public static Boolean CreateIfNotExists(this FileInfo fileInfo, String content)
		{
			var sceneExists = fileInfo.Exists;

			if (sceneExists) return false;


			var pathExists = Directory.Exists(fileInfo.DirectoryName);

			if (!pathExists)
				Directory.CreateDirectory(fileInfo.DirectoryName);


			var file = new StreamWriter(fileInfo.FullName);

			file.WriteLine(content);

			file.Flush(); file.Close();


			return true;
		}


	}
}
