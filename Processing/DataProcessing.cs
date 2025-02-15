using System;
using System.Collections.Generic;
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
                var receivedData = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedData), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    var checkData = Encoding.UTF8.GetString(receivedData).TrimEnd((Char)0);
                    if (checkData.Equals("[object Object]"))
                    {
                        throw new WebSocketException(WebSocketError.InvalidMessageType);
                    }
                    ArraySegment<byte> sendData = MessageHandle(receivedData, router.ActionData);

                    await webSocket.SendAsync(sendData, result.MessageType, result.EndOfMessage, CancellationToken.None);

                    receivedData = new byte[1024 * 4];
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedData), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine("Exception type {0} with message {1}", ex.GetType(), ex.Message);
                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure,
                    ex.WebSocketErrorCode.ToString(),
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
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
            var objectType = Assembly.GetEntryAssembly() == null
                ? throw new WebSocketException(WebSocketError.NativeError)
                : Assembly.GetEntryAssembly()?.GetType(actionData["object"]);
            var msgToForArgs = Encoding.UTF8.GetString(msg).TrimEnd((char)0);
            var func = actionData["methodName"];
            var classInst = Activator.CreateInstance(objectType);
            MethodInfo method = null;

            try
            {
                if (!excepted)
                {
                    method = objectType.GetMethod(func);
                }
                else
                {
                    var methods = objectType.GetMethods();

                    foreach (var m in methods)
                    {
                        if (!m.Name.Equals(func) || m.DeclaringType != objectType) continue;
                        method = m;
                        break;
                    }
                }

            }
            catch (AmbiguousMatchException ex)
            {
                if (excepted)
                {
                    throw new AmbiguousMatchException(ex.Message, ex.InnerException);
                }

                return MessageHandle(msg, actionData, true);
            }

            var args = GetArguments(method, msgToForArgs);

            if (method == null || method.ReturnType != typeof(string))
            {
                throw new WrongReturnedTypeException();
            }
            var responseData = method.Invoke(classInst, args);
            var byteArray = Encoding.ASCII.GetBytes((string)responseData);
            return new ArraySegment<byte>(byteArray);
        }
        
        /// <summary>
        /// Retrieves method arguments based on the received message data.
        /// </summary>
        /// <param name="method">The method to extract arguments for.</param>
        /// <param name="msgToForArgs">The received message as a string.</param>
        /// <returns>An array of objects representing method arguments.</returns>
        private static object[] GetArguments(MethodInfo method, string msgToForArgs)
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

                if (arg.ParameterType != typeof(String) && arg.ParameterType != typeof(IDataBuffer))
                {
                    throw new ArgumentException("Arguments have wrong types");
                }
            }
            return args;
        }
    }
}