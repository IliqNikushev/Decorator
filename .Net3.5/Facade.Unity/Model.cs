using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Facade.Unity
{
    public abstract class Model<T> : global::Facade.Model<T> where T : global::Facade.Model<T>
    {
        [NonSerialized]
        private GameObject gameObject;
        public GameObject GameObject { get { return gameObject; } }

        public Model(GameObject gameObject = null)
        {
            this.gameObject = gameObject;
        }
    }
}
