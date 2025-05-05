using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Text;


internal class Program
{
    const string username = "dashakonopelko@gmail.com";
    const string password = "mtyb njin ypqg xwvw";
    const string otheruser = "velax12199@idoidraw.com";
    private static void Main(string[] args)
    {
        /// Send Mails (SMTP)
        /*MimeMessage message = new();
        message.From.Add(new MailboxAddress("Dasha",username));
        message.To.Add(new MailboxAddress("User",otheruser));

        message.Subject = "Добрий вечір, ми з України! Hello";
        message.Importance = MessageImportance.High;

        *//*message.Body = new TextPart("plain")
        {
            Text = @"Hey Alice,

                What are you up to this weekend? Monica is throwing one of her parties on
                Saturday and I was hoping you could make it.

                Will you be my +1?

                -- Joey
                "
        };*//*
        // create our message text, just like before (except don't set it as the message.Body)
        var body = new TextPart("plain")
        {
            Text = @"Hey Alice,

            What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.

            Will you be my +1?

            -- Joey
            "
        };
        var path = @"D:\Dasha\Programming\Gropus\NetworkProgramming_PV_421\06_Smtp_sendMessage\Smtp_sendMessage\bin\Debug\net7.0-windows\cat.webp";
        // create an image attachment for the file located at path
        var attachment = new MimePart("image", "gif")
        {
            Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(path)
        };

        // now create the multipart/mixed container to hold the message text and the
        // image attachment
        var multipart = new Multipart("mixed");
        multipart.Add(body);
        multipart.Add(attachment);

        // now set the multipart/mixed as the message body
        message.Body = multipart;


        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);
            client.Send(message);
        }*/

        ///////// Get Mails (IMAP)
        ///
        Console.OutputEncoding = Encoding.UTF8;
        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);

            foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
            {
                Console.WriteLine(item.Name);
            }

            client.GetFolder(MailKit.SpecialFolder.Sent).Open(MailKit.FolderAccess.ReadWrite);
            var folder = client.GetFolder(MailKit.SpecialFolder.Sent);

            var id = folder.Search(MailKit.Search.SearchQuery.All)[folder.Search(MailKit.Search.SearchQuery.All).Count - 1];
            var m = folder.GetMessage(id);
            Console.WriteLine($"{m.Date} {m.Subject}");

            //folder.MoveTo(id, client.GetFolder(MailKit.SpecialFolder.Junk));
            folder.AddFlags(id, MessageFlags.Deleted, true);
            folder.Expunge();
            /*foreach (var item in folder.Search(MailKit.Search.SearchQuery.All))
            {
                var m = folder.GetMessage(item);
                Console.WriteLine($"{m.Date} {m.Subject}");
            }*/
        }


    }
}