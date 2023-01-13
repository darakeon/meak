using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Structure.Helpers;

namespace Presentation.Startup
{
	public class Main
	{
		// Add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			PrometheusConfig.Start();
		}

		// HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			PrometheusConfig.UseMetrics(app);

			Directory.SetCurrentDirectory(env.ContentRootPath);
			Config.Init(env.EnvironmentName);
			Rewrite.Apply(app);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			Static.Configure(app);

			app.UseRouting();

			app.UseAuthorization();

			Route.Config(app);
		}
	}
}
