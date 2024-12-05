using Google.Protobuf;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace RavaisiDesktopWPF
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        
        public HistoryWindow()
        {
            InitializeComponent();
        }

        Order loadedOrder;
        public string dbconnect = "server=127.0.0.1; User=root; password=;database=ravaisi";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void getOrders(string from, string to)
        {
            string sql = $"SELECT id, date_time, order_table, price, order_index FROM orders WHERE date_time>='{from}' AND date_time<='{to}'";
            using (MySqlConnection connection = new MySqlConnection(dbconnect))
            {
                connection.Open();
                using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                    da.Fill(dt);
                    ordersDataGrid.DataContext = dt;
                }
                connection.Close();
            }  
        }

        public void getOrder(string id)
        {
            //This method gets all the orders as specified by the sql command
            //converts them from data rows to Order object and adds each order
            //to the Order ArrayList orders
            string sql = $"SELECT * FROM orders WHERE id={id}";
            DataRow[] rows = OrdersSQLDatabase.getRowsArray(sql);
           
                if (rows[0]["id"] == null)
                    return;
                Order order = new Order(rows[0]["order_string"].ToString(), //order constructor
                                        rows[0]["price"].ToString(),
                                        rows[0]["id"].ToString(),
                                        (Boolean)rows[0]["loaded"],
                                        (Boolean)rows[0]["printed"],
                                        rows[0]["order_index"].ToString(),
                                        DateTime.Parse(rows[0]["date_time"].ToString()));

                showOrder(order, order.orderIndex.ToString()); //adding each order to the Order ArrayList orders
            }

        private void addOrderTab(String orderString, String text)
        {
            //This method adds a tab to the OrdersTabControl
            TabItem tabItem = new TabItem();
            tabItem.Header = text;
            //Label label = CopontentsConstructors.createLabel(orderString, "label", new FontFamily("Arial"), 10);
            StackPanel ordersStackPanel = new StackPanel();
            foreach (Button button in loadedOrder.GetButtonsH())
            {
                ordersStackPanel.Children.Add(button);
            }
            tabItem.Content = ordersStackPanel;
            OrdersTabControl.Items.Add(tabItem);
        }
        private void showOrder(Order order,string index)
        {
            //This mehtod add each order to the OrderTabControl
            order.setOrderAsLoaded(true);
            OrdersTabControl.Items.Clear();
            loadedOrder = order;
            if (!order.mergedOrders)
                order.mergeOrders();
            TableLabel.Content = order.table;
            addOrderTab(order.getOrderString(order.order), index);
            OrdersTabControl.SelectedIndex = 0;
            //foreach (String orderString in order.orderStrings)
            // {
            //     addOrderTab(order.getOrderString(order.getAddedOrder(orderString)), "Παραγγελια: " + orderString.Split('#')[1]);
            // }
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

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = DateTime.Parse(startDatePicker.Text);
            DateTime endDate = DateTime.Parse(endDatePicker.Text);
            getOrders(startDate.ToString("yyyy-MM-dd") + " 17:00:00", endDate.ToString("yyyy-MM-dd") + " 05:00:00");
        }

        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {
            getOrder(((DataRowView)((DataGridRow)sender).Item)["id"].ToString());
        }
    }
}
