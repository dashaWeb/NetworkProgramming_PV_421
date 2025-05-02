using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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

namespace Smtp_sendMessage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string server = "smtp.gmail.com";
        const short port = 587;
        const string username = "dashakonopelko@gmail.com";
        const string password = "vlkm eqto wiib dejp";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {

        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            MailMessage message = new MailMessage(username, toBox.Text, themeBox.Text, GetRichText(messageBox));

            using (StreamReader sr = new StreamReader("mail.html"))
            {
                message.Body += sr.ReadToEnd();
            }
            message.IsBodyHtml = true;

            message.Priority = MailPriority.High;
            message.Attachments.Add(new Attachment("cat.webp"));
            message.Attachments.Add(new Attachment("task.txt"));

            SmtpClient client = new SmtpClient(server, port);
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;
            client.SendAsync(message, message);

            client.SendCompleted += SendMessageCompleted;

        }

        private void SendMessageCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var state = (MailMessage)e.UserState!;
            MessageBox.Show(state.Subject);
        }

        string GetRichText(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(
                richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd
            );
            return textRange.Text;
        }
    }
}
