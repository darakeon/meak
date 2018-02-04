using System;

namespace Structure.Extensions
{
	static class EnumExtension
	{
		public static T GetEnum<T>(this String text)
			where T : struct
		{
			return (T) Enum.Parse(typeof(T), text, true);
		}
	}
}
