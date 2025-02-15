namespace Weerly.WebSocketWrapper.Abstractions
{
    /// <summary>
    /// Represents a buffer interface for managing data objects. Provides functionality
    /// to add objects to the buffer and retrieve the number of objects currently stored.
    /// </summary>
    public interface IDataBuffer
    {
        int Length { get; }
        IDataBuffer Add(object obj);
    }
}
