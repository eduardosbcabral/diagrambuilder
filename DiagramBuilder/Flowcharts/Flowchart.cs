using System;
using System.Collections.Generic;
using System.Text;

namespace DiagramBuilder.Flowcharts
{
    public class Flowchart : IDiagram
    {
        public string Name { get; set; }
        public FlowchartOrientation Orientation { get; set; }
        private readonly IList<Node> Nodes;
        private readonly IList<(Node Node, NodeLink Link)> Connections;

        private Node LastNode;        

        public Flowchart(string name)
        {
            Name = name;
            Orientation = FlowchartOrientation.TopToBottom;
            Nodes = new List<Node>();
            Connections = new List<(Node, NodeLink)>();
        }

        public void SetName(string name) => Name = name;

        public Flowchart(string name, FlowchartOrientation orientation) : this(name)
        {
            Orientation = orientation;
        }

        public Flowchart Connect(Node node)
        {
            AddNode(node);
            return this;
        }

        public Flowchart With(NodeLink link)
        {
            CheckLink();

            Connections.Add((LastNode, link));
            return this;
        }

        public Flowchart With(Node node) => With(new NodeLink(node));

        public Flowchart With(Node node, string text) => With(new NodeLink(node, text));

        public Flowchart With(Node node, NodeLinkType linkType) => With(new NodeLink(node, linkType));

        public Flowchart With(Node node, string text, NodeLinkType linkType) => With(new NodeLink(node, text, linkType));

        private void AddNode(Node node)
        {
            LastNode = node;
            Nodes.Add(node);
        }

        private void CheckLink()
        {
            if (LastNode == null)
                throw new Exception("It's necessary some node for the link");
        }

        public string Compile()
        {
            var builder = new StringBuilder();

            builder.AppendLine(Orientation.Compile());

            foreach (var node in Nodes)
                builder.AppendLine(node.Compile());

            foreach (var (Node, Link) in Connections)
                builder.AppendLine(Link.Compile(Node));

            return builder.ToString();
        }

        public string Title()
        {
            return Name;
        }
    }
}
