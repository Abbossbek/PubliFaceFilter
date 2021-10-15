using MaterialDesignThemes.Wpf;

using System;
using System.Collections.Generic;
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

namespace PubliFaceFilter.Pages
{
    /// <summary>
    /// Логика взаимодействия для SaveDetailsPage.xaml
    /// </summary>
    public partial class SaveDetailsPage : Page
    {
        private string _filePath;
        public SaveDetailsPage(string filePath)
        {
            InitializeComponent();
            _filePath = filePath;
            img.Source = new BitmapImage(new Uri(filePath));
        }
        public async override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            tbPath.Text = _filePath;
            var i = Properties.Settings.Default.PictureSavedTimeSeconds;
            while (i > 0)
            {
                tbTimer.Text = i.ToString();
                i--;
                await Task.Delay(1000);
            }
            tbTimer.Visibility = Visibility.Collapsed;
            btnClose.Visibility = Visibility.Visible;
             DialogHost.CloseDialogCommand.Execute(((MainWindow)Window.GetWindow(this)).dialogHost, null);
        }
    }
}
