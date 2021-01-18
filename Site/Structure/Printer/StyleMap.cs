using System.Collections.Generic;
using Structure.Enums;
using Structure.Extensions;

namespace Structure.Printer
{
	public class StyleMap : Dictionary<object, CharMap>
	{
		private StyleMap() { }

		public static StyleMap New<T>(ParagraphType type)
		{
			var map = new StyleMap();
			map.fill<T>(type);
			return map;
		}

		private void fill<T>(ParagraphType type)
		{
			foreach (var style in EnumX.All<T>())
			{
				var name = style.ToString().ToLower();
				var charMap = new CharMap(type, name);
				Add(style, charMap);
			}
		}

	}
}
