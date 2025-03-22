namespace UI
{
    partial class Main_Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            User_Name = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.ForeColor = Color.LightGray;
            textBox1.Location = new Point(358, 151);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(299, 23);
            textBox1.TabIndex = 0;
            textBox1.Text = "Unesite Kod Sobe";
            // 
            // User_Name
            // 
            User_Name.ForeColor = Color.LightGray;
            User_Name.Location = new Point(359, 180);
            User_Name.Name = "User_Name";
            User_Name.Size = new Size(298, 23);
            User_Name.TabIndex = 1;
            User_Name.Text = "Unesite Korisnicko Ime";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 32F);
            label1.Location = new Point(395, 51);
            label1.Name = "label1";
            label1.Size = new Size(237, 59);
            label1.TabIndex = 2;
            label1.Text = "Chat Room";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Main_Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1048, 681);
            Controls.Add(label1);
            Controls.Add(User_Name);
            Controls.Add(textBox1);
            Name = "Main_Menu";
            Text = "Main_Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox User_Name;
        private Label label1;
    }
}