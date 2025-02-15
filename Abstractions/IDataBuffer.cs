namespace Weerly.WebSocketWrapper.Abstractions
{
    public interface IDataBuffer
    {
        int Length { get; }
        IDataBuffer Add(object obj);
    }
}
