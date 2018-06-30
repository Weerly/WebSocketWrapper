using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Weerly.WebSocketWrapper.Routing;
using Weerly.WebSocketWrapper.Exceptions;

namespace Weerly.WebSocketWrapper.Processing
{
    public static class DataProcessing
    {
        public static IDataBuffer DataBuffer { get; set; }
        public static async Task ConnectionProcess(IWebSocketRouteBuilder builder)
        {
            WebSocket webSocket = await builder.Context.WebSockets.AcceptWebSocketAsync();
            IWebSocketRouter Router = builder.RouteHandler.Router;

            try
            {
                var receivedData = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedData), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {

                    ArraySegment<byte> sendData = MessageHandle(receivedData, Router.ActionData);

                    await webSocket.SendAsync(sendData, result.MessageType, result.EndOfMessage, CancellationToken.None);

                    receivedData = new byte[1024 * 4];
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receivedData), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure;
                Console.WriteLine("Exception type {0} with message {1}", ex.GetType(), ex.Message);
                await webSocket.CloseOutputAsync(closeStatus, ex.WebSocketErrorCode.ToString(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public static ArraySegment<byte> MessageHandle(byte[] msg, IDictionary<string, string> ActionData, bool excepted = false)
        {
            string msgToForArgs = Encoding.UTF8.GetString(msg).TrimEnd((Char)0);
            Type objectType = Assembly.GetEntryAssembly().GetType(ActionData["object"]);
            string func = ActionData["methodName"];
            object classInst = Activator.CreateInstance(objectType);
            MethodInfo method = null;

            try
            {
                if (!excepted)
                {
                    method = objectType.GetMethod(func);
                }
                else
                {
                    MethodInfo[] methods = objectType.GetMethods();

                    foreach (var m in methods)
                    {
                        if (m.Name.Equals(func) && m.DeclaringType.Equals(objectType))
                        {
                            method = m;
                            break;
                        }
                    }
                }

            }
            catch (AmbiguousMatchException ex)
            {
                if (excepted)
                {
                    throw new AmbiguousMatchException(ex.Message, ex.InnerException);
                }
                else
                {
                    return MessageHandle(msg, ActionData, true);
                }
            }

            object[] args = GetArguments(method, msgToForArgs);

            if (method.ReturnType != typeof(String))
            {
                throw new WrongReturnedTypeException();
            }
            else
            {
                object responsedData = method.Invoke(classInst, args);
                byte[] byteArray = Encoding.ASCII.GetBytes((string)responsedData);
                ArraySegment<byte> processedMessage = new ArraySegment<byte>(byteArray);

                return processedMessage;
            }
        }
        public static object[] GetArguments(MethodInfo method, string msgToForArgs)
        {
            if (DataBuffer == null)
            {
                DataBuffer = new DataBuffer();
            }

            ParameterInfo[] methodArguments = method.GetParameters();
            object[] args = new object[methodArguments.Length];

            foreach (ParameterInfo arg in methodArguments)
            {
                if (arg.ParameterType == typeof(String))
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