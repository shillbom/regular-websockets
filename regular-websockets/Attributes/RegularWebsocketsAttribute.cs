using System;

namespace RegularWebsockets.Attributes
{
    public class RegularWebsocketsAttribute : Attribute
    {
        public readonly string path;
        public RegularWebsocketsAttribute(string path)
        {
            this.path = path;
        }
    }
}