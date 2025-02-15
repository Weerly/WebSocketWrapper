using System.Collections.Generic;
using Weerly.WebSocketWrapper.Abstractions;

namespace Weerly.WebSocketWrapper.Processing
{
    /// <summary>
    /// Represents a buffer that holds data objects and provides functionality to add objects
    /// while maintaining a count of the total objects in the buffer.
    /// </summary>
    public class DataBuffer : IDataBuffer
    {
        private IList<object> Buffer { get; } = new List<object>();
        public int Length { get; private set; }

        /// <summary>
        /// Adds an object to the data buffer and updates the total count of objects in the buffer.
        /// </summary>
        /// <param name="obj">The object to be added to the buffer.</param>
        /// <returns>The updated instance of the <see cref="IDataBuffer"/> containing the added object.</returns>
        public IDataBuffer Add(object obj)
        {
            Buffer.Add(obj);
            Length = Buffer.Count;

            return this;
        }
    }
}
