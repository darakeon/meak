using System;

namespace Structure.Extensions
{
	public static class StringExtension
	{
		public static Boolean IsName(this String word)
		{
			return !String.IsNullOrEmpty(word)
				&& word[0].ToString() == word[0].ToString().ToUpper();
		}
	}
}