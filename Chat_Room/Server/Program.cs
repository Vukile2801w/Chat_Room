using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Server
{
    class Program
    {
        public static uint curect_id = 0;
        private static readonly object clientListLock = new object();

        private class Client
        {
            public TcpClient TcpClient;
            public NetworkStream stream;
            public string username;
            public uint ID;

            public Client(TcpClient tcpClient)
            {
                TcpClient = tcpClient;
                stream = TcpClient.GetStream();
                ID = curect_id++;
                username = "Guest" + ID;
            }
        }

        private static readonly List<string> EXIT_WORDS = new List<string> { "exit", "quit" };
        private static List<Client> ActiveClient = new List<Client>();
        private static readonly Encoding default_encoder = Encoding.UTF8;
        private static volatile bool serverRunning = true;

        private static void Send_to(Client client, string msg, Encoding encoder)
        {
            try
            {
                byte[] encoded_msg = encoder.GetBytes(msg);
                client.stream.Write(encoded_msg, 0, encoded_msg.Length);
            }
            catch (IOException)
            {
                Console.WriteLine($"Greška pri slanju podataka klijentu {client.username}, zatvaranje veze.");
                lock (clientListLock)
                {
                    ActiveClient.Remove(client);
                }
                client.TcpClient.Close();
            }
        }

        private static string Recv_from(byte[] buffer, int bytesRead, Encoding encoder)
        {
            if (bytesRead <= 0) return string.Empty;
            return encoder.GetString(buffer, 0, bytesRead);
        }

        private static void Send_to_All(Client sender, string msg, bool decorator = true)
        {
            string formattedMessage = decorator ? $"{sender.username}: {msg}" : msg;

            lock (clientListLock)
            {
                for (int i = ActiveClient.Count - 1; i >= 0; i--)
                {
                    var client = ActiveClient[i];
                    if (client != null) if (client == sender) continue;
                    
                    Send_to(client, formattedMessage, default_encoder);
                    
                }
            }
        }


        private static string HandleCommand(Client client, string msg)
        {
            if (string.IsNullOrWhiteSpace(msg) || msg.Length < 2 || msg[0] != '/')
            {
                return "";
            }

            msg = msg.Substring(1);
            string[] tokens = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return "";

            string command = tokens[0].ToLower();
            string response = "";

            if (command == "username" && tokens.Length > 1)
            {
                if (tokens[1] == "get")
                    response = client.username;
                else if (tokens[1] == "set" && tokens.Length > 2)
                {
                    client.username = tokens[2].Trim();
                    response = "Postavljeno";
                }
            }
            return response.EndsWith("-nr") ? response[..^4] : response;
        }


        private static string lastReceivedMessage = "";
        private static void HandleClient(Client client)
        {
            try
            {
                NetworkStream stream = client.TcpClient.GetStream();


                byte[] buffer = new byte[1024];
                int bytesRead;
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                HandleCommand(client, Recv_from(buffer, bytesRead, default_encoder));

                Console.WriteLine($"Client connected: {client.TcpClient.Client.RemoteEndPoint} ({client.username})");

                Send_to(client, $"[SERVER]: Welcome to the chat, {client.username}!", default_encoder);

                // Obaveštavanje novog klijenta da se povezao i slanje liste postojećih korisnika.
                Send_to_All(client, $"/new_client {client.username}", false); // Novi klijent se obaveštava da se povezao.
                

                foreach (Client c in ActiveClient)
                {
                    Console.WriteLine($"Sending /new_client to {c.username}...");
                    Send_to(client, $"/new_client {c.username}", default_encoder);
                    Thread.Sleep(25);
                }







                Send_to_All(client, $"[SERVER]: {client.username} joined the chat.");

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (bytesRead == 0) break;

                    string receivedMessage = Recv_from(buffer, bytesRead, default_encoder);

                    if (receivedMessage == lastReceivedMessage)
                    {
                        continue; // Ignoriši duplikate
                    }

                    if (HandleCommand(client, receivedMessage) == "Postavljeno")
                    {
                        continue;
                    }
                    

                    lastReceivedMessage = receivedMessage; // Zapamti poslednju poruku
                    Console.WriteLine($"Received: {client.username}: {receivedMessage.Trim()}");
                    Send_to_All(client, receivedMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Client {client.username} disconnected.");
                lock (clientListLock)
                {
                    ActiveClient.Remove(client);
                }
                Send_to_All(client, $"[SERVER]: {client.username} left the chat.");
                Send_to_All(null, $"/exit_client {client.username}", false);

                client.TcpClient.Close();
            }
        }


        private static void HandleAdminInput(TcpListener server)
        {
            while (serverRunning)
            {
                Console.Write("~ ");
                string adminInput = Console.ReadLine().ToLower();

                if (EXIT_WORDS.Contains(adminInput))
                {
                    Console.WriteLine("Shutting down server...");
                    serverRunning = false;
                    lock (clientListLock)
                    {
                        foreach (Client client in ActiveClient)
                        {
                            Send_to(client, "[SERVER] Server is shutting down. Disconnected.", default_encoder);
                            client.TcpClient.Close();
                        }
                        ActiveClient.Clear();
                    }
                    server.Stop();
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Any;
            int ipPort = 5050;
            TcpListener server = new TcpListener(ipAddress, ipPort);
            server.Start();

            Console.WriteLine($"Server is listening on {ipAddress}:{ipPort}");
            Console.WriteLine("Waiting for connections...");

            Task.Run(() => HandleAdminInput(server));

            while (serverRunning)
            {
                try
                {
                    Client client = new Client(server.AcceptTcpClient());
                    lock (clientListLock)
                    {
                        ActiveClient.Add(client);
                    }
                    Task.Run(() => HandleClient(client));
                }
                catch (SocketException)
                {
                    Console.WriteLine("Server has stopped accepting new connections.");
                    break;
                }
            }
            Console.WriteLine("Server shut down.");
            
        }
    }
}