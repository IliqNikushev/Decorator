using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace Facade.Test
{
    [TestClass]
    public class Serialization : SerializationTestBase
    {
        [TestMethod]
        public void CantSerializeWhenNonSerializableObjectFound()
        {
            try
            {
                Serialize(new NonSerializableModel());
                Assert.Fail("Must not serialize");
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains(typeof(NonSerializableModel).FullName) && ex.Message.Contains("is not marked as serializable"), ex.Message);
            }
            Clear();

            try
            {
                Serialize(new NonSerializableComponent());
                Assert.Fail("Must not serialize");
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains(typeof(NonSerializableComponent).FullName) && ex.Message.Contains("is not marked as serializable"), ex.Message);
            }
            Clear();

            try
            {
                Serialize(new TestModel());
                Assert.Fail("Must not serialize");
            }
            catch(System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains(typeof(NonSerializableClass).FullName) && ex.Message.Contains("is not marked as serializable"), ex.Message);
            }
            Clear();

            try
            {
                Serialize(new TestComponent());
                Assert.Fail("Must not serialize");
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains(typeof(NonSerializableClass).FullName) && ex.Message.Contains("is not marked as serializable"), ex.Message);
            }
        }

        [TestMethod]
        public void CantSerializeWhenNoConstruct()
        {
            var model = new NoConstructorModel();
            Serialize(model);
            try
            {
                Deserialize();
                Assert.Fail("Must not deserialize");
            }
            catch(System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The constructor to deserialize an object of type") && ex.Message.Contains(model.GetType().FullName), ex.Message);
            }
            Clear();

            var component = new NoConstructorComponent();
            Serialize(component);
            try
            {
                Deserialize();
                Assert.Fail("Must not deserialize");
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The constructor to deserialize an object of type") && ex.Message.Contains(component.GetType().FullName), ex.Message);
            }
        }

        [TestMethod]
        public void CanSerializeWhenWithConstructor()
        {
            var model = new OkModel();
            model.ModelField = 3;
            Serialize(model);
            var deserializedModel = Deserialize() as OkModel;

            Assert.AreEqual(model.ModelField, deserializedModel.ModelField, "Must have same value after deserialization of model");

            var component = new OkComponent();
            component.ComponentField = 3;
            Serialize(component);
            var deserializedComponent = Deserialize() as OkComponent;

            Assert.AreEqual(component.ComponentField, deserializedComponent.ComponentField, "Must have same value after deserialization of component");

            model.GetComponent<OkComponent>().ComponentField = 3;

            Serialize(model);
            deserializedModel = Deserialize() as OkModel;

            Assert.AreEqual(model.GetComponent<OkComponent>().ComponentField, deserializedModel.GetComponent<OkComponent>().ComponentField, "Component Must have same value after deserialization of model");
        }

        class NonSerializableComponent : Facade.Model<NonSerializableModel> { }
        class NonSerializableModel : Facade.Model<NonSerializableModel> { }

        [System.Serializable]
        class TestModel : Facade.Model<TestModel>
        {
            public NonSerializableClass problem = new NonSerializableClass();
        }

        class NonSerializableClass { }

        [System.Serializable]
        class TestComponent : Facade.Component<TestModel>
        {
            public NonSerializableClass problem = new NonSerializableClass();
        }

        [Serializable]
        class NoConstructorComponent : Facade.Model<NoConstructorComponent> { }
        [Serializable]
        class NoConstructorModel : Facade.Model<NoConstructorModel> { }

        [System.Serializable]
        class OkModel : Model<OkModel>
        {
            public OkModel() { }
            public OkModel(SerializationInfo info, StreamingContext context) : base(info, context) { }
            public int ModelField;
        }

        [System.Serializable]
        class OkComponent : Component<OkModel>
        {
            public OkComponent() { }
            public OkComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
            public int ComponentField;
        }
    }
}
