using System.Linq;
using System.Collections.Generic;
using System;

namespace Facade
{
    using Extentions.Type;
    using System.Runtime.Serialization;

    [System.Serializable]
    public abstract class Model : Serializable   
    {
        [NonSerialized]
        internal List<Component> components = new List<Component>();
        public Component[] GetComponents { get { return components.ToArray(); } }

        static protected internal Dictionary<System.Type, List<object>> classComponents = new Dictionary<System.Type, List<object>>();
        static protected internal Dictionary<System.Type, int> instances = new Dictionary<System.Type, int>();

        private static HashSet<string> loadedAssemblies = new HashSet<string>();

        static Model()
        {
            foreach (var item in System.AppDomain.CurrentDomain.GetAssemblies())
                LoadComponentsFromAssembly(item);
        }

        internal Model() { }
        protected Model(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void LoadComponentsFromAssembly(System.Reflection.Assembly item)
        {
            if (loadedAssemblies.Contains(item.FullName))
                return;

            loadedAssemblies.Add(item.FullName);
            try
            {
                foreach (var type in item.GetTypes())
                {
                    if (type.IsSubclassOfGeneric(typeof(Component<>)))
                    {
                        if (type.IsAbstract) continue;

                        System.Type t = type;
                        while (t.BaseType.Name != typeof(Component<>).Name)
                            t = t.BaseType;
                        t = t.BaseType.GetGenericArguments()[0];

                        if (instances.ContainsKey(t))
                            throw new Exception("Adding components can happen when there are no instances created of a class, " + t.FullNormalName());

                        Log.Info("Adding component to " + t.FullNormalName() + " " + type.FullNormalName());

                        var instance = System.Activator.CreateInstance(type, null);
                        if (!classComponents.ContainsKey(t))
                            classComponents.Add(t, new List<object>());

                        classComponents[t].Add(instance);
                    }
                }
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                Log.LogWarning("Unable to load " + item.FullName + "\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public Component GetComponent(System.Type type)
        {
            return GetComponents.FirstOrDefault(x => x.GetType() == type);
        }
    }

    [System.Serializable]
    public abstract class Model<T> : Model where T : Model<T>
    {
        [NonSerialized]
        private Dictionary<System.Type, Component<T>> genericComponents = new Dictionary<System.Type, Component<T>>();

        protected Component<T>[] Components
        {
            get
            {
                return genericComponents.Values.ToArray();
            }
        }

        internal Model() { }
        public Model(params object[] args) { }
        protected Model(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static T CreateInstance(params object[] args)
        {
            T instance = Activator.CreateInstance(typeof(T), args) as T;

            instance.LoadComponents();

            return instance;
        }

        private void LoadComponents()
        {
            var type = this.GetType();

            if (!instances.ContainsKey(type))
                instances.Add(type, 0);
            instances[type] += 1;

            while (type != typeof(object))
            {
                if (classComponents.ContainsKey(type))
                {
                    foreach (var item in classComponents[type])
                    {
                        Component<T> instance = (item as Component<T>).CreateInstance((T)this);
                        this.genericComponents.Add(item.GetType(), instance);
                        components.Add(instance as Component);
                    }
                }

                type = type.BaseType;
            }
        }

        public D GetComponent<D>() where D : Component<T>
        {
            if (this.genericComponents.ContainsKey(typeof(D)))
                return (D)this.genericComponents[typeof(D)];

            return null;
        }

        public static D ClassComponents<D>() where D : Component<T>
        {
            var type = typeof(T);
            if (classComponents.ContainsKey(type))
            {
                if (instances.ContainsKey(type))
                {
                    throw new Exception("Decoration can happen before an instane is made, " + typeof(T).FullNormalName() + ">" + typeof(D).FullNormalName());
                }
                else
                {
                    var component = classComponents[type].FirstOrDefault(x => x.GetType() == typeof(D));
                    if (component != null)
                        return component as D;
                }
            }

            throw new Exception("No Component found, " + typeof(D).FullNormalName() + " for " + typeof(T).FullNormalName());
        }
    }
}