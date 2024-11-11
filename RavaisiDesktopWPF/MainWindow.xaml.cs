using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using Color = System.Drawing.Color;
using FontFamily = System.Windows.Media.FontFamily;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;
using TabControl = System.Windows.Controls.TabControl;

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
    public partial class MainWindow : Window
    {

        //--------------------------------Global variables----------------------------------------------------------
        private int lastIndex;
        private String current_sql_cmd;
        private int loadedOrders;
        private int activeOrders;
        Order loadedOrder;
        List<Order> orders;
        String XAMPPPath = "";
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
        }
        private void init()
        {
            //Initializing the program
            XAMPP.Start();
            this.WindowState = WindowState.Maximized;//maximize window
        }
        private int getLastIndex()
        {
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
                Order order = new Order(row["order_String"].ToString(), //order constructor
                                        row["price"].ToString(),
                                        row["id"].ToString(),
                                        (Boolean)row["loaded"],
                                        (Boolean)row["printed"],
                                        row["order_index"].ToString());

                orders.Add(order); //adding each order to the Order ArrayList orders
            }
        }
        private Button createButton(string text, string name, Point location, int width, int height, Action<object, EventArgs> click)
        {
            //Button constructor
            Button b = new Button()
            {
                Content = text,
                Name = name,
                Width = width,
                Height = height,
                FontSize = 15,
                FontFamily = new FontFamily("Arial")
            };
            Canvas.SetLeft(b,location.X);
            Canvas.SetTop(b, location.Y);
            b.Click += new RoutedEventHandler(click);
            return b;
        }
        private Label createLabel(string text, string name, FontFamily fontFamily, int fontSize)
        {
            //Label constructor
            Label l = new Label()
            {
                Content = text,
                Name = name,
                FontFamily = fontFamily,
                FontSize = fontSize
            };
            return l;
        }
        private void addOrderTab(String orderString, String text)
        {
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = text;
                Label label = createLabel(orderString, "label", new FontFamily("Arial"), 10);             
                tabItem.Content = label;
                OrdersTabControl.Items.Add(tabItem);
            }
        }
        private void showOrder(Order order)
        {
            loadedOrder = order;          
            order.setOrderAsLoaded(true);
            OrdersTabControl.Items.Clear();
            if (!order.mergedOrders)
                order.mergeOrders();
            addOrderTab(order.getOrderString(order.order), "Ολη η παραγγελια");          
            foreach (String orderString in order.orderStrings)
            {
                addOrderTab(order.getOrderString(order.getAddedOrder(orderString)), "Παραγγελια: " + orderString.Split('#')[1]);
            }
        }
        void orderButtonClick(object sender, EventArgs e, Order order)
        {
            showOrder(order);
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

            //for every order in the orders array create a button 
            foreach (Order order in orders)
            {
                
                Button button = createButton(order.table, order.table + "Btn", new Point(ButtonX, ButtonY), ButtonWidth, ButtonHeight, (s, e) => orderButtonClick(s, e, order));
                TableCanvas.Children.Add(button);

                if ((Canvas.GetLeft(button) + 100 + ButtonWidth) > TableCanvas.ActualWidth)
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
        }
        private int getActiveOrders()
        {
            //This method returns the count of active orders 
            string result = OrdersSQLDatabase.GetString(COUNT_OPEN_ORDERS_SQL_CMD);
            if (!result.Equals(""))
                return int.Parse(result);
            else return 0;
        }
        private int checkForActiveOrders()
        {
            //This method returns 1 if active orders are more than
            //before, 2 if they are less and 0 if they are same
            if (this.activeOrders > getActiveOrders())
            {
                this.activeOrders = getActiveOrders();
                return -1;
            }
            if (this.activeOrders < getActiveOrders())
                return 1;
            return 0;
        }
        private bool checkForChanges()
        {
            //This is the main loop of a program
            //it checks the number of the active
            //orders in the database and if the 
            //number changes, it displayes a message
            //produces a sound effect, and refreshes
            //the loaded orders in the orders 
            //array using getOrders and next in the UI
            
            while (true)
            {
                if (checkForActiveOrders() != 0)
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer("bell.wav");
                    player.Play();
                    MessageBox.Show("Νεα παραγγελια!");
                    Dispatcher.Invoke(new Action(() => getOrders(current_sql_cmd)));
                    Dispatcher.Invoke(new Action(() => showOrders()));
                }                
            }

        }
    }
}
