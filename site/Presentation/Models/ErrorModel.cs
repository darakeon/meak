using System;

namespace Presentation.Models
{
	public class ErrorModel : BaseModel
	{
		public Exception Error { get; set; }
	}
}
