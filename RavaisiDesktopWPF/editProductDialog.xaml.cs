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
    /// Interaction logic for editProductDialog.xaml
    /// </summary>
    public partial class editProductDialog : Window
    {
        string productName;
        List<CheckBox> toppingCheckBoxes = new List<CheckBox>();
        public editProductDialog()
        {
            InitializeComponent();

        }
        private void productNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void loadProductToppings()
        {
            string toppings = OrdersSQLDatabase.GetString("SELECT toppings FROM products WHERE name='" + productName + "'");
            if(toppings != null) 
            {
                foreach (string topping in toppings.Split(','))
                {
                    toppingCheckBoxes.Add(new CheckBox() { Content = topping.Split('/')[0] }); 
                }               
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
