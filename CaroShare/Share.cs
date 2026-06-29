using System.Net.Sockets;
using System.Text;

namespace CaroShare
{
    // Class helper gui/nhan du lieu qua Socket
    public static class Share
    {
        // Gui chuoi qua socket
        public static void Send(Socket sck, string msg)
        {
            sck.Send(Encoding.UTF8.GetBytes(msg));
        }

        // Nhan chuoi tu socket
        public static string Receive(Socket sck)
        {
            byte[] data = new byte[1024];
            int size = sck.Receive(data);
            if (size == 0) return "";
            return Encoding.UTF8.GetString(data, 0, size);
        }
    }
}
