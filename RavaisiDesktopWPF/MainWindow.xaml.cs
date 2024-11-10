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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RavaisiDesktopWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// This is an translation of Ravaisi Desktop PDA from the WinForms to WPF
    /// All the functions are modified and adapted to WPF
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
    }
}
