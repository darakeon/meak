using System;
using System.Collections.Generic;
using System.IO;

namespace Presentation.Models.General
{
    public class CssModel
    {
        public CssModel(String path)
        {
            Files = new List<CssFile>();

            foreach (var file in Directory.GetFiles(path))
            {
                Files.Add(new CssFile(file));
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                var directoryName = directory.Substring(directory.LastIndexOf(@"\"));

                foreach (var file in Directory.GetFiles(directory))
                {
                    Files.Add(new CssFile(file, directoryName));
                }
            }
        }

        public IList<CssFile> Files { get; set; }


        public class CssFile
        {
            public String Name;
            public String Media;

            public CssFile(String file)
            {
                Name = file.Substring(file.LastIndexOf(@"\") + 1);

                if (Name.Contains("_"))
                {
                    Media = Name.Remove(Name.LastIndexOf("_")).ToLower();
                }
            }

            public CssFile(String file, String path) : this(file)
            {
                Name = Path.Combine(path, Name);
            }
        }
    }
}