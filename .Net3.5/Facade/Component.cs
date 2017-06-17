using System;
using System.Runtime.Serialization;

namespace Facade
{
    [System.Serializable]
    public abstract class Component : Serializable
    {
        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }
        internal abstract void AssignModel(object model);
    }

    [System.Serializable]
    public abstract class Component<T> : Component where T : Model<T>
    {
        private bool isInstance { get { return Model != null; } }
        [NonSerialized]
        protected internal T Model;

        public void AssignModel(T model)
        {
            AssignModel(model as object);
        }

        internal override void AssignModel(object model)
        {
            if (!(model is T))
                throw new InvalidOperationException("Cannot assign model specified to component");

            this.Model = model as T;
        }

        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected internal Component<T> CreateCloneFor(T model)
        {
            var instance = System.Activator.CreateInstance(this.GetType()) as Component<T>;
            instance.AssignModel(model);

            return instance;
        }
    }
}