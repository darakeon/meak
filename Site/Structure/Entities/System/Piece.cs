using System;
using System.Collections.Generic;

namespace Structure.Entities.System
{
	public class Piece<T> where T : struct
	{
		public Piece() { }

		public Piece(T defaultStyle)
		{
			Style = defaultStyle;
		}


		public T Style { get; set; }
		public String Text { get; set; }
		public String Audio { get; set; }

		public List<Int32> DebugWords = new List<Int32>();

		public override String ToString()
		{
			return Text;
		}
	}
}
