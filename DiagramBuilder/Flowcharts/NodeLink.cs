namespace DiagramBuilder.Flowcharts
{
    public class NodeLink
    {
        public Node DestinationNode { get; set; }
        public string Text { get; set; }
        public NodeLinkType Type { get; set; }

        public NodeLink(Node node)
        {
            DestinationNode = node;
            Type = NodeLinkType.LineArrow;
        }

        public NodeLink(Node node, string text) : this(node)
        {
            Text = text;
        }

        public NodeLink(Node node, NodeLinkType type) : this(node)
        {
            Type = type;            
        }

        public NodeLink(Node node, string text, NodeLinkType type) : this(node)
        {
            Text = text;
            Type = type;
        }        

        public string Compile(Node originNode)
        {
            var result = $"{originNode.Id} {Type.Compile()} #TEXT# {DestinationNode.Compile()}";

            if (string.IsNullOrWhiteSpace(Text))
                result = result.Replace("#TEXT#", string.Empty);
            else
                result = result.Replace("#TEXT#", $"|{Text}|");

            return result;
        }
    }
}
