using System.Collections.Generic;
using Structure.Enums;

namespace Structure.Printer
{
	public class ParagraphMap : Dictionary<ParagraphType, StyleMap>
	{
		public void Add<T>(ParagraphType type)
		{
			Add(type, StyleMap.New<T>(type));
		}
	}
}
