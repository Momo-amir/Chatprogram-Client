using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            TcpClient client = new TcpClient();
            client.Connect("localhost", 1234);

            NetworkStream stream = client.GetStream();

            // Send the username to the server
            byte[] usernameBuffer = Encoding.ASCII.GetBytes(username);
            stream.Write(usernameBuffer, 0, usernameBuffer.Length);

            // Start a new thread to handle server messages
            System.Threading.Thread t = new System.Threading.Thread(() => HandleServerMessages(client));
            t.Start();

            // Read user input and send messages to the server
            while (true)
            {
                string message = Console.ReadLine();
                byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBuffer, 0, messageBuffer.Length);
            }
        }

        static void HandleServerMessages(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);
            }
        }
    }
}
