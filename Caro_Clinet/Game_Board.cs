using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;
using CaroShare;

namespace Caro_Clinet
{
    public partial class Game_Board : Form
    {
        private const int BOARD_SIZE = 15;
        private const int CELL_SIZE = 38;
        private const int TURN_TIME = 30;

        private Socket sck;
        private Button[,] cells = new Button[BOARD_SIZE, BOARD_SIZE];
        private int[,] board = new int[BOARD_SIZE, BOARD_SIZE]; // 0:trong, 1:X, 2:O
        private bool isPlayer1;
        private bool isMyTurn;
        private bool gameOver = false;
        private int timeLeft;

        public Game_Board(Socket sck, string myName, string opponentName, bool isPlayer1)
        {
            InitializeComponent();
            this.sck = sck;
            this.isPlayer1 = isPlayer1;
            this.isMyTurn = isPlayer1; // Player1 (X) di truoc

            name_1.Text = myName;
            Name_2.Text = opponentName;

            //Tao ban co
            for (int y = 0; y < BOARD_SIZE; y++)
            {
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(CELL_SIZE, CELL_SIZE);
                    btn.Location = new Point(x * CELL_SIZE, y * CELL_SIZE);
                    btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    btn.Tag = new int[] { x, y };
                    btn.Click += Cell_Click;
                    btn.FlatStyle = FlatStyle.Flat;
                    cells[x, y] = btn;
                    Board.Controls.Add(btn);
                }
            }

            //Cai dat timer
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            Timer.Maximum = TURN_TIME;
            Timer.Value = TURN_TIME;
            timeLeft = TURN_TIME;
            if (isMyTurn) timer1.Start();

            //Lang nghe du lieu tu server
            Task.Run(() => ListenFromServer());
        }

        //Xu ly click o co
        private void Cell_Click(object? sender, EventArgs e)
        {
            if (gameOver || !isMyTurn) return;

            Button btn = (Button)sender!;
            int[] pos = (int[])btn.Tag!;
            int x = pos[0];
            int y = pos[1];
            if (board[x, y] != 0) return;

            //Danh dau o
            int mark = isPlayer1 ? 1 : 2;
            board[x, y] = mark;
            btn.Text = isPlayer1 ? "X" : "O";
            btn.ForeColor = isPlayer1 ? Color.Red : Color.Blue;
            btn.Enabled = false;

            //Gui nuoc di toi server
            try { Share.Send(sck, "MOVE|" + x + "|" + y); } catch { }

            //Kiem tra thang
            if (CheckWin(x, y, mark))
            {
                gameOver = true;
                timer1.Stop();
                try { Share.Send(sck, "RESULT|Ban da thua!"); } catch { }
                MessageBox.Show("Ban da thang!", "Ket qua", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            //Chuyen luot
            isMyTurn = false;
            timer1.Stop();
            timeLeft = TURN_TIME;
            Timer.Value = TURN_TIME;
        }

        //Lang nghe du lieu tu server
        private void ListenFromServer()
        {
            try
            {
                while (!gameOver)
                {
                    string msg = Share.Receive(sck);
                    if (msg == "") break;

                    string[] parts = msg.Split('|');
                    string type = parts[0];

                    switch (type)
                    {
                        case "MOVE":
                            //Doi thu danh
                            this.Invoke(() =>
                            {
                                int ox = int.Parse(parts[1]);
                                int oy = int.Parse(parts[2]);
                                int mark = isPlayer1 ? 2 : 1;
                                board[ox, oy] = mark;
                                Button btn = cells[ox, oy];
                                btn.Text = isPlayer1 ? "O" : "X";
                                btn.ForeColor = isPlayer1 ? Color.Blue : Color.Red;
                                btn.Enabled = false;
                                //Den luot minh
                                isMyTurn = true;
                                timeLeft = TURN_TIME;
                                Timer.Value = TURN_TIME;
                                timer1.Start();
                            });
                            break;

                        case "RESULT":
                            //Doi thu thang -> minh thua
                            this.Invoke(() =>
                            {
                                gameOver = true;
                                timer1.Stop();
                                MessageBox.Show(parts[1], "Ket qua", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            });
                            break;

                        case "DISCONNECT":
                            //Doi thu thoat
                            this.Invoke(() =>
                            {
                                gameOver = true;
                                timer1.Stop();
                                MessageBox.Show("Doi thu da thoat. Ban thang!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            });
                            break;
                    }
                }
            }
            catch (Exception)
            {
                if (!gameOver)
                {
                    try
                    {
                        this.Invoke(() =>
                        {
                            gameOver = true;
                            timer1.Stop();
                            MessageBox.Show("Mat ket noi voi server.", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                    catch { }
                }
            }
        }

        //Kiem tra thang: 5 o lien tiep theo 4 huong
        private bool CheckWin(int x, int y, int mark)
        {
            int[][] dx = { new[] { 1, 0 }, new[] { 0, 1 }, new[] { 1, 1 }, new[] { 1, -1 } };

            for (int dir = 0; dir < 4; dir++)
            {
                int count = 1;
                //Dem ve mot phia
                for (int i = 1; i < 5; i++)
                {
                    int nx = x + dx[dir][0] * i;
                    int ny = y + dx[dir][1] * i;
                    if (nx >= 0 && nx < BOARD_SIZE && ny >= 0 && ny < BOARD_SIZE && board[nx, ny] == mark)
                        count++;
                    else break;
                }
                //Dem ve phia nguoc lai
                for (int i = 1; i < 5; i++)
                {
                    int nx = x - dx[dir][0] * i;
                    int ny = y - dx[dir][1] * i;
                    if (nx >= 0 && nx < BOARD_SIZE && ny >= 0 && ny < BOARD_SIZE && board[nx, ny] == mark)
                        count++;
                    else break;
                }
                if (count >= 5) return true;
            }
            return false;
        }

        //Timer dem nguoc
        private void Timer1_Tick(object? sender, EventArgs e)
        {
            timeLeft--;
            Timer.Value = Math.Max(timeLeft, 0);
            if (timeLeft <= 0)
            {
                timer1.Stop();
                gameOver = true;
                try { Share.Send(sck, "RESULT|Doi thu het gio. Ban thang!"); } catch { }
                MessageBox.Show("Het gio! Ban da thua.", "Ket qua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        //Nut dau hang
        private void Surrender_Click(object sender, EventArgs e)
        {
            if (gameOver) return;
            DialogResult result = MessageBox.Show("Ban co chac muon dau hang?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                gameOver = true;
                timer1.Stop();
                try { Share.Send(sck, "SURRENDER"); } catch { }
                MessageBox.Show("Ban da dau hang.", "Ket qua", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
