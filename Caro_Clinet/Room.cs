using System.Net;
using System.Net.Sockets;
using CaroShare;

namespace Caro_Clinet
{
    public partial class Room : Form
    {
        private Socket? sck;
        private bool isConnected = false;

        public Room()
        {
            InitializeComponent();
            Match.Enabled = false;
            Cancel.Enabled = false;
        }

        //Nut ket noi den server
        private void Connect_Click(object sender, EventArgs e)
        {
            if (isConnected) return;
            try
            {
                //Tao socket va ket noi
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP.Text), (int)Port.Value);
                sck.Connect(ep);
                isConnected = true;
                //Gui ten nguoi choi len server
                Share.Send(sck, "NAME|" + PlayerName.Text);

                Status.Text = "Da ket noi";
                Connect.Enabled = false;
                Match.Enabled = true;
                Cancel.Enabled = true;

                //Lang nghe phan hoi tu server
                Task.Run(() => ListenFromServer());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi ket noi: " + ex.Message);
            }
        }

        //Nut ghep
        private void Match_Click(object sender, EventArgs e)
        {
            if (!isConnected || sck == null) return;
            Share.Send(sck, "MATCH");
            Status.Text = "Dang cho ghep...";
        }

        //Nut huy
        private void Cancel_Click(object sender, EventArgs e)
        {
            if (!isConnected || sck == null) return;
            Share.Send(sck, "CANCEL");
            Status.Text = "Da huy ghep";
        }

        //Lang nghe phan hoi tu server
        private void ListenFromServer()
        {
            try
            {
                while (isConnected && sck != null && sck.Connected)
                {
                    string msg = Share.Receive(sck);
                    if (msg == "") break;

                    string[] parts = msg.Split('|');
                    string type = parts[0];

                    switch (type)
                    {
                        case "FOUND":
                            //Da tim thay doi thu: FOUND|TenMinh|TenDoiThu|1hoac0
                            this.Invoke(() =>
                            {
                                Status.Text = "Da ghep tran!";
                                string myName = parts[1];
                                string opponentName = parts[2];
                                bool isPlayer1 = parts[3] == "1";

                                Game_Board gameBoard = new Game_Board(sck, myName, opponentName, isPlayer1);
                                gameBoard.FormClosed += (s, args) =>
                                {
                                    this.Show();
                                    Status.Text = "Da ket noi";
                                    Task.Run(() => ListenFromServer());
                                };
                                this.Hide();
                                gameBoard.Show();
                            });
                            return; //Nhuong socket cho Game_Board

                        case "DISCONNECT":
                            this.Invoke(() =>
                            {
                                Status.Text = "Mat ket noi";
                                isConnected = false;
                                Connect.Enabled = true;
                                Match.Enabled = false;
                                Cancel.Enabled = false;
                            });
                            return;
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    this.Invoke(() =>
                    {
                        Status.Text = "Mat ket noi";
                        isConnected = false;
                        Connect.Enabled = true;
                        Match.Enabled = false;
                        Cancel.Enabled = false;
                    });
                }
                catch { }
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
