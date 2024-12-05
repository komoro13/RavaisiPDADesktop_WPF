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
    /// Interaction logic for SupportWindow.xaml
    /// </summary>
    public partial class SupportWindow : Window
    {
        public SupportWindow()
        {
            InitializeComponent();
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

        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
            this.Close();
        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
