using System;
using Structure.Data;
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
        }

        public Episode Story { get; set; }

        public String ReadingScene { get; set; }
        
    }
}