using System;
using Structure.Entities.System;

namespace Presentation.Models
{
	public class PageFrame
	{
		public PageFrame(Episode episode, Int32 page)
		{
			Series = "MEAK";
			Code = $"{episode.Season}{episode}";
			Title = episode.Title;
			Page = page;
		}

		public String Series { get; }
		public String Code { get; }
		public String Title { get; }
		public Int32 Page { get; }
	}
}
