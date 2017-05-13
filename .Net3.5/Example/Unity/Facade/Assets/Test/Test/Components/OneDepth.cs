using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Test.Components
{
    [System.Serializable]
    public class OneDepth : Component
    {
        public OneDepth() { }
        public OneDepth(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int Age;
        public string Health = "good";
        public int[] ArrayField = new int[] { 1, 2, 3 };
        public CustomSerializableClass NullField;
    }
}
