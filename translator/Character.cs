using System;

namespace Translator
{
	internal class Character: Replacer
	{
		public Character(String oldValue, String newValue)
			: base(
				$"\"character\": \"{oldValue}\"",
				$"\"character\": \"{newValue}\""
			) {}
	}
}