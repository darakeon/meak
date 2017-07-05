using System.Web;
using System.Web.Mvc;
using Structure.Helpers;

namespace Presentation.Helpers
{
    public class StoriesAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UrlUserType.IsAuthor(httpContext.Request.Url))
                return base.AuthorizeCore(httpContext);
            
            return true;
        }
    }
}