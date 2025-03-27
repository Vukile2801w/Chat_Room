using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;

namespace Client
{
    public class Client_Network
    {
        private readonly List<string> EXIT_WORDS = new List<string> { "exit", "quit" };
        private bool isActive = false;
        public string username = "New User";
        private string userMessage = "";
        public readonly Encoding default_encoder = Encoding.UTF8;

        private NetworkStream server_stream;


        // Eventss
        public delegate void Primljena_Poruka(string message);
        public delegate void Novi_Korisnik(string message);
        public delegate void Uklonjen_Korisnik(string message);

        public static event Primljena_Poruka OnNewMessage;
        public static event Novi_Korisnik OnNewUser;
        public static event Uklonjen_Korisnik OnExitUser;



        static void Main(string[] args)
        {
            
        }



        public Client_Network(IPAddress ip, int port, string username)
        {
            IPAddress ipAddress = ip;


            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            TcpClient client = new TcpClient();

            Console.WriteLine("Connecting to server...");
            client.Connect(ipEndPoint);
            Console.WriteLine("Connected!");





            NetworkStream stream = client.GetStream();
            server_stream = stream;

            if (stream != null)
            {
                isActive = true;
            }

            Send_to(server_stream, $"/username set {username}", default_encoder);

            byte[] buffer = new byte[1024];
            username = Recv_from(buffer, server_stream.Read(buffer, 0, buffer.Length), default_encoder);
            Console.WriteLine(username);


            Task receiveTask = Task.Run(() => HandleReceiveMessages(server_stream));
            Task sendTask = Task.Run(() => HandleSendMessages(server_stream));

            Task.WaitAny(receiveTask, sendTask);

            Console.WriteLine("Closing connection...");

            server_stream.Close();
            client.Close();
        }



        private void Send_to(NetworkStream stream, string msg, Encoding encoder)
        {
            try
            {
                if (stream == null || !stream.CanWrite)
                {
                    Console.WriteLine("Error: Cannot send message, connection is closed.");
                    return;
                }

                byte[] encoded_msg = encoder.GetBytes(msg);
                stream.Write(encoded_msg, 0, encoded_msg.Length);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("Error: Tried to use a disposed NetworkStream - " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error while sending: " + ex.Message);
            }
        }


        public void Send(string msg)
        {
            if (server_stream == null || !server_stream.CanWrite)
            {
                Console.WriteLine("Error: Cannot send message, connection is closed.");
                return;
            }

            Send_to(server_stream, msg, default_encoder);
        }


        private string Recv_from(byte[] buffer, int bytesRead, Encoding encoder)
        {
            return encoder.GetString(buffer, 0, bytesRead);
        }



        void HandleReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (isActive)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;


                    string message = Recv_from(buffer, bytesRead, default_encoder);

                    switch (message.Split(' ')[0])
                    {
                        case "/new_client":
                            OnNewUser?.Invoke(message.Split(' ')[1]);
                            break;
                        case "/exit_client":
                            OnExitUser?.Invoke(message.Split(' ')[1]);
                            break;

                        default:
                            OnNewMessage?.Invoke(message);
                            break;
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Connection lost.");
            }

            isActive = false;
        }


        void HandleSendMessages(NetworkStream stream)
        {
            try
            {
                while (isActive)
                {

                    userMessage = "";
                    int write_index = 0;

                    Console.Write($"{username}: ");



                    while (true)
                    {


                        // Uzimamo jedan pritisnuti taster
                        ConsoleKeyInfo key = Console.ReadKey(intercept: true); // intercept: true da ne prikaže taster
                        if (key.Key == ConsoleKey.Enter)
                        { // Ako je pritisnut Enter, izlazimo iz petlje
                            Console.Write("\n");
                            break;
                        }

                        if (key.Key == ConsoleKey.Backspace)
                        {
                            int minCursorPos = $"{username}: ".Length; // Minimalna dozvoljena pozicija kursora

                            if (Console.GetCursorPosition().Left > minCursorPos) // Proverava da li nismo na početku poruke
                            {
                                try
                                {
                                    // Pomera kursor nazad
                                    Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.CursorTop);

                                    // Briše karakter ispisivanjem razmaka
                                    Console.Write(" ");

                                    // Vraća kursor nazad kako bi izgledalo kao da je karakter obrisan
                                    //Console.SetCursorPosition(Console.GetCursorPosition().Left - 1, Console.CursorTop);

                                    string userMessage_copy = userMessage;
                                    userMessage = "";

                                    // Briše poslednji karakter iz userMessage stringa
                                    for (int i = 0; i < userMessage_copy.Length; i++)
                                    {
                                        userMessage += userMessage_copy[i];
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Greška pri obradi Backspace-a: {ex.Message}");
                                }
                            }
                        }


                        // Dodajemo pritisnut karakter u string
                        //userMessage[write_index] = key.KeyChar;
                        write_index++;
                        // Prikazujemo pritisnuti karakter odmah
                        Console.Write(key.KeyChar);
                    }

                    if (string.IsNullOrWhiteSpace(userMessage))
                        continue;

                    if (EXIT_WORDS.Contains(userMessage.ToLower()))
                    {
                        isActive = false;
                        break;
                    }

                    Send_to(stream, userMessage, default_encoder);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error sending message.");
            }

            isActive = false;
        }

    }
}