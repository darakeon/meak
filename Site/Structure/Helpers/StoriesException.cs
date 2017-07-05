using System;

namespace Structure.Helpers
{
    public class StoriesException : Exception
    {
        public StoriesException(String message)
            : base(message) { }
    }
}