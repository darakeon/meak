using System;
using System.IO;
using Newtonsoft.Json;

namespace Structure.Extensions
{
	static class JsonExtension
	{
		public static T Read<T>(this String path)
		{
			try
			{
				if (!File.Exists(path))
					return default(T);

				var content = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<T>(content);
			}
			catch (Exception e)
			{
				throw new Exception($"Error on {path}", e);
			}
		}

		public static void Write<T>(this String path, T obj)
		{
			try
			{
				var content = JsonConvert.SerializeObject(
					obj, Formatting.Indented
				);

				File.WriteAllText(path, content);
			}
			catch (Exception e)
			{
				throw new Exception($"Error on {path}", e);
			}
		}
	}
}
