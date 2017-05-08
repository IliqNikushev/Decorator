using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facade.Test.Character.Components
{
    class Stats : Component<Human>
    {
        public int AgeYears { get; private set; }

        public void Age(int years)
        {
            this.AgeYears += 1;
        }
    }
}
