using System.IO;

namespace DiagramBuilder.Html
{
    internal class HtmlResource
    {
        public HtmlResource(string path, Stream stream)
        {
            Path = path;
            Stream = stream;
        }

        public string Path { get; set; }
        public Stream Stream { get; set; }
    }
}