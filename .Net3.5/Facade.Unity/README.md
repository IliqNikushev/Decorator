# Facade.Unity
A C# Library that introduces dynamic Decorato definitions inside Unity.  
Allows for definition of behaviours of Models and Facades.

### Exposes
```
namespace Facade.Unity:
    Model<T> extends Facade.Model<T>
    Component<T> where T: Model<T>
    ModelBehaviour<T> where T: Model<T>
```

### About
Implements a basic extention of the Facade library that exposes Components to be exposed.

- Assets/Scripts
  - Human/ 
    - Components/
      - Face.cs
      - Aging.cs
    - Human.cs
    - **HumanBehaviour.cs**

```
//Human/HumanBehaviour.cs
using Facade.Unity;

namespace Human
{
    using Components;

    class HumanBehaviour : Model<Human>
    {
        private Aging aging;

        private void Start()
        {
            this.aging = base.Model.GetComponent<Aging>();
        }

        private void Update()
        {
            aging.Advance(1);
            UnityEngine.Debug.Log("Current age:" + aging.CurrentAge);
        }
    }
}
```