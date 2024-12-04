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
    /// Interaction logic for ProductsWindow.xaml
    /// 
    /// This window is used to add new categories, products and toppings in the Database
    /// 
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
        }

        //---------------------DATABASE CONSTANTS---------------------------------------

        const string GET_CATEGORIES_SQL_CMD = "SELECT * FROM 'categories'";
        const string GET_PRODUCTS_SQL_CMD = "SELECT * FROM 'products;";
        const string GET_TOPPINGS_SQL_CMD = "SELECT * FROM 'toppings'";

        //---------------------END OF DATABASE CONSTANTS--------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void getCategories()
        {
           
        }
        

        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void productsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
