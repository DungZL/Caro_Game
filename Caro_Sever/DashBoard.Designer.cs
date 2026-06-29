namespace Caro_Sever
{
    partial class DashBoard
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
            Start = new Button();
            Log = new TextBox();
            IP = new TextBox();
            label2 = new Label();
            label1 = new Label();
            Port = new NumericUpDown();
            label3 = new Label();
            Status = new TextBox();
            ((System.ComponentModel.ISupportInitialize)Port).BeginInit();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Start.Location = new Point(12, 7);
            Start.Name = "Start";
            Start.Size = new Size(236, 42);
            Start.TabIndex = 0;
            Start.Text = "Khởi Chạy Sever";
            Start.UseVisualStyleBackColor = true;
            Start.Click += button1_Click;
            // 
            // Log
            // 
            Log.Location = new Point(269, 67);
            Log.Multiline = true;
            Log.Name = "Log";
            Log.Size = new Size(519, 351);
            Log.TabIndex = 1;
            // 
            // IP
            // 
            IP.Location = new Point(303, 11);
            IP.Name = "IP";
            IP.Size = new Size(125, 27);
            IP.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(269, 7);
            label2.Name = "label2";
            label2.Size = new Size(28, 28);
            label2.TabIndex = 9;
            label2.Text = "IP";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(531, 7);
            label1.Name = "label1";
            label1.Size = new Size(52, 28);
            label1.TabIndex = 8;
            label1.Text = "Port:";
            // 
            // Port
            // 
            Port.Location = new Point(589, 12);
            Port.Name = "Port";
            Port.Size = new Size(199, 27);
            Port.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 67);
            label3.Name = "label3";
            label3.Size = new Size(105, 28);
            label3.TabIndex = 12;
            label3.Text = "Trạng Thái:";
            // 
            // Status
            // 
            Status.Location = new Point(123, 71);
            Status.Name = "Status";
            Status.Size = new Size(125, 27);
            Status.TabIndex = 11;
            // 
            // DashBoard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(810, 450);
            Controls.Add(label3);
            Controls.Add(Status);
            Controls.Add(IP);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Port);
            Controls.Add(Log);
            Controls.Add(Start);
            Name = "DashBoard";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)Port).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Start;
        private TextBox Log;
        private TextBox IP;
        private Label label2;
        private Label label1;
        private NumericUpDown Port;
        private Label label3;
        private TextBox Status;
    }
}
