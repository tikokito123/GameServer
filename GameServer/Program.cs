using System;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start(10, 26950);
            Console.Title = "Game Server";
            Console.ReadKey();
        }
    }
}
