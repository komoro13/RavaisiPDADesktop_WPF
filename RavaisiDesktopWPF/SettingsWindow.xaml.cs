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
using System.Windows.Shapes;

namespace RavaisiDesktopWPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void productsButton_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow();
            window.Show();
            this.Close();
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow window = new HistoryWindow();
            window.Show();
            this.Close();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {
            SupportWindow window = new SupportWindow();
            window.Show();
            this.Hide();
        }
    }
}
