using System;
using System.Collections.Generic;
using System.Linq;
using Structure.Data;
using Structure.Enums;
using StringExtension = Structure.Extensions.StringExtension;

namespace Presentation.Models
{
    public class SeasonEditEpisodeModel : SeasonEpisodeModel
    {
        protected SeasonEditEpisodeModel() { }
        public SeasonEditEpisodeModel(Paths paths) : base(paths) { }

        public String SceneCounter { get; set; }
        public Int32 TellerCounter { get; set; }
        public Int32 TalkCounter { get; set; }
        public Int32 ParagraphCounter { get; set; }
        public Int32 PieceCounter { get; set; }

        public Int32 TabIndex { get; set; }

        public String[] CharacterList { get; set; }
        public String[] TalkStyleList { get; set; }
        public String[] TellerStyleList { get; set; }



        public ParagraphType CurrentParagraph
        {
            get
            {
                return Story[SceneCounter].ParagraphTypeList[ParagraphCounter];
            }
        }


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

            Story.SceneList
                .ForEach(s => s.TalkList
                    .Select(t => t.Character)
                    .ToList()
                    .AddRange(characterList));

            CharacterList = 
                characterList
                    .Distinct()
                    .Where(c => !c.Contains("/")
                        && StringExtension.IsName(c))
                    .OrderBy(c => c)
                    .ToArray();
        }

    }
}