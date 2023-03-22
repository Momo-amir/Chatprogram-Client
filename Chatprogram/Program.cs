using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Connect to a remote host.
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8080;
        TcpClient client = new TcpClient();
        client.Connect(ipAddress, port);

        // Send and receive data.
        NetworkStream stream = client.GetStream();
        Console.WriteLine("Connected to chat server. Enter your username:");
        string username = Console.ReadLine();
        byte[] data = Encoding.ASCII.GetBytes(username);
        stream.Write(data, 0, data.Length);

        // Start message loop.
        Console.WriteLine("Enter messages to send to the chat room:");
        while (true)
        {
            string message = Console.ReadLine();
            data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine(responseData);
        }

        // Clean up.
        stream.Close();
        client.Close();
    }
}