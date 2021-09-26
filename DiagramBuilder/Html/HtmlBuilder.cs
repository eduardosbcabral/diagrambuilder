using ClassStructureJson;
using DiagramBuilder.Helper;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiagramBuilder.Html
{
    public class HtmlBuilder
    {
        private const string ResourceBasePath = "DiagramBuilder.Html.data.";
        private readonly StringBuilder Menu;
        private readonly StringBuilder Scripts;
        private string _applicationName;

        public HtmlBuilder()
        {
            Menu = new StringBuilder();
            Scripts = new StringBuilder();
        }

        /// <summary>
        /// Build the diagram as HTML
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns>Return the diagram as HTML</returns>
        private string BuildSection(IDiagram diagram)
        {
            var id = UniqueIdentifier.Create("item_");
            var name = diagram.Title();

            var html = $@"<section class=""docs-section"" id=""{id}"">
                            <h2 class=""section-heading"">{name}</h2>
                            <div class=""mermaid"">
                                {diagram.Compile()}
                            </div>
                        </section>";

            Menu.AppendLine(BuildMenu(id, name));

            return html;
        }

        private string BuildSection(HtmlCustomDiagram custom)
        {
            var id = UniqueIdentifier.Create("item_");
            var name = custom.Diagram.Title();

            var preDiagram = new StringBuilder();
            custom.ClassesPreDiagram.ForEach(c => preDiagram.AppendLine(BuildClassDiagram(c)));

            var posDiagram = new StringBuilder();
            custom.ClassesPosDiagram.ForEach(c => posDiagram.AppendLine(BuildClassDiagram(c)));

            var html = $@"<section class=""pt-10"" id=""{id}"">
                            <p class=""text-4xl font-bold"">{name}</p>
                            {preDiagram}
                            <div class=""mermaid flex justify-center pt-5 pb-5"">
                                {custom.Diagram.Compile()}
                            </div>
                            {posDiagram}
                        </section>";

            Menu.AppendLine(BuildMenu(id, name));

            return html;
        }

        private string BuildClassDiagram(HtmlClassDiagram classDiagram)
        {
            var html = $@"<p class=""pt-2 text-xl font-bold"">{classDiagram.Title}</p>";

            if (string.IsNullOrWhiteSpace(classDiagram.Description) == false)
                html += $@"<p>{classDiagram.Description}</p>";

            html += $@"<pre id=""{classDiagram.Id}"" class=""prettyprint"" style=""background-color: #f8f8f8;border: none;padding: 1em 2em;"">{BuildClassJsonScript(classDiagram)}</pre>";

            Scripts.AppendLine();

            return html;
        }

        private string BuildMenu(string id, string name)
            => $@"
                <li class=""text-left"">
                    <a class=""block px-4 py-2 mt-2 text-md font-semibold text-gray-700 hover:text-gray-900 focus:text-gray-900 hover:bg-gray-200 focus:bg-gray-200 focus:outline-none focus:shadow-outline"" 
                        href=""#{id}"">
                        {name}
                    </a>
                </li>
            ";        

        private string BuildClassJsonScript(HtmlClassDiagram classDiagram)
            => classDiagram.Entity.GetType().GetClassStructure(new SnakeCaseNamingStrategy());

        /// <summary>
        /// Build the diagrams as HTML
        /// </summary>
        /// <param name="diagrams"></param>
        /// <returns>Return the diagrams as HTML</returns>
        private string Build(IEnumerable<IDiagram> diagrams)
        {
            var body = new StringBuilder();

            foreach (var diagram in diagrams)
                body.Append(BuildSection(diagram));

            return body.ToString();
        }

        /// <summary>
        /// Build the diagrams as HTML
        /// </summary>
        /// <param name="customs"></param>
        /// <returns>Return the diagrams as HTML</returns>
        private string Build(IEnumerable<HtmlCustomDiagram> customs)
        {
            var body = new StringBuilder();

            foreach (var custom in customs)
                body.Append(BuildSection(custom));

            return body.ToString();
        }

        private Queue<HtmlResource> LoadBaseResources()
        {
            var resources = new Queue<HtmlResource>();
            resources.Enqueue(new HtmlResource("assets/images/logo-full.svg", HelperBuilder.LoadResource($"{ResourceBasePath}assets.images.logo-full.svg")));
            return resources;
        }


        /// <summary>
        /// Build the documentação e generate a file HTML on specify path
        /// </summary>
        /// <param name="diagrams"></param>
        /// <param name="applicationName"></param>
        /// <param name="path"></param>
        public void BuildDocumentation(string path, string applicationName, params HtmlCustomDiagram[] customs)
        {
            _applicationName = applicationName;
            Directory.CreateDirectory(path);

            var resources = LoadBaseResources();

            var template = HelperBuilder.LoadResourceString($"{ResourceBasePath}documentation.html")
                                            .Replace("##BODY##", Build(customs))
                                            .Replace("##DATE##", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                            .Replace("##MENU##", Menu.ToString())
                                            .Replace("##SCRIPTS##", Scripts.ToString())
                                            .Replace("##APPLICATION_NAME##", _applicationName);

            var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(template));

            HelperBuilder.SaveStream(htmlStream, $"{path}/index.html");

            foreach (var resource in resources)
                HelperBuilder.SaveStream(resource.Stream, $"{path}/{resource.Path}");
        }
    }
}