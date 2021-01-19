using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json;

namespace Presentation.Startup
{
	class Rewrite
	{
		private class Dic : Dictionary<String, List<String>> { }
		private readonly Dic values;

		private Rewrite(String path)
		{
			var json = File.ReadAllText(path);
			values = JsonConvert.DeserializeObject<Dic>(json);
		}

		private delegate void each(String origin, String destiny);
		private void forEach(each action)
		{
			values.Select(
					r => r.Value.Select(v => new
					{
						origin = v,
						destiny = r.Key
					})
				)
				.SelectMany(r => r)
				.ToList()
				.ForEach(r => action(r.origin, r.destiny));
		}

		public static void TestThemAll(IApplicationBuilder app)
		{
			var options = new RewriteOptions();

			new Rewrite("rewrites.json").forEach(
				(origin, destiny) =>
					options.AddRewrite(origin, destiny, true)
			);

			app.UseRewriter(options);
		}
	}
}
