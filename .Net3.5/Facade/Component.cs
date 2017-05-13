using System;
using System.Runtime.Serialization;

namespace Facade
{
    [System.Serializable]
    public abstract class Component : Serializable
    {
        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected virtual void Init() { }

        public static implicit operator bool(Component component)
        {
            return component == null;
        }
    }

    [System.Serializable]
    public abstract class Component<T> : Component where T : Model<T>
    {
        private bool isInstance { get { return Model != null; } }
        protected internal T Model;

        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected internal Component<T> CreateInstance(T Model)
        {
            var instance = System.Activator.CreateInstance(this.GetType()) as Component<T>;
            instance.Model = Model;
            instance.Init();

            return instance;
        }
    }
}