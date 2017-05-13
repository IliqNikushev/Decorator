using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Test.Components
{
    [System.Serializable]
    public class ThreeDepth : TwoDepth
    {
        public ThreeDepth() { }
        public ThreeDepth(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public TwoDepth TwoDepth = new TwoDepth();
    }
}
