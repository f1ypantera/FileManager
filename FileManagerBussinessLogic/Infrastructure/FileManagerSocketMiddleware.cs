using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace FileManagerBussinessLogic.Infrastructure
{
    public class FileManagerSocketMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly FileSocketManager fileSocketManager;

        public FileManagerSocketMiddleware(RequestDelegate requestDelegate, FileSocketManager fileSocketManager)
        {
            this.requestDelegate = requestDelegate;
            this.fileSocketManager = fileSocketManager;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await requestDelegate.Invoke(context);
                return;
            }
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var id = fileSocketManager.AddSocket(socket);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await fileSocketManager.RemoveSocket(id);
                    return;
                }
            });
        }
        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                        cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
