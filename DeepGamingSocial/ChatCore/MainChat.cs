using DeepGamingSocial.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepGamingSocial.ChatCore
{
     class MainChat
    {
        public relayCommand ConnectToServerCommand { get; set; }
        private Server _server;
        public string Username { get; set; }
        public MainChat()
        {
            string path = "C:\\DeepGamingData\\user\\user.bat";
            using var stream = new FileStream(path: path, FileMode.Open);
            Username = import(stream);
            _server = new Server();
            _server.connectedEvent += UserConnected;
            ConnectToServerCommand = new relayCommand(o => _server.ConnectToServer(Username));
        }
        private void UserConnected()
        {

        }
        public string import(Stream stream)
        {
            var byte_leng = new byte[4];
            stream.Read(byte_leng, 0, 4);
            int leng = BitConverter.ToInt32(byte_leng, 0);

            var byte_text = new byte[leng];
            stream.Read(byte_text, 0, leng);
            string text;
            text = Encoding.UTF8.GetString(byte_text);
            return text;
        }
    }
}
