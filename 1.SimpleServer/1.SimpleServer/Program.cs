using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _1.SimpleServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var hostName = Dns.GetHostName();
            Console.WriteLine("This host name {0}", hostName);
            IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
            int selectIP = 0;
            // This is the IP address of the local machine
            int i = 0;
            foreach(var item in localhost.AddressList)
            {

                await Console.Out.WriteLineAsync($"{++i}.{item.ToString()}");
            }
            Console.Write("->_");
            i = int.Parse(Console.ReadLine()) - 1;
            //Локальний ip адреса ПК
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            var ip = localhost.AddressList[i];
            int port = 2083;

            IPEndPoint endPoint = new IPEndPoint(ip, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp);   
            Console.Title = endPoint.ToString();
            try
            {
                //Привязка до IP адреси і порта машини
                socket.Bind(endPoint);
                socket.Listen(10); //початок прослуховання запитів від клієнтів
                Console.WriteLine("Run Server {0}", endPoint);
                while (true)
                {
                    Console.WriteLine("Успішний зпуск сервака очікую запитів");
                    Socket client = socket.Accept(); //На даному блоці ми зупинилися і чекамємо клієнтів
                    int bytes = 0; //кількість байт повідомлення від клієнта
                    byte[] data = new byte[1024]; //масив для збеірігання даних у запиті
                    do
                    {
                        bytes = client.Receive(data);
                        Console.WriteLine("Повідомлення {0}", Encoding.Unicode.GetString(data));
                    } while (client.Available > 0); //цикл продожуємо доки не досягли кінця повідомлення 

                    Console.WriteLine("Client EndPoint {0}", client.RemoteEndPoint);
                    string message = "Дякую. Ваш запит прийнято " + DateTime.Now;
                    data = Encoding.Unicode.GetBytes(message);
                    client.Send(data); // клієнту назад відпавляю повідомлення
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Щось пішло не так {0}", ex.Message);
            }
        }
    }
}
