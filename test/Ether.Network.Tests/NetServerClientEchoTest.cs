﻿using System.Linq;
using System.Threading.Tasks;
using Ether.Network.Tests.Contexts.Echo;
using Xunit;

namespace Ether.Network.Tests
{
    public class NetServerClientEchoTest
    {
        [Fact]
        public void ConnectClientToServer()
        {
            using (var server = new MyEchoServer())
            {
                server.Start();

                var client = new MyEchoClient("127.0.0.1", 4444, 512);

                client.Connect();
                Assert.True(client.ConnectedToServer);

                client.Disconnect();
                Assert.False(client.ConnectedToServer);

                client.Dispose();
            }
        }

        [Fact]
        public void ClientSendDataToServer()
        {
            SendDataToServer(1).Wait();
        }

        [Fact]
        public void ClientSendDataToServer50()
        {
            SendDataToServer(50).Wait();
        }

        [Fact]
        public void ClientSendDataToServer100()
        {
            SendDataToServer(100).Wait();
        }

        [Fact]
        public void ClientSendDataToServer200()
        {
            SendDataToServer(200).Wait();
        }

        [Fact]
        public void ClientSendDataToServer500()
        {
            SendDataToServer(500).Wait();
        }

        [Fact]
        public void ClientSendDataToServer1000()
        {
            SendDataToServer(1000).Wait();
        }

        private static async Task SendDataToServer(int messagesToSend)
        {
            using (var server = new MyEchoServer())
            {
                server.Start();

                var client = new MyEchoClient("127.0.0.1", 4444, 512);

                client.Connect();
                Assert.True(client.ConnectedToServer);

                for (var i = 0; i < messagesToSend; i++)
                {
                    client.SendRandomMessage();
                    await Task.Delay(10);
                }

                EchoClient clientFromServer = server.Clients.FirstOrDefault();
                Assert.NotNull(clientFromServer);

                // Wait for message
                while (clientFromServer.ReceivedData.Count < messagesToSend)
                    await Task.Delay(10);
                
                Assert.Equal(client.SendedData.Count, clientFromServer.ReceivedData.Count);

                for (var i = 0; i < messagesToSend; i++)
                {
                    string clientMessage = client.SendedData.ElementAt(i);
                    string serverMesssage = clientFromServer.ReceivedData.ElementAt(i);

                    Assert.NotNull(clientMessage);
                    Assert.NotNull(serverMesssage);
                    Assert.Equal(clientMessage, serverMesssage);
                }

                client.Disconnect();
                Assert.False(client.ConnectedToServer);
                client.Dispose();
            }
        }
    }
}
