using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Structure.Helpers
{
	public static class Config
	{
		private static IConfiguration dic;

		public static String Login => dic["login"];

		public static String Pass => dic["pass"];

		public static String StoriesPath => dic["Stories"];

		public static String FtpAddress => dic["FtpAddress"];
		public static String FtpUrl => "ftp://" + FtpAddress;
		public static String FtpLogin => dic["FtpLogin"];
		public static String Site => dic["Site"];

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
				.AddJsonFile("appSettings.json", true);

			if (environment != null)
			{
				builder.AddJsonFile(
					$"appSettings.{environment}.json", true
				);
			}

			dic = builder.Build();
		}
	}
}
