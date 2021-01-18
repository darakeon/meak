using System;
using System.Web;

namespace Structure.Helpers
{
	public static class UrlUserType
	{
		public static Boolean IsAuthor()
		{
			return Config.IsAuthor;
		}
	}
}
