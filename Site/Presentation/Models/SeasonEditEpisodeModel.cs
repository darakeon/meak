using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Data;
using Structure.Enums;
using Structure.Extensions;

namespace Presentation.Models
{
	public class SeasonEditEpisodeModel : SeasonEpisodeModel
	{
		public SeasonEditEpisodeModel() { }

		public SeasonEditEpisodeModel(String seasonID, String episodeID, String blockID)
		{
			Story = episodeJson.GetEpisode(seasonID, episodeID);
			ReadingBlock = blockID ?? BlockJson.FIRST_BLOCK;
		}

		public Int32 BlockCounter { get; set; }
		public Int32 TellerCounter { get; set; }
		public Int32 TalkCounter { get; set; }
		public Int32 PageCounter { get; set; }
		public Int32 ParagraphCounter { get; set; }
		public Int32 PieceCounter { get; set; }

		public Int32 TabIndex { get; set; }

		public String[] CharacterList { get; set; }
		public String[] TalkStyleList { get; set; }
		public String[] TellerStyleList { get; set; }

		public ParagraphType CurrentParagraph => 
			Story.BlockList[BlockCounter].ParagraphTypeList[ParagraphCounter];

		public String CurrentBlock => 
			Story.BlockList[BlockCounter].ID;

		public void GetSuggestionLists()
		{
			TalkStyleList =
				Enum.GetNames(typeof(TalkStyle))
					.AsEnumerable()
					.ToArray();

			TellerStyleList =
				Enum.GetNames(typeof(TellerStyle))
					.AsEnumerable()
					.ToArray();

			var characterList = new List<String>();

			Story.BlockList
				.ForEach(s =>
					characterList.AddRange(
						s.TalkList.Select(t => t.Character)
					)
				);

			CharacterList = 
				characterList
					.Distinct()
					.Where(c => c != null && !c.Contains("/") && c.IsName())
					.OrderBy(c => c)
					.ToArray();
		}
	}
}
