namespace DiagramBuilder.SequenceDiagrams.Connections
{
    internal class SequenceAlt
    {
        public string Condition { get; set; }
        public AltOperation Operation { get; set; }

        public SequenceAlt(AltOperation operation)
        {
            Operation = operation;
        }

        public SequenceAlt(AltOperation operation, string condition) : this(operation)
        {
            Condition = condition;
        }

        public string Compile()
        {
            switch (Operation)
            {
                case AltOperation.Begin: return $"alt {Condition}";
                case AltOperation.Else: return $"else {Condition}";
                case AltOperation.End: return "end";
                default: return "";
            };
        }
    }
}
