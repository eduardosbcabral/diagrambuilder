using DiagramBuilder.SequenceDiagrams.Connections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DiagramBuilder.SequenceDiagrams
{
    internal class SequenceDiagram : IDiagram
    {
        public string Name { get; set; }
        public bool Autonumber { get; set; }
        private readonly IList<Actor> Actors;
        private readonly Queue<Connection> Connections;

        private ActorLink LastLink;

        public SequenceDiagram(string name)
        {
            Name = name;
            Autonumber = false;
            Actors = new List<Actor>();
            Connections = new Queue<Connection>();
        }

        public SequenceDiagram(string name, bool autoNumber) : this(name)
        {
            Autonumber = autoNumber;
        }

        public SequenceDiagram AddActor(Actor actor)
        {
            Actors.Add(actor);
            return this;
        }

        public SequenceDiagram AddActors(params Actor[] actors)
        {
            foreach (var actor in actors)
                Actors.Add(actor);

            return this;
        }

        public SequenceDiagram Send(ActorLink link)
        {
            LastLink = link;
            Connections.Enqueue(new Connection(link, ActorLinkDirection.Send));
            return this;
        }

        public SequenceDiagram Send(Actor origin, Actor destination, string message)
        {
            return Send(new ActorLink(origin, destination, message));
        }

        public SequenceDiagram Send(Actor origin, Actor destination, string message, ActorLinkType typeLink)
        {
            return Send(new ActorLink(origin, destination, message, typeLink));
        }

        public SequenceDiagram Send(Actor origin, Actor destination, string message, string note)
        {
            return Send(new ActorLink(origin, destination, message, note));
        }

        public SequenceDiagram Send(Actor origin, Actor destination, string message, string note, ActorLinkType typeLink)
        {
            return Send(new ActorLink(origin, destination, message, note, typeLink));
        }

        public SequenceDiagram Reply(string message)
        {
            CheckLink();

            Connections.Enqueue(new Connection(LastLink.BuildReply(message), ActorLinkDirection.Reply));
            return this;
        }

        public SequenceDiagram HighlightingBegin(Color color)
        {
            Connections.Enqueue(new Connection(new SequenceHighlighting(color, HighlightingOperation.Begin)));
            return this;
        }

        public SequenceDiagram HighlightingBegin(int red, int green, int blue, int alpha)
        {            
            return HighlightingBegin(Color.FromArgb(alpha, red, green, blue));
        }

        public SequenceDiagram HighlightingEnd()
        {
            Connections.Enqueue(new Connection(new SequenceHighlighting(HighlightingOperation.End)));
            return this;
        }

        public SequenceDiagram LoopBegin(string loop = null)
        {
            Connections.Enqueue(new Connection(new SequenceLoop(LoopOperation.Begin, loop)));
            return this;
        }

        public SequenceDiagram LoopEnd()
        {
            Connections.Enqueue(new Connection(new SequenceLoop(LoopOperation.End)));
            return this;
        }

        public SequenceDiagram ParallelBegin(string action = null)
        {
            Connections.Enqueue(new Connection(new SequenceParallel(ParallelOperation.Begin, action)));
            return this;
        }

        public SequenceDiagram ParallelEnd()
        {
            Connections.Enqueue(new Connection(new SequenceParallel(ParallelOperation.End)));
            return this;
        }

        public SequenceDiagram AlternativeBegin(string condition = null)
        {
            Connections.Enqueue(new Connection(new SequenceAlt(AltOperation.Begin, condition)));
            return this;
        }

        public SequenceDiagram AlternativeElse(string condition = null)
        {
            Connections.Enqueue(new Connection(new SequenceAlt(AltOperation.Else, condition)));
            return this;
        }

        public SequenceDiagram AlternativeEnd(string condition = null)
        {
            Connections.Enqueue(new Connection(new SequenceAlt(AltOperation.End, condition)));
            return this;
        }

        private void CheckLink()
        {
            if (LastLink == null)
                throw new Exception("It's necessary some message sent for the reply");
        }

        private bool LoopsMatch()
        {
            var loops = Connections.Where(c => c.Loop != null).Select(c => c.Loop);
            return loops.Count(l => l.Operation == LoopOperation.Begin) == loops.Count(l => l.Operation == LoopOperation.End);
        }

        private bool AlternativesMatch()
        {
            var alts = Connections.Where(c => c.Alt != null).Select(c => c.Alt);
            return alts.Count(l => l.Operation == AltOperation.Begin) == alts.Count(l => l.Operation == AltOperation.End);
        }

        private bool ParallelsMatch()
        {
            var parallels = Connections.Where(c => c.Parallel != null).Select(c => c.Parallel);
            return parallels.Count(l => l.Operation == ParallelOperation.Begin) == parallels.Count(l => l.Operation == ParallelOperation.End);
        }

        private bool HighlightingMatch()
        {
            var highlightings = Connections.Where(c => c.Highlighting != null).Select(c => c.Highlighting);
            return highlightings.Count(l => l.Operation == HighlightingOperation.Begin) == highlightings.Count(l => l.Operation == HighlightingOperation.End);
        }

        public string Compile()
        {
            var builder = new StringBuilder();

            if (LoopsMatch() == false)
                throw new Exception("Some loop hasn't been endeed");

            if (AlternativesMatch() == false)
                throw new Exception("Some alternative hasn't been endeed");

            if (ParallelsMatch() == false)
                throw new Exception("Some parallel hasn't been endeed");

            if (HighlightingMatch() == false)
                throw new Exception("Some highlighting hasn't been endeed");

            builder.AppendLine("sequenceDiagram");

            if (Autonumber)
                builder.AppendLine("autonumber");

            foreach (var actor in Actors)
                builder.AppendLine(actor.Compile());

            foreach (var connection in Connections)
                builder.AppendLine(connection.Compile());

            return builder.ToString();
        }

        public string Title()
        {
            return Name;
        }
    }
}
