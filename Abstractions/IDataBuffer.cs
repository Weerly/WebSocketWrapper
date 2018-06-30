namespace Weerly.WebSocketWrapper.Processing
{
    public interface IDataBuffer
    {
        int Length { get; }
        IDataBuffer Add(object obj);
    }
}
