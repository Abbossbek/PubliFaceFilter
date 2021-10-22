
using MaterialDesignThemes.Wpf;

using PubliFaceFilter.Pages;

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Net;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Renci.SshNet;
using PubliFaceFilter.Properties;
using System.Timers;

namespace PubliFaceFilter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const uint GW_CHILD = 1;

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        private static bool enterClicked = false;

        private Frame dialogHostFrame = new Frame() { NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden };
        public MainWindow()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((s, c, ch, ssl) => true);
            Task.Run(() =>
            {
                ScriptEngine engine = Python.CreateEngine();
                var paths = engine.GetSearchPaths();
                paths.Add(@"C:\Python27\Lib");
                engine.SetSearchPaths(paths);
                engine.ExecuteFile(Environment.CurrentDirectory + "/Library/Server.py");
            });
            var updateTimer = new Timer(Settings.Default.UpdateInterval * 60000);
            updateTimer.Elapsed += async (o, e) =>
              {
                  await FileUploadSFTP();
              };
            updateTimer.Start();
        }

        public async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            await Task.Delay(5000);
            await wv2.EnsureCoreWebView2Async();
            wv2.Source = new Uri("http://127.0.0.1:4443" + Settings.Default.Masks[new Random().Next(0, Settings.Default.Masks.Count)]);
            wv2.PreviewGotKeyboardFocus += wv2_GotFocus;
            await FileUploadSFTP();
        }
        protected async override void OnPreviewKeyUp(KeyEventArgs e)
        
        {
            base.OnPreviewKeyUp(e);
            if (dialogHost.IsOpen)
                return;
            switch (e.Key)
            {
                case Key.Left:
                    dialogHost.IsOpen = false;
                    wv2.Source = new Uri("http://127.0.0.1:4443" + Settings.Default.Masks[new Random().Next(0, Settings.Default.Masks.Count)]);
                    break;
                case Key.Right:
                    dialogHost.IsOpen = false;
                    wv2.Source = new Uri("http://127.0.0.1:4443" + Settings.Default.Masks[new Random().Next(0, Settings.Default.Masks.Count)]);
                    break;
                case Key.Return:
                    if (!enterClicked)
                    {
                        enterClicked = true;
                        var i = Settings.Default.TakePictureTimeSeconds;
                        var content = new TextBlock() { FontSize = 72, Margin = new Thickness(20), Foreground = System.Windows.Media.Brushes.Black };
                        while (i > 0)
                        {
                            content.Text = i.ToString();
                            dialogHost.IsOpen = false;
                            DialogHost.Show(content);
                            i--;
                            await Task.Delay(1000);
                            dialogHost.IsOpen = false;
                        }
                        content.Text = Settings.Default.TakePictureText;
                        DialogHost.Show(content);
                        await Task.Delay(1000);
                        dialogHost.IsOpen = false;
                        await Task.Delay(2000);
                        int width = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Width, height = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Height;

                        var filePath = $"{Settings.Default.SavePath}\\{DateTime.Now.ToString().Replace(':', '_')}.jpg";
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        //TakeScreenShot(0, 0, width, height).Save(filePath, ImageFormat.Jpeg);
                        using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                        {
                            await wv2.CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Jpeg, stream);
                        }

                        frame.Visibility = Visibility.Visible;
                        frame.NavigationService.RemoveBackEntry();
                        var page =new SaveDetailsPage(filePath);
                        page.IsVisibleChanged += (a, s) =>
                        {
                            if (!(bool)s.NewValue)
                            {
                                wv2.Visibility = Visibility.Visible;
                                frame.Visibility = Visibility.Collapsed;
                                enterClicked = false;
                            }
                        };
                        frame.NavigationService.Navigate(page);
                        
                        wv2.Visibility = Visibility.Collapsed;
                        //dialogHostFrame.NavigationService.RemoveBackEntry();
                        //dialogHostFrame.NavigationService.Navigate(new SaveDetailsPage(filePath));

                        //await DialogHost.Show(dialogHostFrame, new DialogOpenedEventHandler((s, u) =>
                        //{
                        //    enterClicked = false;
                        //}));
                    }
                    break;
                case Key.S:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        dialogHostFrame.NavigationService.RemoveBackEntry();
                        dialogHostFrame.NavigationService.Navigate(new SettingsPage());
                        dialogHost.IsOpen = false;
                        await DialogHost.Show(dialogHostFrame);
                    }
                    break;
            }

        }

        private void wv2_GotFocus(object sender, RoutedEventArgs e)
        {
            wv2.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(wv2), null);
            Keyboard.ClearFocus();
            Keyboard.Focus(pbMain);
        }

        private void wv2_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            wv2.Visibility = Visibility.Collapsed;
        }

        private void wv2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            wv2.Visibility = Visibility.Visible;
        }
        public async Task FileUploadSFTP()
        {
            await Task.Run(() =>
            {
                using (var client = new SftpClient(Settings.Default.SFTPHost, Settings.Default.SFTPPort,
                    Settings.Default.SFTPUsername, Settings.Default.SFTPPassword))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        try
                        {
                            var ip = GetMyIp().ToString();
                            var folder = $"{Environment.MachineName} (IP: {ip})";
                            foreach (var file in Directory.GetFiles(Settings.Default.SavePath))
                            {
                                using (var fileStream = new FileStream(file, FileMode.Open))
                                {
                                    if (!client.Exists(folder))
                                        client.CreateDirectory(folder);
                                    client.UploadFile(fileStream, $"{folder}/{Path.GetFileName(file)}");
                                }
                                File.Delete(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            });
        }

        public static IPAddress GetMyIp()
        {
            List<string> services = new List<string>()
        {
            "https://ipv4.icanhazip.com",
            "https://api.ipify.org",
            "https://ipinfo.io/ip",
            "https://checkip.amazonaws.com",
            "https://wtfismyip.com/text",
            "http://icanhazip.com"
        };
            using (var webclient = new WebClient())
                foreach (var service in services)
                {
                    try { return IPAddress.Parse(webclient.DownloadString(service)); } catch { }
                }
            return null;
        }
    }
}
