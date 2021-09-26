using System.IO;
using System.Reflection;

namespace DiagramBuilder.Helper
{
    public static class HelperBuilder
    {
        public static Stream LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream(resourceName);
        }

        public static string LoadResourceString(string resourceName)
        {
            using (Stream stream = LoadResource(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static void SaveStream(Stream stream, string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using(var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}