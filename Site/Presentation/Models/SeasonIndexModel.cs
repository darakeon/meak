using System.Collections.Generic;
using Structure.Entities.System;

namespace Presentation.Models
{
	public class SeasonIndexModel : BaseModel
	{
		public SeasonIndexModel()
		{
			Messages = messageJson.GetAll();
		}

		public IList<Message> Messages { get; set; }

	}
}