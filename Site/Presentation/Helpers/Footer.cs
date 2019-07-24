using System;

namespace Presentation.Helpers
{
	public class Footer
	{
		public Footer(Int32? breaks, Int32 page)
		{
			Breaks = breaks ?? 0;
			Page = page;
		}

		public Int32 Breaks { get; }
		public Int32 Page { get; }
	}
}
