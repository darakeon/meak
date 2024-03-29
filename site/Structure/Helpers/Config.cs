﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Structure.Helpers
{
	public static class Config
	{
		private static IConfiguration dic;

		public static String StoriesPath => dic["Stories"];

		public static Boolean IsAuthor =>
			#if DEBUG
				Boolean.Parse(dic["IsAuthor"]);
			#else
				false;
			#endif

		public static Boolean FixerReview => Boolean.Parse(dic["FixerReview"]);

		public static String Version =>
			typeof(Version).Assembly.GetName().Version?.ToString();

		public static void Init(String environment = null)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true);

			if (environment != null)
			{
				builder.AddJsonFile(
					$"appsettings.{environment}.json", true
				);
			}

			dic = builder.Build();
		}
	}
}
