using Calculate_data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private string exapmle = "";
        TcpClient client;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        NetworkStream ns;
        StreamWriter sw;
        StreamReader sr;
        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            try
            {
                client.Connect(endPoint);
                ns = client.GetStream();
                sw = new StreamWriter(ns);
            }
            catch (Exception ex)
            {

            }
            Listener();
        }

        private void GetValueBtn(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Content.ToString() != "=")
            {
                exapmle += (sender as Button)?.Content.ToString();
                txtBox.Text = exapmle;
            }
            else
            {
                Reg task = new Reg();
                var numbers = exapmle.Split(new char[] { '+', '-', '*', '/' });
                task.One = int.Parse(numbers[0]);
                task.Two = int.Parse(numbers[1]);
                task.Operation = getOperation(exapmle);

                MessageBox.Show($"{task.One} {task.Operation} {task.Two}");
                SendObj(task);
            }

        }
        private string getOperation(string example)
        {
            foreach (var item in exapmle)
            {
                if (!Char.IsDigit(item))
                    return item.ToString();
            }
            return null;
        }
        private void SendObj(Reg reg)
        {
            try
            { 

                sw.WriteLine(JsonSerializer.Serialize(reg));
                sw.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
        private async void Listener()
        {
            while (true)
            {
                try
                {
                    string? message = await sr.ReadLineAsync();
                    MessageBox.Show(message);
                    
                }
                catch (Exception ex)
                {
                    break;
                }

            }
        }

    }
}
