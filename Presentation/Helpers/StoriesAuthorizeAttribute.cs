using System;
using System.Web;
using System.Web.Mvc;
using Structure.Helpers;

namespace Presentation.Helpers
{
    public class StoriesAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = !UrlUserType.IsAuthor(httpContext.Request.Url)
                             || base.AuthorizeCore(httpContext);
            
            var isAjax = httpContext.Request.IsAjaxRequest();

            if (authorized)
                return true;

            if (isAjax)
                throw new ApplicationException();

            return false;
        }

        


    }
}