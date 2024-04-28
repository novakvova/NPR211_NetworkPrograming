using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2.SimpleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вкажіть ip адресу сервера");
            var ip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Вкажіть port сервера");
            var port = int.Parse(Console.ReadLine());
            try
            {
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                //Сокет, який буде взяємодіяти із сервером
                Socket server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                server.Connect(endPoint);
                Console.Write("Вкажіть повідомлення для надсилання->_");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                server.Send(data); //Відправляємо дані на сервер
                //очікуємо відповідь від сервера
                data = new byte[1024];
                int bytes = 0; //розмір повідомлення від сервера
                do
                {
                    bytes = server.Receive(data); //отримати відповідь від сервера
                    Console.WriteLine("Нам прислав сервер {0}", Encoding.Unicode.GetString(data));
                } while (server.Available > 0);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }

        }
    }
}
