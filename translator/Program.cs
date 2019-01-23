using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Translator
{
	public class Program
	{
		public static void Main(params String[] args)
		{
			setCurrentDirectory();

			var converter = FileToJson.Get(
				Console.WriteLine,
				warnIfNotFound
			);

			if (args.Length > 0)
				args.ToList().ForEach(converter.Convert);
			else
				converter.Convert();
			
			Console.WriteLine("Done!");

			Console.ReadLine();
		}

		private static void setCurrentDirectory()
		{
			Directory.SetCurrentDirectory(
				AppDomain.CurrentDomain.BaseDirectory
			);
		}

		private static void warnIfNotFound(List<String> warnings)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			warnings.ForEach(Console.WriteLine);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
