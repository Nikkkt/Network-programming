using System.Net.Sockets;
using System.Net;

static void StartServer()
{
    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    int port = 12345;

    TcpListener listener = new TcpListener(ipAddress, port);

    listener.Start();
    Console.WriteLine("Сервер запущено...");

    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("Клієнт приєднався!");

    NetworkStream stream = client.GetStream();
    StreamReader reader = new StreamReader(stream);
    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

    string message;
    do
    {
        message = reader.ReadLine();
        Console.WriteLine("Клієнт: " + message);

        string response = GetRandomResponse();

        writer.WriteLine(response);
        Console.WriteLine("Сервер: " + response);

    } while (message != "Bye");

    stream.Close();
    client.Close();
    listener.Stop();
}

static string GetRandomResponse()
{
    string[] responses = { "Привіт!", "Я вас слухаю", "Як я можу вам допомогти?", "До побачення!", "Що нового?" };
    Random random = new Random();
    return responses[random.Next(responses.Length)];
}

StartServer();