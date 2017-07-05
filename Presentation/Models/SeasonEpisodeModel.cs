using Presentation.Helpers;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonEpisodeModel : BaseModel
    {
        public SeasonEpisodeModel() {
            Story = new Scene();
        }

        public SeasonEpisodeModel(Paths paths) : base(paths)
        {
            Story = new Scene();
            Next = new SeasonEpisode();
            Prev = new SeasonEpisode();
        }

        public Scene Story { get; set; }

        public SeasonEpisode Next { get; set; }
        public SeasonEpisode Prev { get; set; }
        
    }
}