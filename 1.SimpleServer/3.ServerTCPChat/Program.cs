using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _3.ServerTCPChat
{
    internal class Program
    {
        //змінна для блокування роботи потоку від стороніх користувачів
        static readonly object _lock = new object();
        //Список клієнтів у чаті
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //лічильник клієнтів
            int count = 1;
            string fileName = "config.txt";
            IPAddress ip;
            int port;
            using (StreamReader sr = new StreamReader(fileName))
            {
                ip = IPAddress.Parse(sr.ReadLine());
                port = int.Parse(sr.ReadLine());
            }
            var hostName = Dns.GetHostName();
            Console.WriteLine("This host name {0}", hostName);
            IPHostEntry localhost = Dns.GetHostEntryAsync(hostName).Result;
            int selectIP = 0;
            // This is the IP address of the local machine
            int i = 0;
            foreach (var item in localhost.AddressList)
            {

                Console.WriteLine($"{++i}.{item.ToString()}");
            }
            Console.Write("->_");
            i = int.Parse(Console.ReadLine()) - 1;
            ip = localhost.AddressList[i];

            TcpListener socketServer = new TcpListener(ip, port);
            socketServer.Start();
            Console.WriteLine("Запуск сервера {0}:{1}", ip, port);
            while (true)
            {
                TcpClient client = socketServer.AcceptTcpClient();
                lock(_lock)
                {
                    list_clients.Add(count, client);
                }
                Console.WriteLine("Появився на сервері новий клієнт {0}", client.Client.RemoteEndPoint);
                Thread t = new Thread(handle_clients);
                t.Start(count);
                count++;
            }

        }

        public static void handle_clients(object c)
        {
            int id = (int)c;
            TcpClient client;
            lock (_lock)
            {
                client = list_clients[id]; //отримує інформацію про клієнта по його номеру у списку
            }
            try
            {
                while (true)
                {
                    //Отримуємо посилання на потік зяднання із кліжнтом
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[16054400];
                    int byte_count = stream.Read(buffer);
                    if (byte_count==0)
                    {
                        break;
                    }
                    string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                    Console.WriteLine("Client message {0}", data);
                    brodcast(data);
                }
            }
            catch { }
            lock(_lock)
            {
                Console.WriteLine("Клієнт покинув чат {0}", client.Client.RemoteEndPoint);
                list_clients.Remove(id);
            }
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static void brodcast(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);   
            lock (_lock)
            {
                try
                {
                    foreach(var c in list_clients.Values)
                    {
                        NetworkStream stream = c.GetStream(); //посилання на кліжнта із списка
                        stream.Write(buffer);
                    }
                }
                catch { }
            }
        }
    }
}
