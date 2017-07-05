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
        public static Int32 CountdownFrequency = Int32.Parse(get("CountdownFrequency"));

        public static String FtpUrl = get("FtpUrl");
        public static String FtpLogin = get("FtpLogin");
        public static String Site = get("Site");

        private static string get(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
