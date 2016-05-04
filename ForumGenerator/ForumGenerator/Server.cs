using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ForumGenerator
{
    class Server
    {
        private TcpListener _server;
        private Boolean _isRunning;
        private DAL dal;

        static void Main(string[] args)
        {
           Server server = new Server();
        }
        
        public Server()
        {
            dal = new DAL();

            IPAddress localAddr = IPAddress.Parse("132.72.232.230");
            _server = new TcpListener(localAddr, 1717);
            _server.Start();

            _isRunning = true;

            LoopClients();
        }

        public void LoopClients()
        {
            while (_isRunning)
            {
                TcpClient newClient = _server.AcceptTcpClient();

                Console.WriteLine("Accepted connection");

                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

            Boolean bClientConnected = true;
            String sData = null;
            DateTime dt;

            while (bClientConnected)
            {
                sData = sReader.ReadLine();

                if(sData == null)
                    break;
                dt = DateTime.Now;
                Console.WriteLine(dt.ToLongTimeString() + ": Recieved request " + sData);

                string ans = dal.SendMessage(sData);
                
                dt = DateTime.Now;
                Console.WriteLine(dt.ToLongTimeString() + ": Sending response " + ans);

                sWriter.WriteLine(ans);
                sWriter.Flush();
            }

            Console.WriteLine("Client disconnected " + client.Client);
        }
    }
}
