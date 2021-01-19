using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Structure.Helpers;

namespace Presentation.Helpers
{
    public class StoriesAuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!UrlUserType.IsAuthor())
			{
				context.HttpContext.Response.Redirect("/");
			}
		}
	}
}
