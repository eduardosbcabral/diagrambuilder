namespace DiagramBuilder.SequenceDiagrams
{
    internal static class SequenceDiagramExtensions
    {
        public static ActorLink BuildReply(this ActorLink link, string message)
        {
            return new ActorLink(link.DestinationActor, link.OriginActor, message, string.Empty, ActorLinkType.DottedLineArrow);
        }

        /// <summary>
        /// Define the link type
        /// </summary>
        /// <param name="linkType"></param>
        /// <returns>Return the link type</returns>
        public static string Compile(this ActorLinkType linkType)
        {
            switch (linkType)
            {
                case ActorLinkType.Line: return "->";
                case ActorLinkType.LineArrow: return "->>";
                case ActorLinkType.LineOpenArrow: return "-)";
                case ActorLinkType.LineCross: return "-x";
                case ActorLinkType.DottedLine: return "-->";
                case ActorLinkType.DottedLineArrow: return "-->>";
                case ActorLinkType.DottedLineOpenArrow: return "--)";
                case ActorLinkType.DottedLineCross: return "--x";
                default: return "";
            };
        }
    }
}
