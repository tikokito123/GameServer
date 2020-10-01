using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Claims;

namespace GameServer
{
    class Server
    {
        public static int maxPlayers { get; private set; }
        public static int port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private static TcpListener tcplistener { get; set; }

        public static void Start(int _maxPlayer, int _port)
        {
            maxPlayers = _maxPlayer;
            port = _port;

            Console.WriteLine($"Starting Server...");
            InitializeServerData();
            tcplistener = new TcpListener(IPAddress.Any, port);
            tcplistener.Start();
            tcplistener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);
            Console.WriteLine($"Server Started on {port}.");
        }

        private static void TCPConnectCallBack(IAsyncResult _result)
        {
            TcpClient _client = tcplistener.EndAcceptTcpClient(_result);
            tcplistener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} falied to connect: server full!");
        }
        private static void InitializeServerData()
        {
            for (int i = 1; i < maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }

    }
}
