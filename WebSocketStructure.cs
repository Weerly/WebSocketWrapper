using System;
using System.Threading.Tasks;

namespace Weerly.WebSocketWrapper
{
    public delegate Task WebSocketCallback(string msg);
    public delegate Task WebSocketByteCallback(ArraySegment<byte> msg);

    /// <summary>
    /// Represents the structure of a WebSocket message, including metadata and parsed details.
    /// </summary>
    public class WebSocketStructure
    {
        public Type ObjectType {get; set;}
        public string DecodedMessage {get; set;}
        public string MethodName {get; set;}
        public object ClassInstance {get; set;}

        /// Deconstructs the WebSocketStructure object into its individual fields.
        /// <param name="objectType">The type of the object represented by this WebSocketStructure.</param>
        /// <param name="methodName">The name of the method associated with the WebSocketStructure.</param>
        /// <param name="classInstance">The class instance to which the WebSocketStructure is bound.</param>
        /// <param name="decodedMessage">The decoded message associated with the WebSocketStructure.</param>
        public void Deconstruct(out Type objectType, out string methodName, out object classInstance, out string decodedMessage)
        {
            objectType = ObjectType;
            methodName = MethodName;
            classInstance = ClassInstance;
            decodedMessage = DecodedMessage;
        }
    }
}
