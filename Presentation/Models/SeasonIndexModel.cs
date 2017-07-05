using System.Collections.Generic;
using Structure.Data;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonIndexModel : BaseModel
    {
        public SeasonIndexModel(Paths paths) : base(paths) { }

        public IList<Message> Messages { get; set; }

    }
}