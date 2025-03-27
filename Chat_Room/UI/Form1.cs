using System.Net;
using Client;

namespace UI
{
    public partial class Form1 : Form
    {

        // Server
        private IPAddress ip_adress;
        private int port;

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

            Client_Network.OnNewMessage += (msg) => { Chat_History.Text += $"{msg}\n"; };
            Client_Network.OnNewUser += (msg) => { Active_Users_textbox.Text += $"{msg}\n"; };
            Client_Network.OnExitUser += (msg) => {
                foreach (string user in Active_Users_textbox.Text.Split('\n'))
                {
                    if (user == msg)
                    {
                        Active_Users_textbox.Text.Replace(user, "");
                    }
                }
            };

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Promeni_Scenu(string scena)
        {
            foreach (Panel panel in scene.Values)
            {
                if (panel.Name == scene[scena].Name)
                {
                    panel.Visible = true;
                }
                else
                {
                    panel.Visible = false;
                }
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
            string poruka = Poruka_textbox.Text;

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

            Port_textbox.Clear();
        }

        private void Poruka_textbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
