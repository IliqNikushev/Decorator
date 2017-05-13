using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Facade.Unity
{
    [System.Serializable]
    public abstract class Component<T> : global::Facade.Component<T> where T : Model<T>
    {
        protected Component() { }
        protected Component(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public virtual void Start() { }
        public virtual void Update() { }
    }
}
