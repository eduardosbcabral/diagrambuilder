using System;

namespace DiagramBuilder.Flowcharts
{
    public class Node
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public NodeShapes Shape { get; set; }

        public Node(string text)
        {
            Id = $"{Guid.NewGuid()}".Replace("-", "");
            Text = text;
            Shape = NodeShapes.Default;
        }

        public Node(string text, NodeShapes shape) : this(text)
        {
            Shape = shape;
        }

        public string Compile()
        {
            var shape = Shape.Compile();
            return shape.Replace("#ID#", Id).Replace("#TEXT#", Text);
        }
    }
}
