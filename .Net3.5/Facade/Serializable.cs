using System.Linq;
using System.Collections.Generic;
using System;

namespace Facade
{
    using Extentions.Type;
    using System.Runtime.Serialization;

    [System.Serializable]
    public abstract class Serializable : ISerializable   
    {
        internal Serializable() { }

        protected Serializable(SerializationInfo info, StreamingContext context)
        {
            OnDeserialization(info, context);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            OnSerialization(info, context);
        }

        protected virtual void OnSerialization(SerializationInfo info, StreamingContext context)
        {
        }

        protected virtual void OnDeserialization(SerializationInfo info, StreamingContext context)
        {
        }
    }
}