using System;
using System.Configuration;

namespace Structure.Helpers
{
	public static class Config
	{
		public static String Login = get("login");

		public static String Pass = get("pass");

		public static String StoriesPath = get("Stories");
		public static String MessagesPath = get("Messages");

		public static DateTime CountdownStart = DateTime.Parse(get("CountdownStart"));
		public static Int32 EpisodesInterval = Int32.Parse(get("EpisodesInterval"));
		public static Int32 EpisodesHiatus = Int32.Parse(get("EpisodesHiatus"));

		public static String FtpAddress = get("FtpAddress");
		public static String FtpUrl = "ftp://" + FtpAddress;
		public static String FtpLogin = get("FtpLogin");
		public static String Site = get("Site");

		private static string get(String key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
