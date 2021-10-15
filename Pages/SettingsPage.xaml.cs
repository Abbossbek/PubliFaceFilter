using MaterialDesignThemes.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PubliFaceFilter.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void btnSavePath_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.SavePath = ofd.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            DialogHost.CloseDialogCommand.Execute(((MainWindow)Window.GetWindow(this)).dialogHost, null);
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAddLink_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Masks.Add("/Library/demos");
            lbMasks.Items.Refresh();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
                Properties.Settings.Default.Masks.Remove(((System.Windows.Controls.Button)sender).DataContext.ToString());
            lbMasks.Items.Refresh();
        }
    }
}
