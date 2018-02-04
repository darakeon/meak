using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Structure.Entities.System;
using Structure.Extensions;
using Structure.Helpers;
using path = System.IO.Path;

namespace Structure.Data
{
	public class MessageJson
	{
		public String Path { get; private set; }

		public MessageJson()
		{
			setPath();
		}

		private void setPath()
		{
			var folder = Config.MessagesPath;

			if (folder == null)
				throw new Exception("XML Path not configured.");

			var isAbsolutePath = folder.Length > 1
								 && folder.Substring(1, 1) == ":";

			Path = isAbsolutePath
				? folder
				: path.Combine(Directory.GetCurrentDirectory(), folder);

			if (!Directory.Exists(Path))
				throw new Exception($"Path '{Path}' doesn't exists.");
		}


		public IList<Message> GetAll()
		{
			return getMessageFiles()
				.Select(getMessage)
				.ToList();
		}

		private IEnumerable<MessageFile> getMessageFiles()
		{
			return Directory.GetFiles(Path, "*.json")
				.Select(f => new MessageFile(f))
				.OrderByDescending(f => f.Date)
				.Take(30);
		}

		private static Message getMessage(MessageFile messageFile)
		{
			var message = messageFile.Path.Read<Message>();
			message.Date = messageFile.Date;
			return message;
		}

		class MessageFile
		{
			public MessageFile(String path)
			{
				Path = path;
				Date = File.GetLastWriteTime(path);
			}

			public String Path { get; }
			public DateTime Date { get; }
		}

	}
}
