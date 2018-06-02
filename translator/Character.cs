using System;

namespace Translator
{
	internal class Character: Replace
	{
		public Character(String oldValue, String newValue)
			: base(
				$"\"character\": \"{oldValue}\"",
				$"\"character\": \"{newValue}\""
			) {}
	}
}