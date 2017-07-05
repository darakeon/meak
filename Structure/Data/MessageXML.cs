using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ak.DataAccess.XML;
using Structure.Entities;
using Structure.Helpers;

namespace Structure.Data
{
    public class MessageXML
    {
        public String PathXML { get; private set; }


        public MessageXML(HttpServerUtilityBase server)
        {
            setXMLPath(server);
        }



        public IList<Message> GetAll()
        {
            var messages = new List<Message>();

            var messageFiles = getMessageFiles();

            foreach (var messageFile in messageFiles)
            {
                var node = new Node(messageFile.Path);

                var message = new Message
                {
                    Title = node["Title"],
                    Date = messageFile.Date
                };

                foreach (var child in node)
                {
                    message.Lines.Add(child.Value);
                }

                messages.Add(message);
            }

            return messages;
        }

        private IEnumerable<MessageFile> getMessageFiles()
        {
            return Directory.GetFiles(PathXML, "*.xml")
                .Select(f => new MessageFile(f))
                .OrderByDescending(f => f.Date)
                .Take(30);
        }



        private void setXMLPath(HttpServerUtilityBase server)
        {
            var folder = Config.MessagesPath;

            if (folder == null)
                throw new Exception("XML Path not configured.");

            var isAbsolutePath = folder.Length > 1
                && folder.Substring(1, 1) == ":";

            PathXML =
                isAbsolutePath
                    ? folder
                    : Path.Combine(server.MapPath("~"), folder);

            if (!Directory.Exists(PathXML))
                throw new Exception(String.Format("Path '{0}' doesn't exists.", PathXML));
        }



        class MessageFile
        {
            public MessageFile(String path)
            {
                Path = path;
                Date = File.GetLastWriteTime(path);
            }

            public String Path { get; private set; }
            public DateTime Date { get; private set; }
        }

    }
}
