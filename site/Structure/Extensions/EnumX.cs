using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Extensions
{
	public static class EnumX
	{
		public static T To<T>(this String text)
			where T : struct
		{
			return (T)Enum.Parse(typeof(T), text, true);
		}

		public static T To<T>(this object text)
			where T : struct
		{
			return text.ToString().To<T>();
		}

		public static IList<T> All<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().ToList();
		}
	}
}
