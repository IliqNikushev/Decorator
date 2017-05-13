using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
    [ExecuteInEditMode]
    public class TestBehaviour : Facade.Unity.ModelBehaviour<Test>
    {
        protected override void OnStart()
        {
            this.Model.FourDepthOnModel = this.Model.GetComponent<Components.ThreeDepth>();
            this.Model.CustomSerializableClassOnModel = new CustomSerializableClass();
            this.Model.ComponentOnModel = this.Model.GetComponent<Components.OneDepth>();
            this.Model.CustomSerializableClassArrayOnModel = new CustomSerializableClass[] { new CustomSerializableClass(), new CustomSerializableClass(), new CustomSerializableClass() };
            this.Model.GameObjectArrayOnModel = new GameObject[5];
            this.Model.GameObjectArrayOnModel[2] = this.gameObject;
            this.Model.ComponentArrayOnModel = this.Model.Components.Select(x=>x as Component).ToArray();
            /*
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            var stream = new System.IO.MemoryStream(1024);
            using (stream)
            {
                this.Model.GetComponent<Location>().X += 1;
                b.Serialize(stream, this.Model.GetComponent<Location>());

                stream.Seek(0, System.IO.SeekOrigin.Begin);
                var d = b.Deserialize(stream) as Location;

                Debug.Log(d.X);
                Debug.Log(this.Model.GetComponent<Location>().X);

                Debug.Log(d.X == this.Model.GetComponent<Location>().X);
            }

            base.OnStart();
            */
        }
    }
}