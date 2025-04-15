namespace UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Caskanje = new Panel();
            label2 = new Label();
            label1 = new Label();
            Posalji = new Button();
            Chat_History = new RichTextBox();
            Active_Users_textbox = new RichTextBox();
            Poruka_textbox = new TextBox();
            MainMenu = new Panel();
            Povezi_Se = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            Username_textbox = new TextBox();
            IP_textbox = new TextBox();
            Port_textbox = new TextBox();
            Caskanje.SuspendLayout();
            MainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // Caskanje
            // 
            Caskanje.Controls.Add(label2);
            Caskanje.Controls.Add(label1);
            Caskanje.Controls.Add(Posalji);
            Caskanje.Controls.Add(Chat_History);
            Caskanje.Controls.Add(Active_Users_textbox);
            Caskanje.Controls.Add(Poruka_textbox);
            Caskanje.Dock = DockStyle.Fill;
            Caskanje.Location = new Point(0, 0);
            Caskanje.Name = "Caskanje";
            Caskanje.Size = new Size(944, 681);
            Caskanje.TabIndex = 0;
            Caskanje.Visible = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(721, 9);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 5;
            label2.Text = "Aktivni";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 9);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 4;
            label1.Text = "Ćaskanje";
            // 
            // Posalji
            // 
            Posalji.Location = new Point(608, 590);
            Posalji.Name = "Posalji";
            Posalji.Size = new Size(88, 23);
            Posalji.TabIndex = 3;
            Posalji.Text = "Pošalji";
            Posalji.UseVisualStyleBackColor = true;
            Posalji.Click += Posalji_Click;
            // 
            // Chat_History
            // 
            Chat_History.Location = new Point(20, 18);
            Chat_History.Name = "Chat_History";
            Chat_History.Size = new Size(676, 566);
            Chat_History.TabIndex = 2;
            Chat_History.Text = "";
            // 
            // Active_Users_textbox
            // 
            Active_Users_textbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Active_Users_textbox.Location = new Point(702, 18);
            Active_Users_textbox.Name = "Active_Users_textbox";
            Active_Users_textbox.Size = new Size(172, 595);
            Active_Users_textbox.TabIndex = 1;
            Active_Users_textbox.Text = "";
            // 
            // Poruka_textbox
            // 
            Poruka_textbox.BorderStyle = BorderStyle.None;
            Poruka_textbox.Location = new Point(20, 590);
            Poruka_textbox.Name = "Poruka_textbox";
            Poruka_textbox.Size = new Size(576, 16);
            Poruka_textbox.TabIndex = 0;
            // 
            // MainMenu
            // 
            MainMenu.Controls.Add(Povezi_Se);
            MainMenu.Controls.Add(label5);
            MainMenu.Controls.Add(label4);
            MainMenu.Controls.Add(label3);
            MainMenu.Controls.Add(Username_textbox);
            MainMenu.Controls.Add(IP_textbox);
            MainMenu.Controls.Add(Port_textbox);
            MainMenu.Dock = DockStyle.Fill;
            MainMenu.Location = new Point(0, 0);
            MainMenu.Name = "MainMenu";
            MainMenu.Size = new Size(944, 681);
            MainMenu.TabIndex = 8;
            // 
            // Povezi_Se
            // 
            Povezi_Se.Location = new Point(368, 348);
            Povezi_Se.Name = "Povezi_Se";
            Povezi_Se.Size = new Size(155, 23);
            Povezi_Se.TabIndex = 4;
            Povezi_Se.Text = "Poveži se";
            Povezi_Se.UseVisualStyleBackColor = true;
            Povezi_Se.Click += Povezi_Se_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(404, 310);
            label5.Name = "label5";
            label5.Size = new Size(85, 15);
            label5.TabIndex = 6;
            label5.Text = "Korisničko ime";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(485, 275);
            label4.Name = "label4";
            label4.Size = new Size(29, 15);
            label4.TabIndex = 3;
            label4.Text = "port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(394, 275);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 2;
            label3.Text = "IP adresa";
            // 
            // Username_textbox
            // 
            Username_textbox.Location = new Point(367, 319);
            Username_textbox.Name = "Username_textbox";
            Username_textbox.PlaceholderText = "User0";
            Username_textbox.Size = new Size(156, 23);
            Username_textbox.TabIndex = 5;
            Username_textbox.TextChanged += Username_textbox_TextChanged;
            // 
            // IP_textbox
            // 
            IP_textbox.Location = new Point(369, 284);
            IP_textbox.Name = "IP_textbox";
            IP_textbox.PlaceholderText = "192.168._._";
            IP_textbox.Size = new Size(100, 23);
            IP_textbox.TabIndex = 0;
            // 
            // Port_textbox
            // 
            Port_textbox.Location = new Point(475, 284);
            Port_textbox.Name = "Port_textbox";
            Port_textbox.PlaceholderText = "5050";
            Port_textbox.Size = new Size(49, 23);
            Port_textbox.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 681);
            Controls.Add(MainMenu);
            Controls.Add(Caskanje);
            Name = "Form1";
            Text = "Chat Room";
            Load += Form1_Load;
            Caskanje.ResumeLayout(false);
            Caskanje.PerformLayout();
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel Caskanje;
        private Label label2;
        private Label label1;
        private Button Posalji;
        private RichTextBox Chat_History;
        private RichTextBox Active_Users_textbox;
        private TextBox Poruka_textbox;
        private Panel MainMenu;
        private TextBox Port_textbox;
        private TextBox IP_textbox;
        private Button Povezi_Se;
        private Label label4;
        private Label label3;
        private Label label5;
        private TextBox Username_textbox;
    }
}
