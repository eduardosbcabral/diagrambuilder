namespace DiagramBuilder.SequenceDiagrams.Connections
{
    internal class SequenceParallel
    {
        public string Action { get; set; }
        public ParallelOperation Operation { get; set; }

        public SequenceParallel(ParallelOperation operation)
        {
            Operation = operation;
        }

        public SequenceParallel(ParallelOperation operation, string action) : this(operation)
        {
            Action = action;
        }

        public string Compile()
        {
            switch (Operation)
            {
                case ParallelOperation.Begin: return $"par {Action}";
                case ParallelOperation.End: return "end";
                default: return "";
            };
        }
    }
}
