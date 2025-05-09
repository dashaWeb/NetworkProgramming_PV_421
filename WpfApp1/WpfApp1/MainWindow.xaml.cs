using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ObservableCollection<CopyProcessInfo> files = new ObservableCollection<CopyProcessInfo>();
    public MainWindow()
    {
        InitializeComponent();
        lbFiles.ItemsSource = files;
    }

    private void SourceBtn(object sender, RoutedEventArgs e)
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true; // only folder
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            tbPath.Text = dialog.FileName;
        }
    }

    private async void DownloadBtn(object sender, RoutedEventArgs e)
    {
        if (tbPath == null || tbURL == null)
        {
            MessageBox.Show("Enter a filepath and url", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var file = new CopyProcessInfo(Path.GetFileName(tbURL.Text), tbURL.Text, tbPath.Text);
        files.Add(file);
        await DownloadFile(file);
    }

    public Task DownloadFile(CopyProcessInfo file)
    {
        return Task.Run(() =>
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(file.Path));
            // Download the file
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    // Update the progress bar and percentage
                    file.Percentage = e.ProgressPercentage;
                };
                client.DownloadFileCompleted += (s, e) =>
                {
                    // Show a message when the download is complete
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show("Download complete!", "Success", MessageBoxButton.OK, MessageBoxImage.Information));
                };
                client.DownloadFileAsync(new Uri(file.URL), Path.Combine(file.Path, file.FileName));
            }
        });

    }

    [AddINotifyPropertyChangedInterface]
    public class CopyProcessInfo
    {
        public string FileName { get; set; }
        public string URL { get; set; }
        public string Path { get; set; }
        public double Percentage { get; set; }
        public int PercentageInt => (int)Percentage;
        public CopyProcessInfo(string filename, string url, string path)
        {
            FileName = filename;
            URL = url;
            Path = path;
        }
    }

    private void lbFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (files[lbFiles.SelectedIndex].Percentage < 100)
        {
            MessageBox.Show("File don't downloaded fully", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer",
            Arguments = Path.Combine(files[lbFiles.SelectedIndex].Path, files[lbFiles.SelectedIndex].FileName),
            UseShellExecute = true
        });
    }
}