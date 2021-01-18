using System;
using System.Web;

namespace Structure.Helpers
{
	public static class UrlUserType
	{
		public static Boolean IsAuthor()
		{
			var request = HttpContext.Current.Request;

			var route = RouteStories.Empty();

			return request.Url.DnsSafeHost == "localhost"
				&& request.Url.AbsolutePath != "/"
				&& !request.RawUrl.EndsWith(route.ToString());
		}
	}
}