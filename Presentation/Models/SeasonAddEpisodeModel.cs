using System;
using System.Linq;
using Presentation.Helpers;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonAddEpisodeModel : BaseModel
    {
        public SeasonAddEpisodeModel()
        {
            Init();
        }

        public SeasonAddEpisodeModel(Paths paths) : base(paths)
        {
            Init();

            var lastSeason = Menu.SeasonList.LastOrDefault() ?? new Season();
            var lastEpisode = lastSeason.EpisodesList.LastOrDefault() ?? new Episode();

            SeasonEpisode.NewEp(lastSeason.ID, lastEpisode.ID);

        }

        public void Init()
        {
            SeasonEpisode = new SeasonEpisodeScene();
        }


        public SeasonEpisodeScene SeasonEpisode { get; set; }

        public String Title { get; set; }
    }
}