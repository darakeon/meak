using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Structure.Helpers;

namespace Presentation.Helpers
{
    public class StoriesAuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!Config.IsAuthor)
			{
				context.HttpContext.Response.Redirect("/");
			}
		}
	}
}
