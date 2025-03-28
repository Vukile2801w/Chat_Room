using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public class Client_Network
    {
        private readonly List<string> EXIT_WORDS = new List<string> { "exit", "quit" };
        private bool isActive = false;
        public string username = "New User";
        private NetworkStream server_stream;

        // Eventi
        public delegate void Primljena_Poruka(string message);
        public delegate void Novi_Korisnik(string message);
        public delegate void Uklonjen_Korisnik(string message);

        public event Primljena_Poruka OnNewMessage = delegate { };
        public event Novi_Korisnik OnNewUser = delegate { };
        public event Uklonjen_Korisnik OnExitUser = delegate { };

        public Client_Network(IPAddress ip, int port, string username)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(ip, port));

                server_stream = client.GetStream();
                isActive = true;

                // Pošalji username serveru
                Send_to($"/username set {username}");

                // Pročitaj potvrdu od servera
                byte[] buffer = new byte[1024];
                int bytesRead = server_stream.Read(buffer, 0, buffer.Length);
                this.username = username;

                Console.WriteLine($"Connected as {this.username}");

                // Pokreni procese za slanje i primanje poruka
                Task.Run(() => HandleReceiveMessages());

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
                isActive = false;
            }
        }

        private void Send_to(string msg)
        {
            if (!isActive || server_stream == null || !server_stream.CanWrite)
                return;

            try
            {
                byte[] encoded_msg = Encoding.UTF8.GetBytes(msg + "\n");
                server_stream.Write(encoded_msg, 0, encoded_msg.Length);
            }
            catch
            {
                Console.WriteLine("Failed to send message.");
                isActive = false;
            }
        }

        public void Send(string msg)
        {
            Send_to(msg);
        }

        private void HandleReceiveMessages()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (isActive)
                {
                    int bytesRead = server_stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    if (message.StartsWith("/new_client "))
                    {
                        // Obrađujemo samo poruke o novom klijentu
                        OnNewUser?.Invoke(message.Substring(12));
                    }
                    else if (message.StartsWith("/exit_client "))
                    {
                        // Obrada izlaska klijenta
                        OnExitUser?.Invoke(message.Substring(13));
                    }
                    else
                    {
                        // Obrađujemo ostale poruke
                        OnNewMessage?.Invoke(message);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Disconnected from server.");
            }

            isActive = false;
        }


        private void HandleSendMessages()
        {
            while (isActive)
            {
                string userMessage = Console.ReadLine() ?? string.Empty;
                if (EXIT_WORDS.Contains(userMessage.ToLower()))
                {
                    isActive = false;
                    break;
                }

            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Client_Network client = new Client_Network(IPAddress.Parse("127.0.0.1"), 5000, username);
        }
    }
}
