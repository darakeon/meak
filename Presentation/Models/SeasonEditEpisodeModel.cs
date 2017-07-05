using System;
using System.Linq;
using Structure.Enums;

namespace Presentation.Models
{
    public class SeasonEditEpisodeModel : SeasonEpisodeModel
    {
        protected SeasonEditEpisodeModel() { }
        public SeasonEditEpisodeModel(Paths paths) : base(paths) { }

        public Int32 TellerCounter { get; set; }
        public Int32 TalkCounter { get; set; }
        public Int32 ParagraphCounter { get; set; }
        public Int32 PieceCounter { get; set; }

        public Int32 TabIndex { get; set; }

        public String[] CharacterList { get; set; }
        public String[] TalkStyleList { get; set; }
        public String[] TellerStyleList { get; set; }



        public eParagraph CurrentParagraph
        {
            get
            {
                return Story.ParagraphTypeList[ParagraphCounter];
            }
        }


        public void GetSuggestionLists()
        {
            TalkStyleList =
                Enum.GetNames(typeof(eTalkStyle))
                    .AsEnumerable()
                    .ToArray();

            TellerStyleList =
                Enum.GetNames(typeof(eTellerStyle))
                    .AsEnumerable()
                    .ToArray();

            CharacterList =
                Story.TalkList
                    .Select(t => t.Character)
                    .Distinct()
                    .Where(c => !c.Contains("/")
                        && c.IsName())
                    .OrderBy(c => c)
                    .ToArray();
        }
    }
}