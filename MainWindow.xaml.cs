
using MaterialDesignThemes.Wpf;

using PubliFaceFilter.Pages;

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Windows;
using System.Windows.Input;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;

namespace PubliFaceFilter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int maskIndex = 0;
        private static List<string> masks = new List<string>() {
            "http://127.0.0.1:4443/Library/demos/threejs/angel_demon",
            "http://127.0.0.1:4443/Library/demos/threejs/anonymous",
            "http://127.0.0.1:4443/Library/demos/threejs/butterflies",
            "http://127.0.0.1:4443/Library/demos/threejs/casa_de_papel",
            "http://127.0.0.1:4443/Library/demos/threejs/celFace",
            "http://127.0.0.1:4443/Library/demos/threejs/cloud",
            "http://127.0.0.1:4443/Library/demos/threejs/cube",
            "http://127.0.0.1:4443/Library/demos/threejs/cube2cv",
            "http://127.0.0.1:4443/Library/demos/threejs/dog_face",
            "http://127.0.0.1:4443/Library/demos/threejs/faceDeform",
            "http://127.0.0.1:4443/Library/demos/threejs/fireworks",
            "http://127.0.0.1:4443/Library/demos/threejs/football_makeup",
            "http://127.0.0.1:4443/Library/demos/threejs/glassesVTO",
            "http://127.0.0.1:4443/Library/demos/threejs/gltf_fullScreen",
            "http://127.0.0.1:4443/Library/demos/threejs/halloween_spider",
            "http://127.0.0.1:4443/Library/demos/threejs/luffys_hat_part1",
            "http://127.0.0.1:4443/Library/demos/threejs/luffys_hat_part2",
            "http://127.0.0.1:4443/Library/demos/threejs/matrix",
            "http://127.0.0.1:4443/Library/demos/threejs/miel_pops",
            "http://127.0.0.1:4443/Library/demos/threejs/multiCubes",
            "http://127.0.0.1:4443/Library/demos/threejs/multiLiberty",
            "http://127.0.0.1:4443/Library/demos/threejs/rupy_helmet",
            "http://127.0.0.1:4443/Library/demos/threejs/tiger",
            "http://127.0.0.1:4443/Library/demos/threejs/werewolf",
        };
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
        }
        public async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            await Task.Delay(3000);
            wv2.Source = new Uri(masks[maskIndex]);
        }
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    wv2.Source = new Uri(masks[maskIndex == 0 ? maskIndex = masks.Count - 1 : --maskIndex]);
                    break;
                case Key.Right:
                    wv2.Source = new Uri(masks[maskIndex == masks.Count - 1 ? maskIndex = 0 : ++maskIndex]);
                    break;
                case Key.Enter:
                    tbMain.Visibility = Visibility.Visible;
                    var i = Properties.Settings.Default.TakePictureTimeSeconds;
                    while (i > 0)
                    {
                        tbMain.Text = i.ToString();
                        i--;
                        await Task.Delay(1000);
                    }
                    tbMain.Text = Properties.Settings.Default.TakePictureText;
                    await Task.Delay(2000);
                    tbMain.Visibility = Visibility.Collapsed;
                    int width = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Width, height = (int)System.Windows.Forms.SystemInformation.MaxWindowTrackSize.Height;

                    var filePath = $"{Properties.Settings.Default.SavePath}\\{DateTime.Now.ToString().Replace(':','_')}.jpg";
                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    TakeScreenShot(0, 0, width, height).Save(filePath, ImageFormat.Jpeg);
                    dialogHostFrame.NavigationService.RemoveBackEntry();
                    dialogHostFrame.NavigationService.Navigate(new SaveDetailsPage(filePath));
                    await DialogHost.Show(dialogHostFrame);
                    break;
                case Key.S:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        dialogHostFrame.NavigationService.RemoveBackEntry();
                        dialogHostFrame.NavigationService.Navigate(new SettingsPage());
                        await DialogHost.Show(dialogHostFrame);
                    }
                    break;
                default:
                    break;
            }
        }
        private Bitmap TakeScreenShot(int startX, int startY, int width, int height)
        {
            Bitmap ScreenShot = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ScreenShot);
            g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            return ScreenShot;
        }
    }
}
