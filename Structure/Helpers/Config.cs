using System;
using System.Configuration;

namespace Structure.Helpers
{
    public static class Config
    {
        public static String Login = ConfigurationManager.AppSettings["login"];
        public static String Pass = ConfigurationManager.AppSettings["pass"];

        public static String StoriesPath = ConfigurationManager.AppSettings["Stories"];
        public static String MessagesPath = ConfigurationManager.AppSettings["Messages"];

        public static DateTime CountdownStart = DateTime.Parse(ConfigurationManager.AppSettings["CountdownStart"]);
        public static Int32 CountdownFrequency = Int32.Parse(ConfigurationManager.AppSettings["CountdownFrequency"]);
    }
}
