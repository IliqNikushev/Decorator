using System.Linq;
using System.Collections.Generic;
using System;

namespace Facade
{
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
            foreach (var item in this.AllFields)
                if (item.IsMarkedForSerialization)
                    info.AddValue(item.Name, item.Value);
        }

        protected virtual void OnDeserialization(SerializationInfo info, StreamingContext context)
        {
            foreach (var item in this.AllFields)
                if (item.IsMarkedForSerialization)
                    item.Value = info.GetValue(item.Name, item.Type);
        }

        private SerializableField[] GetAllFields(System.Reflection.BindingFlags flags)
        {
            List<System.Reflection.FieldInfo> infos = new List<System.Reflection.FieldInfo>();

            Type t = this.GetType();

            while (t != null)
            {
                infos.AddRange(t.GetFields(flags).Where(x => x.DeclaringType == t));

                t = t.BaseType;
            }

            return infos.Select(x => new SerializableField(this, x)).ToArray();
        }

        public SerializableField[] PublicFields
        {
            get
            {
                return GetAllFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            }
        }

        protected SerializableField[] AllFields
        {
            get
            {

                return GetAllFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            }
        }
    }
}