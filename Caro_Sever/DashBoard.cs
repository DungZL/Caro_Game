using System.Net;
using System.Net.Sockets;
using CaroShare;

namespace Caro_Sever
{
    public partial class DashBoard : Form
    {
        private Socket? sck;
        private bool isRunning = false;

        private Dictionary<Socket, string> clientNames = new Dictionary<Socket, string>();
        private Queue<Socket> matchQueue = new Queue<Socket>();
        private Dictionary<Socket, Socket> opponents = new Dictionary<Socket, Socket>();
        private object lockObj = new object();

        public DashBoard()
        {
            InitializeComponent();
        }

        //Ghi log len TextBox (thread-safe)
        private void WriteLog(string msg)
        {
            if (Log.InvokeRequired)
            {
                Log.Invoke(() => WriteLog(msg));
                return;
            }
            Log.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\r\n");
        }

        //Nut khoi chay server
        private void button1_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            try
            {
                //Tao socket, gan ket va lang nghe
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP.Text), (int)Port.Value);
                sck.Bind(ep);
                sck.Listen(5);
                isRunning = true;

                Status.Text = "Dang chay";
                WriteLog("Server da khoi chay tai " + IP.Text + ":" + (int)Port.Value);
                Start.Enabled = false;

                //Chay luong lang nghe client
                Task.Run(() => ListenForClients());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi khoi chay: " + ex.Message);
            }
        }

        //Lang nghe client ket noi
        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    Socket sckClient = sck!.Accept();
                    WriteLog("Mot client da ket noi.");
                    //Tao luong xu ly rieng cho moi client
                    Task.Run(() => HandleClient(sckClient));
                }
                catch (Exception)
                {
                    break; //Server da dung
                }
            }
        }

        //Xu ly tung client
        private void HandleClient(Socket sckClient)
        {
            try
            {
                while (sckClient.Connected)
                {
                    string msg = Share.Receive(sckClient);
                    if (msg == "") break;

                    string[] parts = msg.Split('|');
                    string type = parts[0];

                    switch (type)
                    {
                        case "NAME":
                            //Luu ten client
                            lock (lockObj)
                            {
                                clientNames[sckClient] = parts[1];
                            }
                            WriteLog("Nguoi choi \"" + parts[1] + "\" da dang ky.");
                            break;

                        case "MATCH":
                            HandleMatchRequest(sckClient);
                            break;

                        case "CANCEL":
                            HandleCancelMatch(sckClient);
                            break;

                        case "MOVE":
                        case "RESULT":
                            //Chuyen tiep sang doi thu
                            Socket? opp = null;
                            lock (lockObj)
                            {
                                opponents.TryGetValue(sckClient, out opp);
                            }
                            if (opp != null && opp.Connected)
                            {
                                try { Share.Send(opp, msg); } catch { }
                            }
                            break;

                        case "SURRENDER":
                            HandleSurrender(sckClient);
                            break;

                        case "DISCONNECT":
                            HandleDisconnect(sckClient);
                            return;
                    }
                }
            }
            catch (Exception)
            {
                //Client ngat ket noi dot ngot
            }
            finally
            {
                HandleDisconnect(sckClient);
            }
        }

        //Xu ly yeu cau ghep tran
        private void HandleMatchRequest(Socket client)
        {
            lock (lockObj)
            {
                string name = clientNames.ContainsKey(client) ? clientNames[client] : "Unknown";
                matchQueue.Enqueue(client);
                WriteLog("\"" + name + "\" yeu cau ghep tran. Hang doi: " + matchQueue.Count);

                //Neu co >= 2 nguoi -> ghep
                if (matchQueue.Count >= 2)
                {
                    Socket player1 = matchQueue.Dequeue();
                    Socket player2 = matchQueue.Dequeue();

                    //Luu cap doi thu
                    opponents[player1] = player2;
                    opponents[player2] = player1;

                    string name1 = clientNames.ContainsKey(player1) ? clientNames[player1] : "P1";
                    string name2 = clientNames.ContainsKey(player2) ? clientNames[player2] : "P2";

                    WriteLog("Ghep tran: \"" + name1 + "\" vs \"" + name2 + "\"");

                    //Gui cho Player1 (danh X, di truoc)
                    try { Share.Send(player1, "FOUND|" + name1 + "|" + name2 + "|1"); } catch { }
                    //Gui cho Player2 (danh O, di sau)
                    try { Share.Send(player2, "FOUND|" + name2 + "|" + name1 + "|0"); } catch { }
                }
            }
        }

        //Xu ly huy ghep tran
        private void HandleCancelMatch(Socket client)
        {
            lock (lockObj)
            {
                //Tao queue moi khong chua client nay
                Queue<Socket> newQueue = new Queue<Socket>();
                while (matchQueue.Count > 0)
                {
                    Socket c = matchQueue.Dequeue();
                    if (c != client) newQueue.Enqueue(c);
                }
                matchQueue = newQueue;

                string name = clientNames.ContainsKey(client) ? clientNames[client] : "Unknown";
                WriteLog("\"" + name + "\" da huy ghep tran.");
            }
        }

        //Xu ly dau hang
        private void HandleSurrender(Socket client)
        {
            Socket? opponent = null;
            string name;
            lock (lockObj)
            {
                opponents.TryGetValue(client, out opponent);
                name = clientNames.ContainsKey(client) ? clientNames[client] : "Unknown";
            }

            WriteLog("\"" + name + "\" da dau hang.");

            if (opponent != null && opponent.Connected)
            {
                try { Share.Send(opponent, "RESULT|Doi thu da dau hang. Ban thang!"); } catch { }
            }

            //Xoa cap doi thu
            lock (lockObj)
            {
                if (opponent != null) opponents.Remove(opponent);
                opponents.Remove(client);
            }
        }

        //Xu ly ngat ket noi
        private void HandleDisconnect(Socket client)
        {
            string name;
            Socket? opponent = null;

            lock (lockObj)
            {
                name = clientNames.ContainsKey(client) ? clientNames[client] : "Unknown";
                opponents.TryGetValue(client, out opponent);

                //Xoa khoi tat ca danh sach
                clientNames.Remove(client);
                opponents.Remove(client);

                //Xoa khoi hang doi ghep
                Queue<Socket> newQueue = new Queue<Socket>();
                while (matchQueue.Count > 0)
                {
                    Socket c = matchQueue.Dequeue();
                    if (c != client) newQueue.Enqueue(c);
                }
                matchQueue = newQueue;

                if (opponent != null) opponents.Remove(opponent);
            }

            WriteLog("\"" + name + "\" da ngat ket noi.");

            //Bao doi thu (neu dang choi)
            if (opponent != null && opponent.Connected)
            {
                try { Share.Send(opponent, "DISCONNECT"); } catch { }
            }

            try { client.Close(); } catch { }
        }

        private void label2_Click(object sender, EventArgs e) { }
    }
}
