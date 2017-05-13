using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade.Unity
{
    [System.Serializable]
    [UnityEngine.ExecuteInEditMode]
    public abstract class ModelBehaviour : UnityEngine.MonoBehaviour
    {
        [NonSerialized]
        public Model Model;

        protected virtual void Start() { }
        protected virtual void Update() { }

        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
    }

    [System.Serializable]
    [UnityEngine.ExecuteInEditMode]
    public abstract class ModelBehaviour<T> : ModelBehaviour where T: Model<T>
    {
        public new T Model { get { return base.Model as T; } set { base.Model = value; } }

        public ModelBehaviour()
        {
            this.Model = Model<T>.CreateInstance();
        }

        protected override sealed void Start()
        {
            this.Model.GameObject = this.gameObject;
            foreach (var item in this.Model.Components)
                item.Start();
            OnStart();
        }

        protected override sealed void Update()
        {
            foreach (var item in this.Model.Components)
                item.Update();
            
            OnUpdate();
        }
    }
}
