using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using DataGrid = System.Windows.Controls.DataGrid;

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

        const string GET_CATEGORIES_SQL_CMD = "SELECT * FROM categories";
        const string GET_PRODUCTS_SQL_CMD = "SELECT * FROM products";
        const string GET_TOPPINGS_SQL_CMD = "SELECT * FROM toppings";
        static string dbconnect = "server=127.0.0.1; User=root; password=;database=ravaisi";

        //---------------------END OF DATABASE CONSTANTS--------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataGrid(categoriesDataGrid, GET_CATEGORIES_SQL_CMD);
            loadDataGrid(productsDataGrid, GET_PRODUCTS_SQL_CMD);
            loadDataGrid(toppingsDataGrid, GET_TOPPINGS_SQL_CMD);
        }

        private void loadDataGrid(DataGrid dataGrid, string command)
        {
            

            using (MySqlConnection connection = new MySqlConnection(dbconnect))
            {
                connection.Open();
                using (MySqlCommand cmdSel = new MySqlCommand(command, connection))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                    da.Fill(dt);
                    dataGrid.DataContext = dt;
                }
                connection.Close();
            }
        }
    
        private void addCategory(string name)
        {
            string sql = $"INSERT INTO categories (name) VALUES ('{name}')";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(categoriesDataGrid, GET_CATEGORIES_SQL_CMD);
        }

        private void deleteCategory(string id)
        {       
            string sql = $"DELETE FROM categories WHERE id={id}";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(categoriesDataGrid, GET_CATEGORIES_SQL_CMD);
        }

        private void addProduct(string name, string price, string category, string toppings)
        {
            string sql = $"INSERT INTO products (name, price, category, toppings) VALUES ('{name}', {price}, '{category}', '{toppings}')";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(productsDataGrid, GET_PRODUCTS_SQL_CMD);
        }

        private void deleteProduct(string id)
        {
            string sql = $"DELETE FROM products WHERE id={id}";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(productsDataGrid, GET_PRODUCTS_SQL_CMD);
        }

        private void addTopping(string name, string price)
        {
            string sql = $"INSERT INTO toppings (name, extra ) VALUES ('{name}', {price})";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(toppingsDataGrid, GET_TOPPINGS_SQL_CMD);
        }

        private void deleteTopping(string id)
        {
            string sql = $"DELETE FROM toppings WHERE id={id}";
            OrdersSQLDatabase.Post(sql);
            loadDataGrid(toppingsDataGrid, GET_TOPPINGS_SQL_CMD);
        }
        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }



        private void productsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow window = new HistoryWindow();
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
            SupportWindow window = new SupportWindow();
            window.Show();
            this.Close();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!categoryNameTextBox.Text.Equals(string.Empty))
                addCategory(categoryNameTextBox.Text);
        }

        private void deleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {        
            if (categoriesDataGrid.SelectedItem != null)
                deleteCategory(((DataRowView)categoriesDataGrid.SelectedItem)["id"].ToString());
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            addProduct(productNameTextBox.Text, productPriceTextBox.Text, productCategoryLabel.Content.ToString(), productToppingsTextBlock.Text);
        }

        private void deleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if ((DataRowView)productsDataGrid.SelectedItem != null)
                deleteProduct(((DataRowView)productsDataGrid.SelectedItem)["id"].ToString());
        }

       
        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {
            productCategoryLabel.Content = ((DataRowView)((DataGridRow)sender).Item)["name"].ToString();
        }

        private void DataGridRow_Selected_1(object sender, RoutedEventArgs e)
        {
            if (toppingsCheckBox.IsChecked == true && !Searching.FindT(productToppingsTextBlock.Text.Split(','), ((DataRowView)((DataGridRow)sender).Item)["name"].ToString()))
            {
                {
                    if (!productToppingsTextBlock.Text.Equals(string.Empty)) productToppingsTextBlock.Text += ", ";
                    productToppingsTextBlock.Text += ((DataRowView)((DataGridRow)sender).Item)["name"].ToString();
                }
            }
        }

        private void toppingsCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void addToppingButton_Click(object sender, RoutedEventArgs e)
        {
            addTopping(toppingNameTextBox.Text, toppingPriceTextBox.Text);
        }

        private void deleteToppingButton_Click(object sender, RoutedEventArgs e)
        {
            if ((DataRowView)toppingsDataGrid.SelectedItem != null)
                deleteTopping(((DataRowView)toppingsDataGrid.SelectedItem)["id"].ToString());
        }
    }
}
