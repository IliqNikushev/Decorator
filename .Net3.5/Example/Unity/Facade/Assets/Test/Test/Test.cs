using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Test
{
    using Components;
    [System.Serializable]
    public class Test : Facade.Unity.Model<Test>
    {
        public Test() { }
        public Test(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public int IntOnModel;
        public string StringOnModel;
        public GameObject GameObjectOnModel;
        public Color ColorOnModel;
        public Component ComponentOnModel;
        public ThreeDepth FourDepthOnModel;
        public CustomSerializableClass CustomSerializableClassOnModel;
        public Component[] ComponentArrayOnModel;
        public CustomSerializableClass[] CustomSerializableClassArrayOnModel;
        public GameObject[] GameObjectArrayOnModel;
    }

    [System.Serializable]
    public abstract class Component : Facade.Unity.Component<Test>
    {
        public Component() { }
        public Component(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}