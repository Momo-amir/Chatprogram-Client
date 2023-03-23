using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Transactions;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string username;
            IPAddress IP;
            
            // Prompt for username until it is not empty
            do
            {
                Console.Write("Enter your username: ");
                username = Console.ReadLine();
            }
            while (username == null || username == "");

            // Prompt for IP address until it is a valid IP address
            do Console.Write("Enter the IP address to connect to: ");
            while (!IPAddress.TryParse(Console.ReadLine(), out IP));
            
            TcpClient client = new TcpClient();
            client.Connect(IP, 1234);

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

            // Listen for incoming messages forwarded by the server
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
