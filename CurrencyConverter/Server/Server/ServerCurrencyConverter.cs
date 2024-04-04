using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    internal class ServerCurrencyConverter
    {
        static public void StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("Server started...");

            do
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("New client connected");
                HandleClient(client);
            } while (true);
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            string[] currencies = request.Split(' ');

            DateTime startTime = DateTime.Now;
            Console.WriteLine($"Start of request processing: {startTime}");

            Task<string> responseTask = GetCurrencyRate(currencies[0], currencies[1]);
            responseTask.Wait();
            string response = responseTask.Result;

            byte[] data = Encoding.UTF8.GetBytes(response);
            stream.Write(data, 0, data.Length);

            DateTime endTime = DateTime.Now;
            Console.WriteLine($"End of request processing: {endTime}");

            TimeSpan duration = endTime - startTime;
            Console.WriteLine($"Request processing duration: {duration.TotalMilliseconds} ms");

            Console.WriteLine($"Client requested: {request}, currency: {response}");

            client.Close();
        }

        static async Task<string> GetCurrencyRate(string currencyFrom, string currencyTo)
        {
            string appId = "32e8779e5476d6a95a763553";
            string apiUrl = $"https://v6.exchangerate-api.com/v6/{appId}/pair/{currencyFrom}/{currencyTo}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "CurrencyServer");

                DateTime startTime = DateTime.Now;
                Console.WriteLine($"Start of API request: {startTime}");

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseBody);
                    double rate = result["conversion_rate"];

                    DateTime endTime = DateTime.Now;
                    Console.WriteLine($"End of API request: {endTime}");

                    TimeSpan duration = endTime - startTime;
                    Console.WriteLine($"API request duration: {duration.TotalMilliseconds} ms");

                    return rate.ToString();
                }
                else return "ERROR: currency getting error";
            }
        }
    }
}
