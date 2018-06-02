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
				warnIfNotFind
			);

			converter.Convert();

			Console.WriteLine("Done!");

			Console.ReadLine();
		}

		private static void warnIfNotFind(List<String> warnings)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			warnings.ForEach(Console.WriteLine);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
