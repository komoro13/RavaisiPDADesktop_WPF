﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using Point = System.Windows.Point;

namespace RavaisiDesktopWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// This is an translation of Ravaisi Desktop PDA from the WinForms to WPF
    /// All the methods are modified and adapted to WPF
    /// Changes to the UI are going to be made as well in order to 
    /// make it more user fiendly and extra features are going to be added as well
    /// </summary>
    /// 
    /// To contributors
    /// There are two ways to contribute to that project
    /// 1. Start building the next features
    /// 2. Help enhancing the existing ones
    /// 
    /// The current features that need enhancement are
    /// 1.Autoprint functionality: When autoprint checkbox is checked order has to be printed automatically
    /// 2.TabItem appearence: TabItem text font has to be bigger
    /// 3.TabItem product items: Items in the order tab have to be selectable and clickable for the delete product functionality
    /// 
    /// Next features are
    /// 1.Deleting or editing items of an order: Items of the order must be deletable and editable, this requires decoding, editing and encoding the order
    /// 2.Filters: The side menu in the UI have to contain a sorting and filter menu that is gonna let the user sort and filer the orders displayed
    /// For example: show only the unread ones, and sort by time
    /// 3. Navbar: A navbar is going to navigate to the rest windows of the program and its going to consist of 5 buttons, orders, products, history, settings, help
    ///


    public partial class MainWindow : Window
    {

        //--------------------------------Global variables----------------------------------------------------------
        private int lastIndex;
        private String current_sql_cmd;
        private int loadedOrders;
        private int activeOrders;
        bool autoprint;
        private const int OPEN_ORDERS = 0;
        private const int NEW_ORDERS = 1;
        int filter;
        private const int BY_PRICE = 0;
        private const int BY_SIZE = 1;
        private const int BY_TIME_SET = 2;
        int sort;
        bool descending;
        ArrayList newOrders;
        ArrayList unprintedOrders;
        Order loadedOrder;
        List<Order> orders;
        String XAMPPPath = "";
        Order selectedOrder;
        //--------------------------------End of Global variables---------------------------------------------------

        //--------------------------------------Database constants---------------------------------------------------
        public string dbconnect = "server=127.0.0.1; User=root; password=;database=ravaisi";
        private const String OPEN_ORDERS_SQL_CMD = "SELECT * FROM orders WHERE closed=0 AND order_index=1";
        private const String NEW_ORDERS_SQL_CMD = "SELECT * FROM orders WHERE closed=0 AND loaded=0";
        private const String ALL_ORDERS_SQL_CMD = "SELECT * FROM orders";
        private const String COUNT_OPEN_ORDERS_SQL_CMD = "SELECT COUNT(*) FROM orders WHERE closed=0 AND order_index=1";
        private const String COUNT_NEW_ORDERS_SQL_CMD = "SELECT COUNT(*) FROM orders WHERE closed=0 AND loaded=0";
        private const String ALL_ORDERS_FOR_PRINTING_SQL_CMD = "SELECT * FROM orders WHERE closed=0 AND printed=0";
        //--------------------------------------End of Database constants----------------------------------------------

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            init();
            Task.Run(()=>checkForChanges());
        }
        private void init()
        {
            //Initializing the program
            XAMPP.Start();
            this.WindowState = WindowState.Maximized;//maximize window
            autoprint = true;
            current_sql_cmd = OPEN_ORDERS_SQL_CMD;
            filter = OPEN_ORDERS;
        }
        private int getLastIndex()
        {
            //This method returns the index of the last order sent in the database
            //This method returns the index of the last order sent in the database
            String sql_command = "SELECT id FROM orders ORDER BY id DESC LIMIT 1";
            string result = OrdersSQLDatabase.GetString(sql_command);
            if (!result.Equals(""))
                return int.Parse(result);
            else return 0;
        }
        
        public void getOrders(string cmd)
        {
            //This method gets all the orders as specified by the sql command
            //converts them from data rows to Order object and adds each order
            //to the Order ArrayList orders
            orders = new List<Order>();
            DataRow[] rows = OrdersSQLDatabase.getRowsArray(cmd);
            foreach (DataRow row in rows)
            {
                if (row["id"] == null)
                    break;
                Order order = new Order(row["order_string"].ToString(), //order constructor
                                        row["price"].ToString(),
                                        row["id"].ToString(),
                                        (Boolean)row["loaded"],
                                        (Boolean)row["printed"],
                                        row["order_index"].ToString(),
                                        DateTime.Parse(row["date_time"].ToString()));

                orders.Add(order); //adding each order to the Order ArrayList orders
            }            
        }
        
 
        private void addOrderTab(String orderString, String text)
        {
            //This method adds a tab to the OrdersTabControl
            TabItem tabItem = new TabItem();
            tabItem.Header = text;
            //Label label = CopontentsConstructors.createLabel(orderString, "label", new FontFamily("Arial"), 10);
            StackPanel ordersStackPanel = new StackPanel();
            foreach (Button button in loadedOrder.GetButtons())
            {
                ordersStackPanel.Children.Add(button);
            }
            tabItem.Content = ordersStackPanel;
            OrdersTabControl.Items.Add(tabItem);
        }
        private void showOrder(Order order)
        {
            //This mehtod add each order to the OrderTabControl
            loadedOrder = order;
            order.setOrderAsLoaded(true);
            OrdersTabControl.Items.Clear();
            if (!order.mergedOrders)
                order.mergeOrders();
            TableLabel.Content = order.table;
            addOrderTab(order.getOrderString(order.order), "Ολη η παραγγελια");
            OrdersTabControl.SelectedIndex = 0;
            //foreach (String orderString in order.orderStrings)
           // {
           //     addOrderTab(order.getOrderString(order.getAddedOrder(orderString)), "Παραγγελια: " + orderString.Split('#')[1]);
           // }
        }
        void orderButtonClick(object sender, EventArgs e, Order order)
        {
            showOrder(order);
            selectedOrder = order;
        }
        void getUnloadedOrders()
        {
            //this method writes all the table names of the orders that has not been yet displayed to an array
            newOrders = new ArrayList();
            DataRow[] data = OrdersSQLDatabase.getRowsArray("SELECT order_table FROM orders WHERE loaded=0");
            foreach (DataRow row in data)
            {
                newOrders.Add(row["order_table"]);
            }
        }
        void printUnprintedOrders()
        {
            //This method prints all the orders that has not been printed
            ArrayList indices;
            foreach (Order order in orders)
            {             
                indices = order.getUnprintedIndices();
                foreach (int index in indices)
                order.Print(index.ToString());                   
            }
        }
        private void sortOrders()
        {
            if (sort == BY_PRICE)
            {
                if (!descending)
                    orders = Sorting.Price(orders);
                else orders = Sorting.PriceD(orders);
                return;
            }
            if (sort == BY_SIZE)
            {
                if (!descending)
                    orders = Sorting.Size(orders);
                else orders = Sorting.SizeD(orders);
                return;
            }
            if (sort == BY_TIME_SET)
            {
                if (!descending)
                    orders = Sorting.Time(orders);
                else orders = Sorting.TimeD(orders);
            }
        }
        private void showOrders()
        {
            int ButtonWidth = 200;
            int ButtonHeight = 150;

            int ButtonX = 15;
            int ButtonY = 15;
            int StandardButtonX = 15;
            Point buttonLocation = new Point();
            TableCanvas.Children.Clear();//Clearing all the controls to update
            getUnloadedOrders();           
            //for every order in the orders array create a button 
            foreach (Order order in orders)
            {
                if (filter == 1)
                    if (!Searching.Find(newOrders, order.table))
                        continue;
                Button button = CopontentsConstructors.createButton(order.table, order.table + "Btn", new Point(ButtonX, ButtonY), ButtonWidth, ButtonHeight, (s, e) => orderButtonClick(s, e, order));
                TableCanvas.Children.Add(button);

                if ((Canvas.GetLeft(button) + 200 + ButtonWidth) > TableCanvas.ActualWidth)
                {
                    ButtonX = StandardButtonX;
                    ButtonY = ButtonY + 15 + ButtonHeight;
                }
                else
                {
                    ButtonX += ButtonWidth + StandardButtonX;
                }
                buttonLocation.X = ButtonX;
                buttonLocation.Y = ButtonY;                
            }

            if (autoprint)
                printUnprintedOrders();
        }
        private int getActiveOrders()
        {
            //This method returns the count of active orders 
            string result = OrdersSQLDatabase.GetString(COUNT_OPEN_ORDERS_SQL_CMD);
            if (!result.Equals(""))
                return int.Parse(result);
            else return 0;
        }
        private bool checkForActiveOrders()
        {
            if (this.activeOrders != getActiveOrders())
            {
                this.activeOrders = getActiveOrders();
                return true;
            }
            return false;
        }
        private bool checkForNewOrder()
        {
            if (this.lastIndex < getLastIndex())
            {
                this.lastIndex = getLastIndex();
                {
                    return true;
                }
            }
            //this.lastIndex = getLastIndex();
            return false;
        }
         private int getLoadedOrders()
         {
            string result = OrdersSQLDatabase.GetString(COUNT_NEW_ORDERS_SQL_CMD);
            if (!result.Equals(""))
                return int.Parse(result);
            else return 0;

        }
        private bool checkForLoadedOrders()
        {
            if (this.loadedOrders != getLoadedOrders())
            {
                this.loadedOrders = getLoadedOrders();
                return true;
            }
            return false;
        }

          
        private bool checkForChanges()
        {
            while (true)
            {
                if (checkForNewOrder())
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer("bell.wav");
                    player.Play();
                    //MessageBox.Show("Νεα παραγγελια!");
                    Dispatcher.Invoke(new Action(() => getOrders(current_sql_cmd)));
                    Dispatcher.Invoke(new Action(() => showOrders()));
                }
                if (checkForLoadedOrders() && filter!=NEW_ORDERS)
                {
                    Dispatcher.Invoke(new Action(() => getOrders(current_sql_cmd)));
                    Dispatcher.Invoke(new Action(() => showOrders()));
                }
                if (checkForActiveOrders())
                {
                    Dispatcher.Invoke(new Action(() => getOrders(current_sql_cmd)));
                    Dispatcher.Invoke(new Action(() => showOrders()));
                }
            }

        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (loadedOrder == null)
                return;
            loadedOrder.Print(loadedOrder.orderIndex);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (loadedOrder == null)
                return;
            loadedOrder.closeOrder();
        }

        private void AutoPrintCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            autoprint = true;
        }

        private void AutoPrintCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            autoprint = false;
        }

        private void OpenTablesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            filter = OPEN_ORDERS;
            if (orders != null)
                showOrders();
        }

        private void UnreadTablesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            filter = NEW_ORDERS;
            if (orders != null)
                showOrders();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SortComboBox.SelectionBoxItem.ToString().Equals("Price"))
                sort = BY_PRICE;
            else if (SortComboBox.SelectionBoxItem.ToString().Equals("Order size"))
                sort = BY_SIZE;
            else sort = BY_TIME_SET;           
            descending = (bool)descedingCheckBox.IsChecked;
            sortOrders();
            showOrders();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            selectedOrder.showDialog();
        }

        private void productsButton_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow();
            window.Show();
            this.Close();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
            this.Close();
        }

        private void ordersButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow window = new HistoryWindow();
            window.Show();
            this.Close();
        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {
            SupportWindow window = new SupportWindow();
            window.Show();
        }
    }
}
