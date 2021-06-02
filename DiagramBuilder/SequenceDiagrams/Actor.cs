using System;

namespace DiagramBuilder.SequenceDiagrams
{
    public class Actor
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Actor(string name)
        {
            Id = $"{Guid.NewGuid()}".Replace("-", "");
            Name = name;
        }

        public string Compile()
        {
            return $"participant {Id} as {Name}";
        }
    }
}
