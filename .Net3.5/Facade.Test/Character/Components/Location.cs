using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facade.Test.Character.Components
{
    class Location : Component<Human>
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public void MoveTo(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
