using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Presentation.Startup
{
	public class Tls
	{
		public static void Https(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				var redirect = await genCertAndCheckRedirect(context);

				if (redirect)
				{
					var url = context.Request
						.GetDisplayUrl()
						.Insert(4, "s");

					context.Response.Redirect(url, false);
				}
				else
				{
					await next();
				}
			});
		}

		private static readonly String issuedPath = getPath("is_issued");

		// this script is inside darakeon/nginx image
		private static readonly String letsEncryptPath = getPath("cert-lets-encrypt.sh");

		private static readonly Process process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "bash",
				Arguments = $"-c {letsEncryptPath}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			}
		};

		private static async Task<Boolean> genCertAndCheckRedirect(HttpContext context)
		{
			var request = context.Request;

			if (request.IsHttps)
				return false;

			var path = request.Path.Value ?? "";

			if (path.StartsWith("/Assets"))
				return false;

			var domain = request.Host.Host;

			if (domain != "meak-stories.com")
			{
				Console.WriteLine($"Host [{domain}] cannot be validated for certificate");
				return false;
			}

			var issuedFile = new FileInfo(issuedPath);

			Console.WriteLine($"File '{issuedFile.FullName}' exists: {issuedFile.Exists}");

			if (issuedFile.Exists)
			{
				var issuedContent = await File.ReadAllTextAsync(issuedPath);
				var issued = Boolean.Parse(issuedContent);

				var checkDays = issued ? 60 : 5;
				var validUntil = issuedFile.CreationTime.AddDays(checkDays);
				var stillValid = validUntil > DateTime.Now;

				Console.WriteLine($"Issued: {issued}");
				Console.WriteLine($"Validity: {validUntil}");
				Console.WriteLine($"Valid: {stillValid}");

				if (stillValid)
				{
					return issued;
				}
			}

			await File.WriteAllTextAsync(issuedPath, false.ToString());

			if (!File.Exists(letsEncryptPath))
			{
				Console.WriteLine($"{letsEncryptPath} does not exist");
				return false;
			}

			var started = process.Start();

			Console.WriteLine(await File.ReadAllTextAsync("/etc/nginx/conf.d/default.conf"));

			if (!started)
			{
				Console.WriteLine($"Process could not be executed: {letsEncryptPath}");
				return false;
			}

			Console.WriteLine(await File.ReadAllTextAsync("/etc/nginx/conf.d/default.conf"));

			await process.WaitForExitAsync();
			var isOk = process.ExitCode == 0;

			Console.WriteLine(await File.ReadAllTextAsync("/etc/nginx/conf.d/default.conf"));

			var result = isOk
				? process.StandardOutput
				: process.StandardError;

			Console.WriteLine(await result.ReadToEndAsync());

			if (isOk)
			{
				await File.WriteAllTextAsync(issuedPath, true.ToString());
			}

			return isOk;
		}

		private static String getPath(String file)
		{
			var relative = Path.Combine("..", "cert", file);
			var info = new FileInfo(relative);
			return info.FullName;
		}
	}
}
