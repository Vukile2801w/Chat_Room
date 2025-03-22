using Client;

namespace UI
{
    public partial class Chat_Screen : Form
    {

        Client_Network client_network;


        public Chat_Screen()
        {
            InitializeComponent();



            client_network = new Client_Network();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string msg = textBox1.Text;

            //client_network.Send_to(, msg, Client_Network.default_encoder);

            textBox1.Clear();


        }
    }
}
