using System;
using System.Linq;
using Presentation.Helpers;
using Structure.Entities.System;

namespace Presentation.Models
{
    public class SeasonAddEpisodeModel : BaseModel
    {
        public SeasonEpisode SeasonEpisode { get; set; }
        public String Title { get; set; }



        public SeasonAddEpisodeModel()
        {
            init();

            var lastSeason = Menu.SeasonList.LastOrDefault() ?? new Season();
            var lastEpisode = lastSeason.EpisodeList.LastOrDefault() ?? new Episode();

            SeasonEpisode.NewEp(lastSeason.ID, lastEpisode.ID);
        }


        private void init()
        {
            SeasonEpisode = new SeasonEpisode();
        }


    }
}