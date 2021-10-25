using MaterialDesignThemes.Wpf;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
        public ObservableCollection<StringWrapper> Strings { get; set; } = new ObservableCollection<StringWrapper>();

        public class StringWrapper
        {
            public string Content { get; set; }

            public StringWrapper(string content)
            {
                this.Content = content;
            }

            public static implicit operator StringWrapper(string content)
            {
                return new SettingsPage.StringWrapper(content);
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            foreach (var item in Properties.Settings.Default.Masks)
            {
                this.Strings.Add(item);
            }
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
            Properties.Settings.Default.Masks.Clear();
            Properties.Settings.Default.Masks.AddRange(Strings.Select(x => x.Content).ToArray());
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
            Strings.Add("/Library/demos");
            lbMasks.Items.Refresh();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Strings.Remove((StringWrapper)((System.Windows.Controls.Button)sender).DataContext);
            lbMasks.Items.Refresh();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (Hash(tbPasswordEnter.Text) == Properties.Settings.Default.Password)
            {
                grMain.Visibility = Visibility.Visible;
                grPass.Visibility = Visibility.Collapsed;
            }
        }
        static string Hash(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
