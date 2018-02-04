using System;

namespace Presentation.Models.General
{
	public class LogOnModel
	{
		public Boolean Is { get; set; }

		public String ReturnUrl { get; set; }

		public String Login { get; set; }
		public String Password { get; set; }
	}
}