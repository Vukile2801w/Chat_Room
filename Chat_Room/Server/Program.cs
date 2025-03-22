using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Server
{
    class Program
    {

        public static uint curect_id = 0;

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
                username = "Guest" + ID; // Dodajemo podrazumevani username
            }


            public Client(TcpClient tcpClient, string name)
            {
                TcpClient = tcpClient;
                stream = TcpClient.GetStream();

                ID = curect_id++;
                
                username = name;
            }
            public static bool operator ==(Client c1, Client c2)
            {
                return c1.ID == c2.ID;
            }

            public static bool operator !=(Client c1, Client c2)
            {
                return c1.ID != c2.ID;
            }

            public override bool Equals(object obj)
            {
                return obj is Client other && this.ID == other.ID;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

        }

        private static readonly List<string> EXIT_WORDS = new List<string> { "exit", "quit" };
        private static List<Client> ActiveClient = new List<Client>();
        private static readonly Encoding default_encoder = Encoding.UTF8;
        private static volatile bool serverRunning = true;  // Volatile osigurava da nitovi vide ažuriranu vrednost

        private static int Get_Index_Client(List<Client> active_client, Client client)
        {
            for (int i = 0; i < active_client.Count; i++)
            {
                if (active_client[i] == client) 
                    return i;
            }

            return -1;
        }

        private static void Send_to(Client client, string msg, Encoding encoder)
        {
            byte[] encoded_msg = encoder.GetBytes(msg);
            client.stream.Write(encoded_msg, 0, encoded_msg.Length);
        }

        private static string Recv_from(byte[] buffer, int bytesRead, Encoding encoder)
        {
            return encoder.GetString(buffer, 0, bytesRead);
        }

        private static void Send_to_All(Client sender, string msg)
        {
            string formattedMessage = $"[{DateTime.Now:HH:mm}] {sender.username}: {msg}\n";
            foreach (var client in ActiveClient)
            {
                if (client == sender) continue;

                NetworkStream stream = client.TcpClient.GetStream();
                Send_to(sender, formattedMessage, default_encoder);
            }
        }

        private static string HandleCommand(Client client, string msg)
        {
            string response = "";

            // Provera da li string nije prazan i da počinje s '!'
            if (string.IsNullOrWhiteSpace(msg) || msg.Length < 2)
            {
                return response;
            }

            msg = msg.Substring(1); // Uklanjamo prvi znak ('!')
            string[] tokens = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length == 0)
            {
                return response;
            }

            string command = tokens[0];
            string subcommand = tokens.Length > 1 ? tokens[1] : null;

            switch (command.ToLower()) // Ignorišemo velika/mala slova
            {
                case "username":
                    if (subcommand == null)
                    {
                        return response;
                    }

                    switch (subcommand.ToLower())
                    {
                        case "get":
                            response = client.username;
                            break;

                        case "set":
                            if (tokens.Length > 2)
                            {
                                client.username = tokens[2];
                                response = "Postavljeno.";
                            }
                            else
                            {
                                response = "Greška: Nedostaje korisničko ime za 'username set'.";
                            }
                            break;

                        default:
                            response = $"Nevažeća podkomanda: {subcommand}";
                            break;
                    }
                    break;

                default:
                    response = $"Nepoznata komanda: {command}";
                    break;
            }

            if (tokens.Length > 0 && tokens[tokens.Length - 1] == "-nr")
                return $"{response} -nr";
            else
                return response;
        }


        private static void HandleClient(Client client)
        {
            try
            {
                NetworkStream stream = client.TcpClient.GetStream();

                Console.WriteLine($"Client connected: {client.TcpClient.Client.RemoteEndPoint}");

                byte[] buffer = new byte[1024];
                int bytesRead;

                string username = HandleCommand(client, Recv_from(buffer, stream.Read(buffer, 0, buffer.Length), default_encoder));
                username = username.Remove(username.Length - ("-nr".Length + 1));

                Send_to(
                    client,
                    username,
                    default_encoder
                    );

                // Pošaljemo ovu poruku samo jednom pri povezivanju!
                Send_to(client, "[SERVER]: Connection established\n", default_encoder);
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedMessage = Recv_from(buffer, bytesRead, default_encoder);

                    if (receivedMessage[0] == '/')
                    {
                        string response = HandleCommand(client, receivedMessage);

                        if (!string.IsNullOrEmpty(response))
                        {
                            bool is_no_replay = response.EndsWith("-nr");

                            if (!response.StartsWith("[SERVER]:") && !is_no_replay)
                            {
                                response = $"[SERVER]: {response}";
                            }
                            if (is_no_replay)
                            {
                                response.Remove(response.Length - ("-nr".Length + 1));
                            }

                            Send_to(client, response + "\n", default_encoder);
                        }
                        continue;
                    }

                    string packet = $"{client.username}: {receivedMessage}";
                    Console.WriteLine($"Received: {packet.Trim()}");

                    Send_to_All(client, packet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Client disconnected.");
                ActiveClient.Remove(client);
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

                    serverRunning = false; // Prekida glavnu petlju

                    // Obavesti sve klijente i zatvori ih
                    foreach (Client client in ActiveClient)
                    {
                        Send_to(client, "[SERVER] Server is shutting down. Disconnected.", default_encoder);
                        client.TcpClient.Close();
                    }

                    ActiveClient.Clear();
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

            // Pokrećemo admin input u posebnom thread-u
            Task.Run(() => HandleAdminInput(server));

            while (serverRunning)
            {
                try
                {
                    Client client = new Client(server.AcceptTcpClient());
                    ActiveClient.Add(client);
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
