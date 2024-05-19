using System.Net;
using System;

namespace _5.SMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string myIP = "3.64.137.133";
            string apiKey = "ua976c69739d15fa0df41dfeebc34fb6c46178e39d211742007203ac01e8970c82235f";
            //Отримати баланс
            string urlBalance = $@"http://api.mobizon.ua/service/user/getownbalance?apiKey={apiKey}";


            WebRequest request = WebRequest.Create(urlBalance);
            request.ContentType = "application/json";
            request.Method = "GET";

            try
            {
                // Get the response
                using (WebResponse response = request.GetResponse())
                {
                    // Get the stream containing content returned by the server
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Read the content
                            string responseFromServer = reader.ReadToEnd();
                            // Display the content
                            Console.WriteLine(responseFromServer);
                        }
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("WebException occurred: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred: {0}", e.Message);
            }

            //Console.WriteLine("Hello, World!");
        }
    }
}
