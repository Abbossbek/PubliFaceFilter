
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
            "http://127.0.0.1:4443/Library/demos/threejs/cubeExpr",
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
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((s,c,ch, ssl)=>true);
            Task.Run(() =>
            {

            ScriptEngine engine = Python.CreateEngine();
            var paths = engine.GetSearchPaths();
            paths.Add(@"C:\Python27\Lib");
            engine.SetSearchPaths(paths);
            engine.ExecuteFile(Environment.CurrentDirectory + "/Library/Server.py");
            });
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
