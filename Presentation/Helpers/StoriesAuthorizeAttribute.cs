using System.Web;
using System.Web.Mvc;
using Structure.Helpers;

namespace Presentation.Helpers
{
    public class StoriesAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return !UrlUserType.IsAuthor(httpContext.Request.Url)
                || base.AuthorizeCore(httpContext);
        }
    }
}