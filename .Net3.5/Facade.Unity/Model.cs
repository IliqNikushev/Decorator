using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Facade.Unity
{
    [System.Serializable]
    public abstract class Model<T> : global::Facade.Model<T> where T : Model<T>
    {
        public new Component<T>[] Components { get { return base.Components.Cast<Component<T>>().ToArray(); } }

        public Model() { }
        public Model(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [NonSerialized]
        public GameObject GameObject;
    }
}
