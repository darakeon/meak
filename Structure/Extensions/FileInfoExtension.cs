using System;
using System.IO;

namespace Structure.Extensions
{
    public static class FileInfoExtension
    {
        public static String NameWithoutExtension(this FileInfo fileInfo)
        {
            var name = fileInfo.Name;
            var extension = fileInfo.Extension;

            return name.Substring(0, name.LastIndexOf(extension));
        }
    }
}
