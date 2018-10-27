using System;
using System.Collections.Generic;
using System.Linq;

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
			var day = Config.CountdownStart;
			var interval = Config.EpisodesInterval;
			var hiatus = Config.EpisodesHiatus;
			var dates = new List<DateTime?>();

			for (var e = 1; e <= 20; e++)
			{
				dates.Add(day);

				var add = e % 5 == 4 ? hiatus : interval;

				day = day.AddDays(add);
			}

			var nextYearStart = Config.CountdownStart.AddYears(1);

			var nextEpisode =
				dates.FirstOrDefault(d => d >= today)
					?? nextYearStart;

			return (nextEpisode - today).TotalDays;
		}
	}
}
