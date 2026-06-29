namespace Caro_Clinet
{
    partial class Room
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
            Connect = new Button();
            Match = new Button();
            Cancel = new Button();
            Port = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            IP = new TextBox();
            Status = new TextBox();
            label3 = new Label();
            PlayerName = new TextBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)Port).BeginInit();
            SuspendLayout();
            // 
            // Connect
            // 
            Connect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Connect.Location = new Point(12, 4);
            Connect.Name = "Connect";
            Connect.Size = new Size(236, 41);
            Connect.TabIndex = 0;
            Connect.Text = "Kết Nối Đến Sever";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += Connect_Click;
            // 
            // Match
            // 
            Match.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Match.Location = new Point(186, 181);
            Match.Name = "Match";
            Match.Size = new Size(134, 100);
            Match.TabIndex = 1;
            Match.Text = "Ghép";
            Match.UseVisualStyleBackColor = true;
            Match.Click += Match_Click;
            // 
            // Cancel
            // 
            Cancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Cancel.Location = new Point(460, 181);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(134, 99);
            Cancel.TabIndex = 2;
            Cancel.Text = "Huỷ";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // Port
            // 
            Port.Location = new Point(555, 12);
            Port.Name = "Port";
            Port.Size = new Size(199, 27);
            Port.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(485, 10);
            label1.Name = "label1";
            label1.Size = new Size(52, 28);
            label1.TabIndex = 4;
            label1.Text = "Port:";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(294, 11);
            label2.Name = "label2";
            label2.Size = new Size(28, 28);
            label2.TabIndex = 5;
            label2.Text = "IP";
            // 
            // IP
            // 
            IP.Location = new Point(341, 11);
            IP.Name = "IP";
            IP.Size = new Size(125, 27);
            IP.TabIndex = 6;
            // 
            // Status
            // 
            Status.Location = new Point(123, 64);
            Status.Name = "Status";
            Status.Size = new Size(125, 27);
            Status.TabIndex = 7;
            Status.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 60);
            label3.Name = "label3";
            label3.Size = new Size(105, 28);
            label3.TabIndex = 8;
            label3.Text = "Trạng Thái:";
            // 
            // PlayerName
            // 
            PlayerName.Location = new Point(469, 67);
            PlayerName.Name = "PlayerName";
            PlayerName.Size = new Size(200, 27);
            PlayerName.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(316, 66);
            label4.Name = "label4";
            label4.Size = new Size(147, 28);
            label4.TabIndex = 11;
            label4.Text = "Tên Người Chơi";
            // 
            // Room
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(PlayerName);
            Controls.Add(label3);
            Controls.Add(Status);
            Controls.Add(IP);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Port);
            Controls.Add(Cancel);
            Controls.Add(Match);
            Controls.Add(Connect);
            Name = "Room";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)Port).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Connect;
        private Button Match;
        private Button Cancel;
        private NumericUpDown Port;
        private Label label1;
        private Label label2;
        private TextBox IP;
        private TextBox Status;
        private Label label3;
        private TextBox PlayerName;
        private Label label4;
    }
}
