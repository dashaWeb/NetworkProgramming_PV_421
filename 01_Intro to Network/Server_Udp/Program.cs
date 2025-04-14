using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Program
{
    static string address = "127.0.0.1";
    static int port = 4040; // 1000 - 65000
    private static void Main(string[] args)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        UdpClient listener = new UdpClient(endPoint);

        try
        {
            Console.WriteLine("Server started! Waiting connection");
            byte[] data;
            while (true)
            {
                data = listener.Receive(ref remoteEndPoint);
                

                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()} :: {message} from {remoteEndPoint}");


                //////
                message = "Message was send!";
                data = Encoding.Unicode.GetBytes(message);
                listener.Send(data, data.Length, remoteEndPoint);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

       listener.Close();
    }
}