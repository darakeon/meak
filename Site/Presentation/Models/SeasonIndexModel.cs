using System.Collections.Generic;
using Structure.Entities;

namespace Presentation.Models
{
    public class SeasonIndexModel : BaseModel
    {
        public SeasonIndexModel()
        {
            Messages = MessageXML.GetAll();
        }

        public IList<Message> Messages { get; set; }

    }
}