using MaterialDesignThemes.Wpf;

using PubliFaceFilter.Pages;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace PubliFaceFilter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int maskIndex = 0;
        private static List<string> masks = new List<string>() { 
            "https://127.0.0.1:4443/demos/threejs/angel_demon",
            "https://127.0.0.1:4443/demos/threejs/anonymous",
            "https://127.0.0.1:4443/demos/threejs/butterflies",
            "https://127.0.0.1:4443/demos/threejs/casa_de_papel",
            "https://127.0.0.1:4443/demos/threejs/celFace",
            "https://127.0.0.1:4443/demos/threejs/cloud",
            "https://127.0.0.1:4443/demos/threejs/cube",
            "https://127.0.0.1:4443/demos/threejs/cube2cv",
            "https://127.0.0.1:4443/demos/threejs/cubeExpr",
            "https://127.0.0.1:4443/demos/threejs/dog_face",
            "https://127.0.0.1:4443/demos/threejs/faceDeform",
            "https://127.0.0.1:4443/demos/threejs/fireworks",
            "https://127.0.0.1:4443/demos/threejs/football_makeup",
            "https://127.0.0.1:4443/demos/threejs/glassesVTO",
            "https://127.0.0.1:4443/demos/threejs/gltf_fullScreen",
            "https://127.0.0.1:4443/demos/threejs/halloween_spider",
            "https://127.0.0.1:4443/demos/threejs/luffys_hat_part1",
            "https://127.0.0.1:4443/demos/threejs/luffys_hat_part2",
            "https://127.0.0.1:4443/demos/threejs/matrix",
            "https://127.0.0.1:4443/demos/threejs/miel_pops",
            "https://127.0.0.1:4443/demos/threejs/multiCubes",
            "https://127.0.0.1:4443/demos/threejs/multiLiberty",
            "https://127.0.0.1:4443/demos/threejs/rupy_helmet",
            "https://127.0.0.1:4443/demos/threejs/tiger",
            "https://127.0.0.1:4443/demos/threejs/werewolf", 
        };
        public MainWindow()
        {
            InitializeComponent();
            var processInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/Library/httpsServer.py");
            processInfo.CreateNoWindow = true;
            Process.Start(processInfo);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            wv2.Source = new Uri(masks[maskIndex]);
        }
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    wv2.Source = new Uri(masks[maskIndex==0?maskIndex = masks.Count-1:--maskIndex]);
                    break;
                case Key.Right:
                    wv2.Source = new Uri(masks[maskIndex == masks.Count-1 ? maskIndex = 0 : ++maskIndex]);
                    break;
                case Key.Enter:

                    break;
                case Key.S:
                    if(Keyboard.IsKeyDown(Key.LeftCtrl)|| Keyboard.IsKeyDown(Key.RightCtrl))
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
    }
}
