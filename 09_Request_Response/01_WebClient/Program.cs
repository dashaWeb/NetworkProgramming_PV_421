using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_WebClient
{
    class Program
    {
        static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        const string imageUri = @"https://www.atlanticcouncil.org/wp-content/uploads/2021/12/2021-12-16T122128Z_1435296858_RC2KFR903WZL_RTRMADP_3_UKRAINE-CITYSCAPE-scaled-e1640800288914.jpg";
        const string image2Uri = @"https://ukrainetrek.com/images/rivne-ukraine-oblast-views-10.jpg";
        static async Task Main(string[] args)
        {
            // -=-=-=-=-=-=-=-=-=-=- Donwload File using HttpClient -=-=-=-=-=-=-=-=-=-=-
            HttpClient httpClient = new HttpClient();

            /////////////// variant 1
            /* HttpRequestMessage message = new HttpRequestMessage()
             {
                 Method = HttpMethod.Get,
                 RequestUri = new Uri(imageUri)
             };
             HttpResponseMessage response = await httpClient.SendAsync(message);
             Console.WriteLine("Status: " + response.StatusCode);



             using (FileStream fs = new FileStream(desktopPath + "/image.jpg", FileMode.Create))
             {
                 await response.Content.CopyToAsync(fs);
             }*/

            /////////////// variant 2
            /*byte[] data = await httpClient.GetByteArrayAsync(image2Uri);
            File.WriteAllBytes($@"{desktopPath}/image2.jpg", data);*/

            // -=-=-=-=-=-=-=-=-=-=- Donwload File using WebClient -=-=-=-=-=-=-=-=-=-=-
           /* WebClient webClient = new WebClient();
            //Console.WriteLine(webClient.DownloadString("https://jsonplaceholder.typicode.com/users"));
            //sync download
            webClient.DownloadFile(imageUri, $@"{desktopPath}\picture.jpg");

            Console.WriteLine("File loaded");*/



            // async download
            Console.WriteLine("File loading...");
            //DownloadFileAsync("http://212.183.159.230/5MB.zip");
            //DownloadFileAsync("http://212.183.159.230/10MB.zip");
            //DownloadFileAsync(imageUri);

            Console.WriteLine("Continue...");
            Console.Read();
        }

        private static async void DownloadFileAsync(string address)
        {
            WebClient client = new WebClient();

            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            
            string fileName = Path.GetFileName(address);

            await client.DownloadFileTaskAsync(address, $@"{desktopPath}\{fileName}");

            // cancel download
            //client.CancelAsync();

            Console.WriteLine($"{fileName} - File loaded!");
        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
            Console.WriteLine($"Downloading... {(float)e.BytesReceived / 1024 / 1024}MB of {(float)e.TotalBytesToReceive / 1024 / 1024}MB {e.ProgressPercentage}%");
        }
        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine("File downloading was cancelled!");
            else
                Console.WriteLine("File downloaded succesfully!");
        }
    }
}
