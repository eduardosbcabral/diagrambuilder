using System;
using System.Collections.Generic;
using System.Text;

namespace DiagramBuilder.SequenceDiagrams
{
    internal class ActorLink
    {
        public Actor OriginActor { get; set; }
        public Actor DestinationActor { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public ActorLinkType Type { get; set; }

        public ActorLink(Actor originActor, Actor destinationActor, string message)
        {
            OriginActor = originActor;
            DestinationActor = destinationActor;
            Message = message;
            Type = ActorLinkType.LineArrow;
        }

        public ActorLink(Actor originActor, Actor destinationActor, string message, string note) : this(originActor, destinationActor, message)
        {
            Note = note;
        }

        public ActorLink(Actor originActor, Actor destinationActor, string message, ActorLinkType type) : this(originActor, destinationActor, message)
        {
            Type = type;
        }

        public ActorLink(Actor originActor, Actor destinationActor, string message, string note, ActorLinkType type) : this(originActor, destinationActor, message)
        {
            Note = note;
            Type = type;
        }

        public string Compile(ActorLinkDirection direction = ActorLinkDirection.Send)
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Note) == false)
                builder.AppendLine($"Note over {OriginActor.Id}, {DestinationActor.Id}: {Note}");

            if (direction == ActorLinkDirection.Reply)
                builder.AppendLine($"activate {OriginActor.Id}");

            builder.AppendLine($"{OriginActor.Id} {Type.Compile()} {DestinationActor.Id}: {Message}");

            if (direction == ActorLinkDirection.Reply)
                builder.AppendLine($"deactivate {OriginActor.Id}");

            return builder.ToString().Trim();
        }

    }
}
