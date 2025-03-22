using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;

namespace Client
{
    public class Client_Network
    {
        private static readonly List<string> EXIT_WORDS = new List<string> { "exit", "quit" };
        private static bool isActive = false;
        private static string username = "New User";
        private static string userMessage = "";
        public static readonly Encoding default_encoder = Encoding.UTF8;

        
        static void Main(string[] args)
        {
            

        }
        


        public Client_Network()
        {
            Console.Write("Input address and port to connect to server\nAddr: ");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Console.ReadLine()

            Console.Write("Port: ");
            int port = int.Parse("5050");  // Console.ReadLine()

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            TcpClient client = new TcpClient();

            Console.WriteLine("Connecting to server...");
            client.Connect(ipEndPoint);
            Console.WriteLine("Connected!");


            Console.WriteLine("To set your username type: \n\b/username set {username}\b");




            NetworkStream stream = client.GetStream();


            if (stream != null)
            {
                isActive = true;
            }

            Send_to(stream, "/username get -nr", default_encoder);

            byte[] buffer = new byte[1024];
            username = Recv_from(buffer, stream.Read(buffer, 0, buffer.Length), default_encoder);
            Console.WriteLine(username);


            Task receiveTask = Task.Run(() => HandleReceiveMessages(stream));
            Task sendTask = Task.Run(() => HandleSendMessages(stream));

            Task.WaitAny(receiveTask, sendTask);

            Console.WriteLine("Closing connection...");

            stream.Close();
            client.Close();
        }



        private static void Send_to(NetworkStream stream, string msg, Encoding encoder)
        {
            string packet = msg;

            byte[] encoded_msg = encoder.GetBytes(packet);
            stream.Write(encoded_msg, 0, encoded_msg.Length);
        }

        private static string Recv_from(byte[] buffer, int bytesRead, Encoding encoder)
        {
            return encoder.GetString(buffer, 0, bytesRead);
        }



        static void HandleReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (isActive)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    int lastLine = Console.CursorTop;
                    Console.SetCursorPosition(0, lastLine);

                    // Prekrij liniju praznim karakterima
                    Console.Write(new string(' ', Console.WindowWidth));

                    // Vrati kursor na početak obrisane linije (ne mora ako ne želiš da pišeš ponovo)
                    Console.SetCursorPosition(0, lastLine);

                    string message = Recv_from(buffer, bytesRead, default_encoder);
                    Console.WriteLine(message.Trim());  // Trim da ne dodaje dodatne prazne redove
                    Console.Write($"{username}: {userMessage}");  // Ispis za sledeći unos


                }
            }
            catch (Exception)
            {
                Console.WriteLine("Connection lost.");
            }

            isActive = false;
        }


        static void HandleSendMessages(NetworkStream stream)
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