using System.Drawing;

namespace DiagramBuilder.SequenceDiagrams.Connections
{
    internal class SequenceHighlighting
    {
        public Color Cor { get; set; }
        public HighlightingOperation Operation { get; set; }

        public SequenceHighlighting(HighlightingOperation operation)
        {
            Operation = operation;
        }

        public SequenceHighlighting(Color cor, HighlightingOperation operation) : this(operation)
        {
            Cor = cor;
        }

        public string Compile()
        {
            switch (Operation)
            {
                case HighlightingOperation.Begin: return $"rect rgba({Cor.R},{Cor.G},{Cor.B}, {Cor.A})";
                case HighlightingOperation.End: return "end";
                default: return "";
            };
        }
    }
}
