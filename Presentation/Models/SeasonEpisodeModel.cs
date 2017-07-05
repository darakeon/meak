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
            Next = new SeasonEpisodeScene();
            Prev = new SeasonEpisodeScene();
        }

        public Scene Story { get; set; }

        public SeasonEpisodeScene Next { get; set; }
        public SeasonEpisodeScene Prev { get; set; }
        
    }
}