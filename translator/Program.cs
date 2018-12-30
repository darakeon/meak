using System;
using System.Collections.Generic;

namespace Translator
{
	public class Program
	{
		public static void Main()
		{
			var converter = FileToJson.Get(
				Console.WriteLine,
				warnIfNotFound
			);

			converter.Convert();

			Console.WriteLine("Done!");

			Console.ReadLine();
		}

		private static void warnIfNotFound(List<String> warnings)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			warnings.ForEach(Console.WriteLine);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
