using System.Collections.Generic;

namespace Weerly.WebSocketWrapper.Processing
{
    public class DataBuffer : IDataBuffer
    {
        public IList<object> Buffer { get; }
        private int length { get; set; }
        public int Length { get { return length; }}

        public DataBuffer()
        {
            Buffer = new List<object>();
        }
        public IDataBuffer Add(object obj)
        {
            Buffer.Add(obj);
            length = Buffer.Count;

            return this;
        }
    }
}
