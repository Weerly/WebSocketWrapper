using System.Collections.Generic;
using Weerly.WebSocketWrapper.Abstractions;

namespace Weerly.WebSocketWrapper.Processing
{
    public class DataBuffer : IDataBuffer
    {
        private IList<object> Buffer { get; } = new List<object>();
        public int Length { get; private set; }

        public IDataBuffer Add(object obj)
        {
            Buffer.Add(obj);
            Length = Buffer.Count;

            return this;
        }
    }
}
