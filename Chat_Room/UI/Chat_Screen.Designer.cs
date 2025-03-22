namespace UI
{
    partial class Chat_Screen
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
            richTextBox1 = new RichTextBox();
            textBox1 = new TextBox();
            richTextBox2 = new RichTextBox();
            Chat_Name_Lable = new Label();
            label1 = new Label();
            Posalji_Button = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(49, 35);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(826, 589);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // textBox1
            // 
            textBox1.ForeColor = SystemColors.ControlDark;
            textBox1.Location = new Point(49, 630);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(717, 23);
            textBox1.TabIndex = 2;
            textBox1.Text = "Unesite vasu poruku";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(881, 35);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(140, 619);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // Chat_Name_Lable
            // 
            Chat_Name_Lable.AutoSize = true;
            Chat_Name_Lable.Location = new Point(60, 26);
            Chat_Name_Lable.Name = "Chat_Name_Lable";
            Chat_Name_Lable.Size = new Size(54, 15);
            Chat_Name_Lable.TabIndex = 4;
            Chat_Name_Lable.Text = "Ćaskanje";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(891, 26);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 5;
            label1.Text = "Povezani Klijenti";
            // 
            // Posalji_Button
            // 
            Posalji_Button.Location = new Point(772, 630);
            Posalji_Button.Name = "Posalji_Button";
            Posalji_Button.Size = new Size(103, 23);
            Posalji_Button.TabIndex = 6;
            Posalji_Button.Text = "Posalji";
            Posalji_Button.UseVisualStyleBackColor = true;
            Posalji_Button.Click += button1_Click;
            // 
            // Chat_Screen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1048, 681);
            Controls.Add(Posalji_Button);
            Controls.Add(label1);
            Controls.Add(Chat_Name_Lable);
            Controls.Add(richTextBox2);
            Controls.Add(textBox1);
            Controls.Add(richTextBox1);
            Name = "Chat_Screen";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox richTextBox1;
        private TextBox textBox1;
        private RichTextBox richTextBox2;
        private Label Chat_Name_Lable;
        private Label label1;
        private Button Posalji_Button;
    }
}
