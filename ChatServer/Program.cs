using System;
using System.Net;
using System.Net.Sockets;
#pragma warning disable IDE0051;

namespace ChatServer
{
    class Program
    {
        static TcpListener _listener;
        static List<Client> _users;
        static void Main(string[] args)
        {
            _users= new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 65293);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);

                /* Broadcast to everyone*/
            }
            
        }
        }
    }

