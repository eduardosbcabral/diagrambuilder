using DiagramBuilder.Helper;
using System.Collections.Generic;

namespace DiagramBuilder.Html
{
    public class HtmlCustomDiagram
    {
        public HtmlCustomDiagram(IDiagram diagram)
        {
            Diagram = diagram;
            ClassesPreDiagram = new List<HtmlClassDiagram>();
            ClassesPosDiagram = new List<HtmlClassDiagram>();
        }

        public HtmlCustomDiagram(IDiagram diagram, List<HtmlClassDiagram> classesPreDiagram) : this(diagram)
        {
            ClassesPreDiagram = classesPreDiagram;
        }

        public HtmlCustomDiagram(IDiagram diagram, List<HtmlClassDiagram> classesPreDiagram, List<HtmlClassDiagram> classesPosDiagram) : this(diagram, classesPreDiagram)
        {
            ClassesPosDiagram = classesPosDiagram;
        }

        public IDiagram Diagram { get; private set; }
        public List<HtmlClassDiagram> ClassesPreDiagram { get; private set; }
        public List<HtmlClassDiagram> ClassesPosDiagram { get; private set; }

        public void AddPreClassDiagram(HtmlClassDiagram diagram) => ClassesPreDiagram.Add(diagram);
        public void AddPreClassDiagram(string title, object entitie) => ClassesPreDiagram.Add(new HtmlClassDiagram(title, entitie));
        public void AddPreClassDiagram(string title, object entitie, string description) => ClassesPreDiagram.Add(new HtmlClassDiagram(title, entitie, description));

        public void AddPosClassDiagram(HtmlClassDiagram diagram) => ClassesPosDiagram.Add(diagram);
        public void AddPosClassDiagram(string title, object entitie) => ClassesPosDiagram.Add(new HtmlClassDiagram(title, entitie));
        public void AddPosClassDiagram(string title, object entitie, string description) => ClassesPosDiagram.Add(new HtmlClassDiagram(title, entitie, description));
    }

    public class HtmlClassDiagram
    {
        public HtmlClassDiagram(string title, object entitie)
        {
            Id = UniqueIdentifier.Create("json_");
            Title = title;
            Entitie = entitie;
        }

        public HtmlClassDiagram(string title, object entitie, string description) : this(title, entitie)
        {
            Description = description;
        }

        public string Id { get; private set; }
        public string Title { get; private set; }
        public object Entitie { get; private set; }
        public string Description { get; private set; }
    }
}