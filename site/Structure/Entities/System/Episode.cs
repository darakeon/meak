using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Data;
using Structure.Enums;
using Structure.Helpers;

namespace Structure.Entities.System
{
	public class Episode
	{
		public Episode()
		{
			Season = new Season();
			BlockList = new List<Block>();
		}

		public static Episode Get(String path, String season, String episode)
		{
			var info = MainInfoJson.Get(path, season, episode);

			if (info == null)
				return null;

			return new Episode
			{
				ID = episode,
				Title = info.Title,
				Link = info.Link,
				Publish = Countdown.GetDate(season, episode),
				LastBlock = info.Last,
				Synopsis = info.Synopsis,
				Summary = info.Summary,
				Season = new Season {ID = season},
			};
		}

		public String ID { get; set; }

		public String Title { get; set; }
		public String Link { get; set; }
		public DateTime Publish { get; set; }
		public String LastBlock { get; set; }
		public String Synopsis { get; set; }
		public String Summary { get; set; }

		public List<Block> BlockList { get; set; }
		public IList<String> NoGender { get; set; }

		public Season Season { get; set; }

		public Boolean IsPublished()
		{
			return Publish < DateTime.Now || Config.IsAuthor;
		}

		public bool HasSummary()
		{
			return !String.IsNullOrEmpty(Summary) || Config.IsAuthor;
		}

		public override String ToString()
		{
			return ID;
		}

		public Block this[String block]
		{
			get { return BlockList.SingleOrDefault(s => s.ID == block); }
		}

		public Boolean HasGenderFix(TalkStyle style, String character)
		{
			return style == TalkStyle.Teller
			    || NoGender.Contains(character);
		}

		public ParagraphType? GlobalParagraphType(Block block, Int32 paragraph)
		{
			while (block.ParagraphTypeList.Count <= paragraph)
			{
				var index = BlockList.IndexOf(block) + 1;
				if (index >= BlockList.Count)
					return null;

				paragraph -= block.ParagraphTypeList.Count;
				block = BlockList[index];
			}

			return block.ParagraphTypeList[paragraph];
		}
	}
}
