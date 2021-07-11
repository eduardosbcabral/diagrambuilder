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

            var html = $@"<section class=""docs-section"" id=""{id}"">
                            <h2 class=""section-heading"">{name}</h2>
                            {preDiagram}
                            <div class=""mermaid"">
                                {custom.Diagram.Compile()}
                            </div>
                            {posDiagram}
                        </section>";

            Menu.AppendLine(BuildMenu(id, name));

            return html;
        }

        private string BuildClassDiagram(HtmlClassDiagram classDiagram)
        {
            var html = $@"<h5>{classDiagram.Title}</h5>";

            if (string.IsNullOrWhiteSpace(classDiagram.Description) == false)
                html += $@"<p>{classDiagram.Description}</p>";

            html += $@"<pre id=""{classDiagram.Id}"" class=""docs-code-block""></pre>";

            Scripts.AppendLine(BuildClassJsonScript(classDiagram));

            return html;
        }

        private string BuildMenu(string id, string name)
        {
            return $@"<li class=""nav-item""><a class=""nav-link scrollto"" href=""#{id}"">{name}</a></li>";
        }

        private string BuildClassJsonScript(HtmlClassDiagram classDiagram)
        {
            var serialized = classDiagram.Entity.GetType().GetClassStructure(new SnakeCaseNamingStrategy());
            return $@"$('#{classDiagram.Id}').jsonViewer({serialized}, {{collapsed: true, rootCollapsable: false, withQuotes: true, withLinks: false}});";
        }

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

            resources.Enqueue(new HtmlResource("assets/css/theme.css", HelperBuilder.LoadResource($"{ResourceBasePath}assets.css.theme.css")));
            resources.Enqueue(new HtmlResource("assets/css/jquery.json-viewer.css", HelperBuilder.LoadResource($"{ResourceBasePath}assets.css.jquery.json-viewer.css")));

            resources.Enqueue(new HtmlResource("assets/images/coderdocs-logo.svg", HelperBuilder.LoadResource($"{ResourceBasePath}assets.images.coderdocs-logo.svg")));

            resources.Enqueue(new HtmlResource("assets/js/docs.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.js.docs.js")));
            resources.Enqueue(new HtmlResource("assets/js/jquery.json-viewer.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.js.jquery.json-viewer.js")));
            resources.Enqueue(new HtmlResource("assets/fontawesome/js/all.min.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.fontawesome.js.all.min.js")));
            resources.Enqueue(new HtmlResource("assets/plugins/jquery-3.4.1.min.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.plugins.jquery-3.4.1.min.js")));
            resources.Enqueue(new HtmlResource("assets/plugins/jquery.scrollTo.min.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.plugins.jquery.scrollTo.min.js")));
            resources.Enqueue(new HtmlResource("assets/plugins/popper.min.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.plugins.popper.min.js")));
            resources.Enqueue(new HtmlResource("assets/plugins/bootstrap/js/bootstrap.min.js", HelperBuilder.LoadResource($"{ResourceBasePath}assets.plugins.bootstrap.js.bootstrap.min.js")));

            return resources;
        }

        /// <summary>
        /// Build the documentação e generate a file HTML on specify path
        /// </summary>
        /// <param name="diagrams"></param>
        /// <param name="path"></param>
        public void BuildDocumentation(string path, params IDiagram[] diagrams)
        {
            var resources = LoadBaseResources();

            var template = HelperBuilder.LoadResourceString($"{ResourceBasePath}documentation.html")
                                            .Replace("##BODY##", Build(diagrams))
                                            .Replace("##DATE##", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                            .Replace("##MENU##", Menu.ToString())
                                            .Replace("##SCRIPTS##", Scripts.ToString());

            var htmlStream = new MemoryStream(Encoding.ASCII.GetBytes(template));

            HelperBuilder.SaveStream(htmlStream, $"{path}/index.html");

            foreach (var resource in resources)
                HelperBuilder.SaveStream(resource.Stream, $"{path}/{resource.Path}");
        }

        /// <summary>
        /// Build the documentação e generate a file HTML on specify path
        /// </summary>
        /// <param name="diagrams"></param>
        /// <param name="path"></param>
        public void BuildDocumentation(string path, params HtmlCustomDiagram[] customs)
        {
            Directory.CreateDirectory(path);

            var resources = LoadBaseResources();

            var template = HelperBuilder.LoadResourceString($"{ResourceBasePath}documentation.html")
                                            .Replace("##BODY##", Build(customs))
                                            .Replace("##DATE##", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                                            .Replace("##MENU##", Menu.ToString())
                                            .Replace("##SCRIPTS##", Scripts.ToString());

            var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(template));

            HelperBuilder.SaveStream(htmlStream, $"{path}/index.html");

            foreach (var resource in resources)
                HelperBuilder.SaveStream(resource.Stream, $"{path}/{resource.Path}");
        }
    }
}