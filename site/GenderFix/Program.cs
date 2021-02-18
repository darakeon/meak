using System;

namespace GenderFix
{
	class Program
	{
		static void Main(string[] args)
		{
			var text = "Você beijou ela?";

			Console.WriteLine(new Gender().Remove(text));
			Console.ReadLine();
		}
	}
}
