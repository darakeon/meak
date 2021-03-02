using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Presentation.Startup;

namespace Presentation
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(b => b.UseStartup<Main>())
				.Build()
				.Run();
		}
	}
}
