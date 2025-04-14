using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static string address = "127.0.0.1";
    static int port = 4040; // 1000 - 65000
    private static void Main(string[] args)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Socket listenSocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);

        try
        {
            listenSocket.Bind(endPoint);
            Console.WriteLine("Server started! Waiting connection");
            while (true)
            {
                byte[] data = new byte[1024];
                listenSocket.ReceiveFrom(data, ref remoteEndPoint);

                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} :: {message} from {remoteEndPoint}");


                //////
                message = "Message was send!";
                data = Encoding.Unicode.GetBytes(message);
                listenSocket.SendTo(data, remoteEndPoint);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        listenSocket.Shutdown(SocketShutdown.Both);
        listenSocket.Close();
    }
}