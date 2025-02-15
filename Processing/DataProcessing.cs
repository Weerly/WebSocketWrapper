using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Exceptions;

namespace Weerly.WebSocketWrapper.Processing
{


    /// <summary>
    /// Provides methods for processing WebSocket data connections and handling messages.
    /// </summary>
    public static class DataProcessing
    {
        private const int BufferSize = 1024 * 4;
        private const string InvalidObjectMessage = "[object Object]";
        private static IDataBuffer DataBuffer { get; set; }
        
        /// <summary>
        /// Handles the WebSocket connection process, receiving and sending data asynchronously.
        /// </summary>
        /// <param name="builder">The WebSocket route builder.</param>
        public static async Task ConnectionProcess(IWebSocketRouteBuilder builder)
        {
            var webSocket = await builder.Context.WebSockets.AcceptWebSocketAsync();
            var router = builder.RouteHandler.Router;

            try
            {
                var buffer = new byte[BufferSize];
                WebSocketReceiveResult receiveResult;
    
                do
                {
                    // Extract receive logic into a function for clarity
                    receiveResult = await ReceiveDataAsync(webSocket, buffer);
                    string messageContent = DecodeMessage(buffer);

                    if (IsInvalidMessage(messageContent))
                    {
                        throw new WebSocketException(WebSocketError.InvalidMessageType);
                    }

                    // Handle async vs sync message processing
                    if (router.IsAsync)
                    {
                        await HandleAsyncMessage(buffer, router.ActionData, webSocket, receiveResult);
                    }
                    else
                    {
                        await HandleMessage(buffer, router.ActionData, webSocket, receiveResult);
                    }

                } while (!receiveResult.CloseStatus.HasValue);

                await CloseWebSocketAsync(webSocket, receiveResult);
            }
            catch (WebSocketException wsEx)
            {
                HandleWebSocketException(wsEx, webSocket);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        /// <summary>
        /// Asynchronously receives data from a WebSocket connection.
        /// </summary>
        /// <param name="socket">The WebSocket instance used for communication.</param>
        /// <param name="buffer">The buffer to store the received data.</param>
        /// <returns>A task representing the asynchronous operation, containing the result of the WebSocket receive operation.</returns>
        private static async Task<WebSocketReceiveResult> ReceiveDataAsync(WebSocket socket, byte[] buffer)
        {
            return await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        /// <summary>
        /// Decodes a byte array into a UTF-8 encoded string, with trailing null characters removed.
        /// </summary>
        /// <param name="data">The byte array containing the encoded message data.</param>
        /// <returns>The decoded message as a string.</returns>
        private static string DecodeMessage(byte[] data) => Encoding.UTF8.GetString(data).TrimEnd((char)0);

        /// <summary>
        /// Determines whether the provided message is considered invalid based on predefined criteria.
        /// </summary>
        /// <param name="message">The message content to be evaluated.</param>
        /// <returns>True if the message is invalid; otherwise, false.</returns>
        private static bool IsInvalidMessage(string message) => message.Equals(InvalidObjectMessage);

        /// <summary>
        /// Processes an asynchronous WebSocket message by invoking the corresponding action and sending the response.
        /// </summary>
        /// <param name="buffer">The byte array containing the received message data.</param>
        /// <param name="actionData">A dictionary containing metadata and routing information for message processing.</param>
        /// <param name="socket">The WebSocket connection handling the message.</param>
        /// <param name="result">The result of the WebSocket receive operation, containing metadata about the received message.</param>
        /// <returns>A task representing the asynchronous operation of handling and responding to the message.</returns>
        private static async Task HandleAsyncMessage(byte[] buffer, IDictionary<string, string> actionData,
            WebSocket socket, WebSocketReceiveResult result)
        {
            await AsyncMessageHandle(
                buffer,
                actionData,
                async (sendData) =>
                {
                    await socket.SendAsync(sendData, result.MessageType, result.EndOfMessage, CancellationToken.None);
                });
        }

        /// <summary>
        /// Handles sending a response message to the WebSocket client after processing the received data.
        /// </summary>
        /// <param name="buffer">The buffer containing the received WebSocket message data.</param>
        /// <param name="actionData">A dictionary containing routing and action details for processing the message.</param>
        /// <param name="socket">The WebSocket connection through which the response will be sent.</param>
        /// <param name="result">The result of the WebSocket receive operation, containing metadata about the received message.</param>
        /// <returns>A task that represents the asynchronous operation of sending the response message.</returns>
        private static async Task HandleMessage(byte[] buffer, IDictionary<string, string> actionData, WebSocket socket,
            WebSocketReceiveResult result)
        {
            var responseData = MessageHandle(buffer, actionData);
            await socket.SendAsync(responseData, result.MessageType, result.EndOfMessage, CancellationToken.None);
        }

        /// <summary>
        /// Closes the WebSocket connection asynchronously, providing the appropriate close status and description.
        /// </summary>
        /// <param name="socket">The WebSocket instance to be closed.</param>
        /// <param name="result">The WebSocket receive result containing the close status and description.</param>
        /// <returns>A task that represents the asynchronous close operation.</returns>
        private static async Task CloseWebSocketAsync(WebSocket socket, WebSocketReceiveResult result)
        {
            if (result.CloseStatus != null)
                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription,
                    CancellationToken.None);
        }

        /// <summary>
        /// Handles exceptions occurring in the WebSocket communication process and performs necessary cleanup.
        /// </summary>
        /// <param name="ex">The WebSocket exception that was thrown during communication.</param>
        /// <param name="socket">The WebSocket instance associated with the current communication.</param>
        private static void HandleWebSocketException(WebSocketException ex, WebSocket socket)
        {
            Console.WriteLine($"WebSocket exception: {ex.GetType()} with message {ex.Message}");
            socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, ex.WebSocketErrorCode.ToString(), CancellationToken.None).Wait();
        }

        /// <summary>
        /// Handles incoming WebSocket messages and invokes the appropriate method based on action data.
        /// </summary>
        /// <param name="msg">The received message as a byte array.</param>
        /// <param name="actionData">A dictionary containing routing action data.</param>
        /// <param name="excepted">Specifies whether exception handling is required.</param>
        /// <returns>A byte array segment representing the response message.</returns>
        private static ArraySegment<byte> MessageHandle(byte[] msg, IDictionary<string, string> actionData, bool excepted = false)
        {
            var (objectType, methodName, classInstance, decodedMessage) = MessageHandlerWrapper(msg, actionData);
            var method = ResolveMethod(objectType, methodName, excepted);
            
            var args = GetArguments(method, decodedMessage);
            
            CheckForWrongReturnedTypeException<string>(method);
            var responseData = method.Invoke(classInstance, args);
            var byteArray = Encoding.ASCII.GetBytes((string)responseData);
            return new ArraySegment<byte>(byteArray);
        }


        /// <summary>
        /// Asynchronously handles a WebSocket message, invoking a specified method of a class or object
        /// and processing the response using a callback.
        /// </summary>
        /// <param name="msg">The byte array containing the received WebSocket message.</param>
        /// <param name="actionData">A dictionary containing the details for processing, such as the object type and method name.</param>
        /// <param name="callback">A callback invoked to send responses as a byte array.</param>
        /// <param name="excepted">Indicates whether to handle exceptions and retry processing (default is false).</param>
        /// <returns>An asynchronous operation representing the message handling workflow.</returns>
        private static async Task AsyncMessageHandle(byte[] msg, IDictionary<string, string> actionData, WebSocketByteCallback callback, bool excepted = false)
        {
            var (objectType, methodName, classInstance, decodedMessage) = MessageHandlerWrapper(msg, actionData);
            MethodInfo method = null;
            try
            {
                method = FindMethod(objectType, methodName, excepted);
            }
            catch (AmbiguousMatchException ex)
            {
                if (excepted)
                {
                    throw new AmbiguousMatchException(ex.Message, ex.InnerException);
                }
                await AsyncMessageHandle(msg, actionData, callback, true);
            }

            var args = GetArguments(method, decodedMessage, async (responseData) =>
            {
                var byteArray = Encoding.ASCII.GetBytes(responseData);
                await callback(new ArraySegment<byte>(byteArray));
            });

            CheckForWrongReturnedTypeException<Task>(method);
            await (Task)method.Invoke(classInstance, args); 
        }

        /// <summary>
        /// Validates if the return type of a given method matches the specified generic type parameter.
        /// </summary>
        /// <typeparam name="T">The expected return type of the method.</typeparam>
        /// <param name="method">The MethodInfo object that represents the method to be checked.</param>
        /// <exception cref="WrongReturnedTypeException">Thrown when the return type of the method does not match the expected type.</exception>
        private static void CheckForWrongReturnedTypeException<T>(MethodInfo method)
        {
            if (method == null || method.ReturnType != typeof(T))
            {
                throw new WrongReturnedTypeException();
            }
        }

        /// <summary>
        /// Constructs a WebSocketStructure object by processing the input message and action data.
        /// </summary>
        /// <param name="msg">The byte array representing the incoming WebSocket message.</param>
        /// <param name="actionData">The dictionary containing metadata such as object type and method name for handling the message.</param>
        /// <returns>A WebSocketStructure instance that encapsulates the parsed message, target object type, method name, and class instance.</returns>
        private static WebSocketStructure MessageHandlerWrapper(byte[] msg, IDictionary<string, string> actionData)
        {
            const char nullCharacter = (char)0;
            var entryAssembly = Assembly.GetEntryAssembly() ?? throw new WebSocketException(WebSocketError.NativeError);
            var objectType = entryAssembly.GetType(actionData["object"]);
            return new WebSocketStructure()
            {
                ObjectType = objectType,
                DecodedMessage = Encoding.UTF8.GetString(msg).TrimEnd(nullCharacter),
                MethodName = actionData["methodName"],
                ClassInstance = Activator.CreateInstance(objectType)
            };
        }

        /// <summary>
        /// Finds a method in the given object type by its name, with an optional exception-based search mechanism.
        /// </summary>
        /// <param name="objectType">The type of the object where the method search will be performed.</param>
        /// <param name="methodName">The name of the method to search for.</param>
        /// <param name="excepted">A flag indicating whether to use an exception-based approach for finding the method.</param>
        /// <returns>The found method as a MethodInfo object, or null if no matching method is found.</returns>
        private static MethodInfo FindMethod(Type objectType, string methodName, bool excepted)
        {
            return !excepted
                ? objectType.GetMethod(methodName)
                : objectType.GetMethods()
                    .FirstOrDefault(m => m.Name.Equals(methodName) && m.DeclaringType == objectType);
        }

        /// <summary>
        /// Resolves a method from a specified object type and method name, handling potential exceptions.
        /// </summary>
        /// <param name="objectType">The type of the object containing the method to resolve.</param>
        /// <param name="methodName">The name of the method to resolve.</param>
        /// <param name="isExceptionHandled">Indicates whether exceptions should be handled within the resolution process.</param>
        /// <returns>A MethodInfo object representing the resolved method.</returns>
        private static MethodInfo ResolveMethod(Type objectType, string methodName, bool isExceptionHandled)
        {
            try
            {
                return FindMethod(objectType, methodName, isExceptionHandled);
            }
            catch (AmbiguousMatchException) when (!isExceptionHandled)
            {
                return ResolveMethod(objectType, methodName, true);
            }
        }

        /// <summary>
        /// Retrieves method arguments based on the received message data.
        /// </summary>
        /// <param name="method">The method to extract arguments for.</param>
        /// <param name="msgToForArgs">The received message as a string.</param>
        /// <param name="callback"></param>
        /// <returns>An array of objects representing method arguments.</returns>
        private static object[] GetArguments(MethodInfo method, string msgToForArgs, WebSocketCallback callback = null)
        {
            if (DataBuffer == null)
            {
                DataBuffer = new DataBuffer();
            }

            var methodArguments = method.GetParameters();
            var args = new object[methodArguments.Length];

            foreach (var arg in methodArguments)
            {
                if (arg.ParameterType == typeof(string))
                {
                    args.SetValue(msgToForArgs, arg.Position);
                }

                if (arg.ParameterType == typeof(IDataBuffer))
                {
                    args.SetValue(DataBuffer, arg.Position);
                }

                if (arg.ParameterType == typeof(WebSocketCallback))
                {
                    args.SetValue(callback, arg.Position);
                }

                if (arg.ParameterType != typeof(String) && arg.ParameterType != typeof(IDataBuffer) && arg.ParameterType != typeof(WebSocketCallback))
                {
                    throw new ArgumentException("Arguments have wrong types");
                }
            }
            return args;
        }
    }
}