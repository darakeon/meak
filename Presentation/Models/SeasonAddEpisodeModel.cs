using System;
using System.Linq;
using Presentation.Helpers;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonAddEpisodeModel : BaseModel
    {
        public SeasonEpisode SeasonEpisode { get; set; }
        public String Title { get; set; }



        public SeasonAddEpisodeModel()
        {
            init();
        }

        public SeasonAddEpisodeModel(Paths paths) : base(paths)
        {
            init();

            var lastSeason = Menu.SeasonList.LastOrDefault() ?? new Season();
            var lastEpisode = lastSeason.EpisodesList.LastOrDefault() ?? new Episode();

            SeasonEpisode.NewEp(lastSeason.ID, lastEpisode.ID);
        }


        private void init()
        {
            SeasonEpisode = new SeasonEpisode();
        }


    }
}