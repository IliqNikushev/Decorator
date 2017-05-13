using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace Facade.Test
{
    class Human : Model<Human>
    {
        protected Human(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
