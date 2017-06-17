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
        public static event Action<string> OnComponentTypeNotFoundDuringDeserialization = (x) => { };
        public static event Action<System.Type> OnComponentNotFoundDuringDeserialization = (x) => { };

        [NonSerialized]
        internal List<Component> components = new List<Component>();
        public Component[] Components { get { return components.ToArray(); } }

        static protected internal Dictionary<System.Type, List<object>> classComponents = new Dictionary<System.Type, List<object>>();
        static protected internal Dictionary<System.Type, int> instances = new Dictionary<System.Type, int>();
        private static HashSet<string> loadedAssemblies = new HashSet<string>();

        static Model()
        {
            foreach (var item in System.AppDomain.CurrentDomain.GetAssemblies())
                LoadComponentsFromAssembly(item);
        }

        public Model() { LoadComponents(); }
        public Model(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void LoadComponentsFromAssembly(System.Reflection.Assembly item)
        {
            if (loadedAssemblies.Contains(item.FullName))
                return;

            System.AppDomain.CurrentDomain.TypeResolve += (x, y) =>
            {
                if (item.GetType(y.Name, false) != null)
                    return item;

                return null;
            };

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
            return Components.FirstOrDefault(x => x.GetType() == type);
        }

        protected override void OnSerialization(SerializationInfo info, StreamingContext context)
        {
            base.OnSerialization(info, context);

            string[] componentTypes = this.Components.Select(x => x.GetType().FullName).ToArray();
            info.AddValue("ComponentTypes", componentTypes); 

            foreach (var component in this.Components)
                info.AddValue(this.GetType().FullName + "." + component.GetType().FullName, (object)component);
        }

        protected override void OnDeserialization(SerializationInfo info, StreamingContext context)
        {
            base.OnDeserialization(info, context);
            LoadComponents();

            string[] componentTypes = info.GetValue("ComponentTypes", typeof(string[])) as string[];

            foreach (var type in componentTypes)
            {
                // find registered

                System.Type found = System.Type.GetType(type);
                if (found == null)
                {
                    OnComponentTypeNotFoundDuringDeserialization(type);
                    try
                    {
                        object o = info.GetValue(this.GetType().FullName + "." + type, typeof(Exception));
                    }
                    catch { }
                    continue;
                }

                object component = info.GetValue(this.GetType().FullName + "." + type, found);

                for (int i = 0; i < this.components.Count; i++)
                {
                    if (this.components[i].GetType() == found)
                    {
                        this.components[i] = component as Facade.Component;
                        this.components[i].AssignModel(this);
                        component = 0;
                        break;
                    }
                }

                if (component is int)
                {
                    continue;
                }

                OnComponentNotFoundDuringDeserialization(found);
            }
        }

        internal abstract void LoadComponents();
    }

    [System.Serializable]
    public abstract class Model<T> : Model where T : Model<T>
    {
        public new Component<T>[] Components
        {
            get
            {
                return base.Components.Where(x => x is Component<T>).Select(x => x as Component<T>).ToArray();
            }
        }

        public Model() { }
        public Model(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static T CreateInstance()
        {
            T instance = Activator.CreateInstance(typeof(T), null) as T;

            return instance;
        }

        internal override void LoadComponents()
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
                        Component<T> instance = (item as Component<T>).CreateCloneFor((T)this);
                        components.Add(instance as Component);
                    }
                }

                type = type.BaseType;
            }
        }

        public D GetComponent<D>() where D : Component<T>
        {
            return this.Components.FirstOrDefault(x => x is D) as D;
        }

        public static D GetClassComponent<D>() where D : Component<T>
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