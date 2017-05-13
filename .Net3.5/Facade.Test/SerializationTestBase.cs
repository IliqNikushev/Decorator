using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facade.Test
{
    public class SerializationTestBase
    {
        private System.IO.MemoryStream stream;
        private System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        protected void Serialize(object obj)
        {
            if (stream == null)
                stream = new System.IO.MemoryStream();
            if (stream.Length != 0)
                throw new InvalidOperationException("Cannot serialize twice");

            bf.Serialize(stream, obj);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        protected object Deserialize()
        {
            if (stream.Position != 0)
                throw new InvalidOperationException("Deserialization can happen after Serialization");

            object result = bf.Deserialize(stream);

            Clear();

            return result;
        }

        protected void Clear()
        {
            stream.Dispose();
            stream = null;
        }
    }
}
