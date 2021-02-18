using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Structure.Extensions
{
	static class JsonExtension
	{
		public const String NewLine = "\n";

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
				var utf8 = new UTF8Encoding(false);

				using (var file = File.Create(path))
				using (var streamWriter = getStreamWriter(file, utf8))
				using (var jsonWriter = getJsonWriter(streamWriter))
				{
					Serializer.Serialize(jsonWriter, obj);
				}

				File.AppendAllText(path, NewLine);
			}
			catch (Exception e)
			{
				throw new Exception($"Error on {path}", e);
			}
		}

		private static StreamWriter getStreamWriter(Stream stream, Encoding encoding)
		{
			return new StreamWriter(stream, encoding)
			{
				NewLine = NewLine
			};
		}

		private static JsonTextWriter getJsonWriter(StreamWriter stream)
		{
			return new JsonTextWriter(stream)
			{
				Formatting = Formatting.Indented,
				Indentation = 1,
				IndentChar = '\t',
			};
		}

		public static JsonSerializer Serializer
		{
			get
			{
				var serializer = new JsonSerializer
				{
					ContractResolver = new lowerCaseResolver(),
					NullValueHandling = NullValueHandling.Ignore,
					SerializationBinder = new DefaultSerializationBinder(),
					Converters = { 
						new StringEnumConverter
						{
							NamingStrategy = new CamelCaseNamingStrategy()
						},
						new IsoDateTimeConverter
						{
							DateTimeFormat = "yyyy-MM-dd",
						},
					},
				};

				return serializer;
			}
		}
		
		private sealed class lowerCaseResolver : DefaultContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var property = base.CreateProperty(member, memberSerialization);
				property.PropertyName = property.PropertyName.ToLower();
				return property;
			}
		}
	}
}
