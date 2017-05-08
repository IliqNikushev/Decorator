# Facade
A C# Library that introduces dynamic [Facade Pattern](https://en.wikipedia.org/wiki/Facade_pattern) definitions.  

### Exposes
```
namespace Facade:
    Model<T>
    Component<T> where T:Model<T>
```

using Model as the Facade  
using Component as the SubSystem  

### Intent
- Remove direct references to seperate components
- Introduce generic encapsulation of components
- Allow Dynamic extention of classes

### Usage
```
using Facade;
class Human: Model<Human>{}
class Location : Component<Human>
{
    public float X{get; private set;}
    public float Y{get; private set;}

    public void MoveTo(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }
}

// Prorgram.cs
void Main()
{
    Human human = new Human();
    Location location = human.GetComponent<Location>()

    location.MoveTo(1, 1);
    Console.WriteLine("{0}x{1}", location.X, location.Y)
}
```
### Serialization
Each **Model** can define its own `OnSerialization`, `OnDeserialization`  
Each **Component** can define its own `OnSerialization`, `OnDeserialization`  
**Default** implementation assignes and writes all **Fields**

### About

This library extends the [Facade Pattern](https://en.wikipedia.org/wiki/Facade_pattern) and introduces dynamic definitions.
The implementation is similar to how it is done in Unity3D using Components.

During my development flow I strive to reuse my code.  
The facade pattern has been quite useful in splitting up certain functional parts.  
I have seen that most classes tend to become big and thus resulting in thousands of methods under one class.  
I implemented this dynamic facade pattern so that my development flow can be split across DLL's which can later be versioned independently.  

- Allows for dynamic extention of classes  
- Prevents the necessity of having 1 large DLL  
- External parties can also include their components

example:
  - Character.dll
    - model Character
    - class Modifier
    - component for Character: Location {x, y, world}
    - component for Character: Stats {age, name}
      - using Modifier to alter the resulting stats
  - Equipment.dll
    - class Item, Weapon, Armor
    - component for Character: Inventory {slots for Items}
    - component for Character: Equipment {slots for Armor, Weapons}
      - Each changing Character's stats though Modifiers