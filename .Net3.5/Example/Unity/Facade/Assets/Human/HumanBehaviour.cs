using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Human
{
    [ExecuteInEditMode]
    public class HumanBehaviour : Facade.Unity.ModelBehaviour<Human>
    {
        protected override void OnStart()
        {
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
        }
    }
}