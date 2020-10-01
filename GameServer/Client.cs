using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.CompilerServices;

namespace GameServer
{
    class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;
        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
        public class TCP
        {
            public TcpClient socket;
            private readonly int id;
            private NetworkStream stream;
            private byte[] reciveBuffer;
            public TCP(int _id)
            {
                id = _id;
            }
            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                reciveBuffer = new byte[dataBufferSize];
                stream.BeginRead(reciveBuffer, 0, dataBufferSize,ReceiveCallBack, null);
                //TODO 
            }
            private void ReceiveCallBack(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        // TODO: disconnect;
                        return;
                    }
                    Byte[] _data = new byte[_byteLength];
                    Array.Copy(reciveBuffer, _data, _byteLength);

                    //TODO handle data
                    stream.BeginRead(reciveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    //TODO Disconect
                }
            }
        }
    }
}

