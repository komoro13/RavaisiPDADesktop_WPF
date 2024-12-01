using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CheckBox = System.Windows.Controls.CheckBox;

namespace RavaisiDesktopWPF
{
    /// <summary>
    /// Interaction logic for editProductDialog.xaml
    /// </summary>
    public partial class editProductDialog : Window
    {
        string productName;
        string currentToppings;
        string comments;
        string quantity;
        Order order;

        List<CheckBox> toppingCheckBoxes = new List<CheckBox>();
        public editProductDialog(string productName, string currentToppings, string comments, string quantity, Order order)
        {
            InitializeComponent();
            this.productName = productName;
            this.currentToppings = currentToppings;
            this.comments = comments;
            this.quantity = quantity;
        }
        private void productNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        bool searchTopping(string topping)
        {
            foreach(string currentTopping in currentToppings.Split(','))
            {
                if (topping.Equals(currentTopping))
                    return true;
            }
            return false;
        }

        private void loadProductToppings()
        {
            string toppings = OrdersSQLDatabase.GetString("SELECT toppings FROM products WHERE name='" + productName + "'");
            string toppingsString;
            if(toppings != null) 
            {
                foreach (string topping in toppings.Split(','))
                {
                    if (!topping.Equals(""))
                    {
                        toppingsString = topping.Split('/')[0];
                        toppingCheckBoxes.Add(new CheckBox() { Content = toppingsString, IsChecked = searchTopping(toppingsString) });
                    }
                }               
            }
        }

        private void showCheckBoxes()
        {
            double y=0;
            foreach (CheckBox checkBox in toppingCheckBoxes) 
            {
                toppingsCanvas.Children.Add(checkBox);
                y = y + 15;
                Canvas.SetTop(checkBox, y);
            }
          
        }

        private void showComments()
        {
            commentsTextBlock.Text = comments;
        }

        private void showQuantity()
        {
            quantityTextBox.Text = quantity;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadProductToppings();
            showCheckBoxes();
            showComments();
            showQuantity();
            productNameLabel.Content = productName;
        }

        private string getCurrentToppings()
        {
            string toppings = "";
            foreach(CheckBox checkBox in toppingCheckBoxes)
            {
                if (checkBox.IsChecked == true)
                {
                    toppings += checkBox.Content + ",";
                }
            }
            if (!toppings.Equals(""))
                toppings.Remove(-1);
            return toppings;
                
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            order.editProductFromOrder(getCurrentToppings(), commentsTextBlock.Text, quantityTextBox.Text);
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
