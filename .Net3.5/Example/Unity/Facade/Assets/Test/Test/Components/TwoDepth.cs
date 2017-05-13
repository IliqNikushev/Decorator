using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Test.Components
{
    [System.Serializable]
    public class TwoDepth : OneDepth
    {
        public TwoDepth() { }
        public TwoDepth(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public OneDepth OneDepth = new OneDepth();
    }
}
