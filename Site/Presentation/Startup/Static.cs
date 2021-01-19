using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Presentation.Startup
{
	static class Static
	{
		public static void Files(IApplicationBuilder app)
		{
			var folder = "Assets";
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
					Path.Combine(Directory.GetCurrentDirectory(), folder)),
				RequestPath = "/" + folder,
			});
		}
	}
}
