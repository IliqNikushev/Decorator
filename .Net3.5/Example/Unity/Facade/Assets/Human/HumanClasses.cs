using System.Runtime.Serialization;

namespace Human
{
    [System.Serializable]
    public class Human : Facade.Unity.Model<Human>
    {
        public Human() { }
        public Human(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int Age;
    }

    [System.Serializable]
    public abstract class Component : Facade.Unity.Component<Human>
    {
        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [System.Serializable]
    public class Location : Component
    {
        public Location() { }
        public Location(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int X;
        public int Y;
    }

    [System.Serializable]
    public class Stats : Component
    {
        public Stats() { }
        public Stats(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int Age;
        public int Health;
    }
}