using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade.Unity
{
    [System.Serializable]
    public abstract class ModelBehaviour<T> : UnityEngine.MonoBehaviour where T: Model<T>
    {
        [NonSerialized]
        public T Model;
        public ModelBehaviour()
        {
            this.Model = Model<T>.CreateInstance(this.gameObject);
        }
    }
}
