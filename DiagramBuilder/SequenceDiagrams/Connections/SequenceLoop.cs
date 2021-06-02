namespace DiagramBuilder.SequenceDiagrams.Connections
{
    internal class SequenceLoop
    {
        public string Loop { get; set; }
        public LoopOperation Operation { get; set; }

        public SequenceLoop(LoopOperation operation)
        {
            Operation = operation;
        }

        public SequenceLoop(LoopOperation operation, string loop) : this(operation)
        {
            Loop = loop;
        }

        public string Compile()
        {
            switch (Operation)
            {
                case LoopOperation.Begin: return $"loop {Loop}";
                case LoopOperation.End: return "end";
                default: return "";
            };
        }
    }
}
