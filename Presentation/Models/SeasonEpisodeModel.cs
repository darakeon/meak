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
        }

        public Scene Story { get; set; }
        
    }
}