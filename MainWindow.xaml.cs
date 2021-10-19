
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
using System.Threading;
using PubliFaceFilter.Properties;

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
            var updateTimer = new Timer((a) =>
              {

              }, null, 10000, Settings.Default.PictureSavedTimeSeconds);
        }

        public async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            await Task.Delay(5000);
            await wv2.EnsureCoreWebView2Async();
            wv2.Source = new Uri("http://127.0.0.1:4443" + Properties.Settings.Default.Masks[new Random().Next(0, Properties.Settings.Default.Masks.Count)]);
            wv2.PreviewGotKeyboardFocus += wv2_GotFocus;
        }
        private Bitmap TakeScreenShot(int startX, int startY, int width, int height)
        {
            Bitmap ScreenShot = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ScreenShot);
            g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            return ScreenShot;
        }
        protected async override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            switch (e.Key)
            {
                case Key.Left:
                    dialogHost.IsOpen = false;
                    wv2.Source = new Uri("http://127.0.0.1:4443" + Properties.Settings.Default.Masks[new Random().Next(0, Properties.Settings.Default.Masks.Count)]);
                    break;
                case Key.Right:
                    dialogHost.IsOpen = false;
                    wv2.Source = new Uri("http://127.0.0.1:4443" + Properties.Settings.Default.Masks[new Random().Next(0, Properties.Settings.Default.Masks.Count)]);
                    break;
                case Key.Return:
                    if (!enterClicked)
                    {
                        enterClicked = true;
                        var i = Properties.Settings.Default.TakePictureTimeSeconds;
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
                        content.Text = Properties.Settings.Default.TakePictureText;
                        DialogHost.Show(content);
                        await Task.Delay(1000);
                        dialogHost.IsOpen = false;
                        await Task.Delay(2000);
                        int width = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Width, height = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Height;

                        var filePath = $"{Properties.Settings.Default.SavePath}\\{DateTime.Now.ToString().Replace(':', '_')}.jpg";
                        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        //TakeScreenShot(0, 0, width, height).Save(filePath, ImageFormat.Jpeg);
                        using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                        {
                            await wv2.CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Jpeg, stream);
                        }
                        dialogHostFrame.NavigationService.RemoveBackEntry();
                        dialogHostFrame.NavigationService.Navigate(new SaveDetailsPage(filePath));

                        await DialogHost.Show(dialogHostFrame, new DialogOpenedEventHandler((s, u) =>
                        {
                            enterClicked = false;
                        }));
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
        public void FileUploadSFTP()
        {
            using (var client = new SftpClient(Properties.Settings.Default.SFTPHost, Properties.Settings.Default.SFTPPort,
                Properties.Settings.Default.SFTPUsername, Properties.Settings.Default.SFTPPassword))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    try
                    {
                        foreach (var file in Directory.GetFiles(Properties.Settings.Default.SavePath))
                        {
                            using (var fileStream = new FileStream(file, FileMode.Open))
                            {
                                client.UploadFile(fileStream, Path.GetFileName(file));
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
        }
    }
}
