using System.Net;
using Client;

namespace UI
{
    public partial class Form1 : Form
    {



        // Server
        private IPAddress ip_adress;
        private int port;
        //               username, line in textbox
        private Dictionary<string, int> Active_Users = new Dictionary<string, int>();

        // Client
        private Client_Network client;
        private string username;


        // Scene
        private Dictionary<string, Panel> scene = new Dictionary<string, Panel>();

        public Form1()
        {
            InitializeComponent();

            scene.Add("MainMenu", MainMenu);
            scene.Add("Chat", Caskanje);

            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Handle_New_Message(string msg)
        {
            Invoke((MethodInvoker)delegate {
                Chat_History.Text += $"{msg}\n"; 
            });
        }

        private void Handle_New_Client(string msg)
        {
            if (Active_Users.ContainsKey(msg)) return;
            else
            {
                Invoke((MethodInvoker)delegate {
                    Active_Users.Add(msg, Active_Users.Count);
                    Active_Users_textbox.Text += $"{msg}\n";  // Dodavanje korisnika u tekstbox
                });
            }

            
        }


        private void Handle_Exit_Client(string msg)
        {
            if (Active_Users.ContainsKey(msg))
            {
                int user_for_remove_index = Active_Users[msg];

                // Pomeri sve korisnike iznad jednog prema dolje
                foreach (var key in Active_Users.Keys.ToList())  // .ToList() da se izbegne modifikacija kolekcije tokom iteracije
                {
                    if (Active_Users[key] > user_for_remove_index)
                    {
                        Active_Users[key]--;
                    }
                }

                // Ukloni korisnika iz liste
                Invoke((MethodInvoker)delegate {
                    Active_Users.Remove(msg);
                    Active_Users_textbox.Text = string.Join("\n", Active_Users.Keys);  // Ponovo popuni TextBox
                });
            }
        }



        private void Promeni_Scenu(string scena)
        {
            foreach (var entry in scene)
            {
                entry.Value.Visible = (entry.Key == scena);
            }
        }



        private void Povezi_Se_Click(object sender, EventArgs e)
        {

            try
            {
                ip_adress = IPAddress.Parse(IP_textbox.Text);
                port = int.Parse(Port_textbox.Text);
                username = Username_textbox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;

            }


            try
            {
                client = new Client_Network(ip_adress, port, username);

                client.OnNewMessage += Handle_New_Message;
                client.OnNewUser += Handle_New_Client;
                client.OnExitUser += Handle_Exit_Client;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            



            Promeni_Scenu("Chat");

        }

        private void Posalji_Click(object sender, EventArgs e)
        {
            string poruka = (Poruka_textbox.Text).Trim();

            if (poruka == "")
            {
                return;
            }

            try
            {
                client.Send(poruka);
                Chat_History.Text += $"{client.username}: {poruka}\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Poruka_textbox.Clear();
        }

        private void Poruka_textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
