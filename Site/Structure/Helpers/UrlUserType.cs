using System;
using System.Web;

namespace Structure.Helpers
{
    public static class UrlUserType
    {
        public static Boolean IsAuthor()
        {
            return IsAuthor(HttpContext.Current.Request.Url);
        }

        public static Boolean IsAuthor(Uri uri)
        {
            return uri.AbsolutePath.StartsWith("/_");
        }
    }
}