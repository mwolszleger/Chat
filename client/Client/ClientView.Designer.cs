namespace Client
{
    partial class ClientView
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
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.buttonLogOut = new System.Windows.Forms.Button();
            this.listBoxLogged = new System.Windows.Forms.ListBox();
            this.listBoxNotLogged = new System.Windows.Forms.ListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelLogin = new System.Windows.Forms.Label();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.BackColor = System.Drawing.Color.White;
            this.buttonLogIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonLogIn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonLogIn.FlatAppearance.BorderSize = 0;
            this.buttonLogIn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.buttonLogIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.buttonLogIn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.buttonLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonLogIn.Location = new System.Drawing.Point(47, 211);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(160, 42);
            this.buttonLogIn.TabIndex = 2;
            this.buttonLogIn.Text = "zaloguj";
            this.buttonLogIn.UseVisualStyleBackColor = false;
            this.buttonLogIn.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(330, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "hasło:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(333, 163);
            this.textBoxPassword.MaxLength = 40;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(160, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.UseSystemPasswordChar = true;
            this.textBoxPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox4_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(330, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "login:";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(333, 119);
            this.textBoxLogin.MaxLength = 40;
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(160, 20);
            this.textBoxLogin.TabIndex = 0;
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.BackColor = System.Drawing.Color.White;
            this.buttonLogOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonLogOut.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonLogOut.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.buttonLogOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.buttonLogOut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.buttonLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonLogOut.Location = new System.Drawing.Point(47, 307);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(160, 42);
            this.buttonLogOut.TabIndex = 4;
            this.buttonLogOut.Text = "wyloguj";
            this.buttonLogOut.UseVisualStyleBackColor = false;
            this.buttonLogOut.Visible = false;
            this.buttonLogOut.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // listBoxLogged
            // 
            this.listBoxLogged.BackColor = System.Drawing.Color.Silver;
            this.listBoxLogged.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxLogged.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxLogged.FormattingEnabled = true;
            this.listBoxLogged.ItemHeight = 16;
            this.listBoxLogged.Location = new System.Drawing.Point(11, 32);
            this.listBoxLogged.Name = "listBoxLogged";
            this.listBoxLogged.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxLogged.Size = new System.Drawing.Size(254, 112);
            this.listBoxLogged.TabIndex = 12;
            this.listBoxLogged.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // listBoxNotLogged
            // 
            this.listBoxNotLogged.BackColor = System.Drawing.Color.Silver;
            this.listBoxNotLogged.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxNotLogged.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxNotLogged.FormattingEnabled = true;
            this.listBoxNotLogged.ItemHeight = 16;
            this.listBoxNotLogged.Location = new System.Drawing.Point(13, 223);
            this.listBoxNotLogged.Name = "listBoxNotLogged";
            this.listBoxNotLogged.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxNotLogged.Size = new System.Drawing.Size(253, 112);
            this.listBoxNotLogged.TabIndex = 13;
            this.listBoxNotLogged.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Silver;
            this.buttonStart.FlatAppearance.BorderSize = 0;
            this.buttonStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonStart.Location = new System.Drawing.Point(12, 155);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(254, 35);
            this.buttonStart.TabIndex = 14;
            this.buttonStart.Text = "Rozpocznij rozmowę";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(8, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "Zalogowani użytkownicy:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "Niezalogowani użytkownicy:";
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.BackColor = System.Drawing.Color.Silver;
            this.labelLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLogin.Location = new System.Drawing.Point(330, 23);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(44, 18);
            this.labelLogin.TabIndex = 17;
            this.labelLogin.Text = "login";
            this.labelLogin.Visible = false;
            // 
            // buttonRegister
            // 
            this.buttonRegister.BackColor = System.Drawing.Color.White;
            this.buttonRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonRegister.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonRegister.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.buttonRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.buttonRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.buttonRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRegister.Location = new System.Drawing.Point(47, 259);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(160, 42);
            this.buttonRegister.TabIndex = 3;
            this.buttonRegister.Text = "zarejestruj";
            this.buttonRegister.UseVisualStyleBackColor = false;
            this.buttonRegister.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.listBoxLogged);
            this.panel1.Controls.Add(this.listBoxNotLogged);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 352);
            this.panel1.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.buttonLogIn);
            this.panel2.Controls.Add(this.buttonLogOut);
            this.panel2.Controls.Add(this.buttonRegister);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(286, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 352);
            this.panel2.TabIndex = 20;
            // 
            // ClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(538, 352);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MaximumSize = new System.Drawing.Size(554, 391);
            this.MinimumSize = new System.Drawing.Size(554, 391);
            this.Name = "ClientView";
            this.Text = "Chat";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientView_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonLogIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.Button buttonLogOut;
        private System.Windows.Forms.ListBox listBoxLogged;
        private System.Windows.Forms.ListBox listBoxNotLogged;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}

