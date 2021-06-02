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

        //public static ZipArchiveEntry CreateFromResource(ZipArchive zipArchive, string entryName, string resourceName)
        //{
        //    var entry = zipArchive.CreateEntry(entryName);
        //    using (var str = entry.Open())
        //    using (var resourceStream = LoadResource(resourceName))
        //    {
        //        resourceStream.CopyTo(str);
        //    }

        //    return entry;
        //}

        //public static byte[] BuildZip(string graph, IDictionary<string, string> descriptions)
        //{
        //    byte[] bytes;

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
        //        {
        //            CreateFromResource(zipArchive, "mermaid.min.js", "DiagramBuilder.HtmlData.assets.js.docs.js");
        //            //CreateFromResource(zipArchive, "popper.min.js", "DiagramBuilder.Data.popper.min.js");
        //            //CreateFromResource(zipArchive, "tippy.min.js", "DiagramBuilder.Data.tippy.min.js");

        //            var template = LoadResourceString("DiagramBuilder.HtmlData.documentation.html");

        //            var descriptionsJson = JsonConvert.SerializeObject(descriptions, Formatting.None);

        //            var resultHtml = template.Replace("##GRAPH##", graph).Replace("##DESCRIPTIONS##", descriptionsJson);

        //            var entry = zipArchive.CreateEntry("index.html");
        //            using (var str = entry.Open())
        //            using (var writer = new StreamWriter(str))
        //            {
        //                writer.Write(resultHtml);
        //            }
        //        }

        //        bytes = memoryStream.ToArray();
        //    }

        //    return bytes;
        //}

        //public static void SaveZip(string zipPath, string graph, IDictionary<string, string> descriptions)
        //{
        //    var bytes = BuildZip(graph, descriptions);
        //    File.WriteAllBytes(@"C:\Users\Yuri Pereira\source\repos\DiagramBuilder\DiagramBuilder\bin\Debug\netcoreapp3.1\teste.zip", bytes);
        //    //using (var str1 = File.OpenWrite(zipPath))
        //    //{
        //    //    str1.Write(bytes, 0, bytes.Length);
        //    //}
        //}
    }
}