using System;

namespace Structure.Helpers
{
	public class Countdown
	{
		public static String GetTimeLeft()
		{
			return getDaysLeft().ToString("00");
		}

		private static Double getDaysLeft()
		{
			var today = DateTime.Now.Date;

			var allTime = today - Config.CountdownStart;

			var episodesDone = allTime.TotalDays/Config.CountdownFrequency;

			if (episodesDone > 19)
			{
				var nextYearStart = Config.CountdownStart.AddYears(1);

				return (nextYearStart - today).TotalDays;
			}

			var passedDays = (allTime.TotalDays-1)
				% Config.CountdownFrequency + 1;

			var timeLeft = Config.CountdownFrequency - passedDays;

			return Math.Ceiling(timeLeft);
		}
	}
}
