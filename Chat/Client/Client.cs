using System.Net.Sockets;

void StartClient()
{
    string serverIP = "127.0.0.1";
    int port = 12345;

    TcpClient client = new TcpClient(serverIP, port);
    Console.WriteLine("Підключено до сервера...");

    NetworkStream stream = client.GetStream();
    StreamReader reader = new StreamReader(stream);
    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

    string message;
    do
    {
        Console.Write("Ви: ");
        message = Console.ReadLine();

        writer.WriteLine(message);

        string response = reader.ReadLine();
        Console.WriteLine("Сервер: " + response);

    } while (message != "Bye");

    stream.Close();
    client.Close();
}

StartClient();