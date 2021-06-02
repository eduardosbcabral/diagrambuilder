namespace DiagramBuilder.Flowcharts
{
    public static class FlowchartExtensions
    {
        /// <summary>
        /// Define the flowchart orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns>Return the graph orientation</returns>
        public static string Compile(this FlowchartOrientation orientation)
        {
            switch (orientation)
            {
                case FlowchartOrientation.BottomToTop: return "graph BT";
                case FlowchartOrientation.TopToBottom: return "graph TB";
                case FlowchartOrientation.RightToLeft: return "graph RL";
                case FlowchartOrientation.LeftToRight: return "graph LR";
                default: return "";
            };
        }

        /// <summary>
        /// Define the node shapes
        /// </summary>
        /// <param name="nodeShapes"></param>
        /// <returns>Return struct of node based on shape</returns>
        public static string Compile(this NodeShapes nodeShapes)
        {
            switch (nodeShapes)
            {
                case NodeShapes.Default: return "#ID#[#TEXT#]";
                case NodeShapes.RoundEdges: return "#ID#(#TEXT#)";
                case NodeShapes.StadiumShaped: return "#ID#([#TEXT#])";
                case NodeShapes.RectangularWithBorder: return "#ID#[[#TEXT#]]";
                case NodeShapes.Cylindrical: return "#ID#[(#TEXT#)]";
                case NodeShapes.Circle: return "#ID#((TEXT#))";
                case NodeShapes.Asymmetric: return "#ID#>#TEXT#]";
                case NodeShapes.Rhombus: return "#ID#{#TEXT#}";
                case NodeShapes.Hexagon: return "#ID#{{#TEXT#}}";
                case NodeShapes.Parallelogram: return "#ID#[/#TEXT#/]";
                case NodeShapes.ParallelogramAlt: return @"#ID#[\#TEXT#\]";
                case NodeShapes.Trapezoid: return @"#ID#[/#TEXT#\]";
                case NodeShapes.TrapezoidAlt: return @"#ID#[\#TEXT#/]";
                default: return "";
            };
        }

        /// <summary>
        /// Define the link type
        /// </summary>
        /// <param name="linkType"></param>
        /// <returns>Return the link type</returns>
        public static string Compile(this NodeLinkType linkType)
        {
            switch (linkType)
            {
                case NodeLinkType.Line: return "---";
                case NodeLinkType.LineArrow: return "-->";
                case NodeLinkType.DottedLine: return "-.-";
                case NodeLinkType.DottedLineArrow: return "-.->";
                case NodeLinkType.ThickLine: return "===";
                case NodeLinkType.ThickLineArrow: return "==>";
                default: return "";
            };
        }
    }
}
