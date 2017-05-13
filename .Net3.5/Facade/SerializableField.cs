using System.Linq;
using System.Collections.Generic;
using System;

namespace Facade
{
    public class SerializableField
    {
        private object Target;
        private System.Reflection.FieldInfo info;
        public string Name { get { return this.info.Name; } }
        public Type Type { get { return info.FieldType; } }
        /// <exception cref="InvalidCastException">When the value is not the correct type</exception>
        public object Value { get { return this.info.GetValue(this.Target); } set { info.SetValue(this.Target, value); } }

        public bool IsMarkedForSerialization { get { return !info.GetCustomAttributes(true).Any(x => x.GetType() == typeof(System.NonSerializedAttribute)); } }

        public SerializableField(object target, System.Reflection.FieldInfo info)
        {
            this.Target = target;
            this.info = info;
        }
    }
}