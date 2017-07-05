using Presentation.Helpers;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonEpisodeModel : BaseModel
    {
        public SeasonEpisodeModel() {
            Story = new Episode();
        }

        public SeasonEpisodeModel(Paths paths) : base(paths)
        {
            Story = new Episode();
            Next = new SeasonEpisode();
            Prev = new SeasonEpisode();
        }

        public Episode Story { get; set; }

        public SeasonEpisode Next { get; set; }
        public SeasonEpisode Prev { get; set; }
        
    }
}