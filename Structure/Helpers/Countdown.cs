using System;

namespace Structure.Helpers
{
    public class Countdown
    {
        public static String GetTimeLeft()
        {
            var allTime = DateTime.UtcNow - Config.CountdownStart;

            var timeLeft = Config.CountdownFrequency -
                (allTime.TotalDays % Config.CountdownFrequency);

            var positions = Math.Ceiling(Math.Log10(Config.CountdownFrequency));
            var format = Math.Pow(10, positions).ToString().Replace("1", "");

            return Math.Ceiling(timeLeft).ToString(format);
        }

    }
}
