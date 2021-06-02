namespace DiagramBuilder.SequenceDiagrams.Connections
{
    internal class Connection
    {
        public ConnectionType Type { get; set; }

        public ActorLink ActorLink { get; set; }
        public ActorLinkDirection ActorLinkDirection { get; set; }

        public SequenceLoop Loop { get; set; }
        public SequenceAlt Alt { get; set; }
        public SequenceParallel Parallel { get; set; }
        public SequenceHighlighting Highlighting { get; set; }

        public Connection(ActorLink actorLink, ActorLinkDirection direction)
        {
            ActorLinkDirection = direction;
            ActorLink = actorLink;
            Type = ConnectionType.Default;
        }

        public Connection(SequenceLoop loop)
        {
            Loop = loop;
            Type = ConnectionType.Loop;
        }

        public Connection(SequenceAlt alt)
        {
            Alt = alt;
            Type = ConnectionType.AlternativePath;
        }

        public Connection(SequenceParallel parallel)
        {
            Parallel = parallel;
            Type = ConnectionType.Parallel;
        }

        public Connection(SequenceHighlighting highlighting)
        {
            Highlighting = highlighting;
            Type = ConnectionType.Highlighting;
        }

        public string Compile()
        {
            switch (Type)
            {
                case ConnectionType.Default: return ActorLink.Compile(ActorLinkDirection);
                case ConnectionType.AlternativePath: return Alt.Compile();
                case ConnectionType.Loop: return Loop.Compile();
                case ConnectionType.Parallel: return Parallel.Compile();
                case ConnectionType.Highlighting: return Highlighting.Compile();
                default: return "";
            };
        }
    }
}
