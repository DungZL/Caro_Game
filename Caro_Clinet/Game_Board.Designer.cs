namespace Caro_Clinet
{
    partial class Game_Board
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
            components = new System.ComponentModel.Container();
            Board = new Panel();
            label1 = new Label();
            name_1 = new TextBox();
            Name_2 = new TextBox();
            label2 = new Label();
            Timer = new ProgressBar();
            Surrender = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // Board
            // 
            Board.Location = new Point(143, 49);
            Board.Name = "Board";
            Board.Size = new Size(777, 577);
            Board.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(73, 28);
            label1.TabIndex = 2;
            label1.Text = "Name: ";
            // 
            // name_1
            // 
            name_1.Location = new Point(91, 12);
            name_1.Name = "name_1";
            name_1.Size = new Size(163, 27);
            name_1.TabIndex = 3;
            // 
            // Name_2
            // 
            Name_2.Location = new Point(911, 12);
            Name_2.Name = "Name_2";
            Name_2.Size = new Size(163, 27);
            Name_2.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(832, 8);
            label2.Name = "label2";
            label2.Size = new Size(73, 28);
            label2.TabIndex = 4;
            label2.Text = "Name: ";
            // 
            // Timer
            // 
            Timer.Location = new Point(409, 12);
            Timer.Name = "Timer";
            Timer.Size = new Size(290, 32);
            Timer.TabIndex = 6;
            // 
            // Surrender
            // 
            Surrender.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Surrender.Location = new Point(954, 482);
            Surrender.Name = "Surrender";
            Surrender.Size = new Size(120, 53);
            Surrender.TabIndex = 7;
            Surrender.Text = "Đầu Hàng";
            Surrender.UseVisualStyleBackColor = true;
            Surrender.Click += Surrender_Click;
            // 
            // Game_Board
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1098, 675);
            Controls.Add(Surrender);
            Controls.Add(Timer);
            Controls.Add(Name_2);
            Controls.Add(label2);
            Controls.Add(name_1);
            Controls.Add(label1);
            Controls.Add(Board);
            Name = "Game_Board";
            Text = "Game_Board";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel Board;
        private Label label1;
        private TextBox name_1;
        private TextBox Name_2;
        private Label label2;
        private ProgressBar Timer;
        private Button Surrender;
        private System.Windows.Forms.Timer timer1;
    }
}