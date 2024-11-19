using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Printing;
using FontStyle = System.Drawing.FontStyle;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;
using Point = System.Drawing.Point;
using PrintDialog = System.Windows.Controls.PrintDialog;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Label = System.Windows.Controls.Label;
using FontFamily = System.Windows.Media.FontFamily;
using MessageBox = System.Windows.MessageBox;

namespace RavaisiDesktopWPF
{
    class Order
    {
        
        string dbconnect = "server=127.0.0.1; User=root; password=;database=ravaisi";//Database credentials
        

        string orderString; 
        public string table;

        ArrayList products = new ArrayList();
        
        string price;
        string products_string;
        string current_order;

        public DateTime dateTime;
        
        public ArrayList order = new ArrayList();
        public ArrayList orderStrings = new ArrayList();
        
        public string orderId;
        string content = "";
        
        public string orderIndex;
        
        Product product1;
        public bool loaded;
        public bool printed;
        public bool mergedOrders;
        string stringToPrint;

        
        bool setHeader = false;

        Font font = new Font("Arial", 10);
        PrintDocument printDocument = new PrintDocument();
    

        public Order(String orderString, String price, String orderId, Boolean loaded, Boolean printed, String orderIndex, DateTime dateTime)
        {

            this.orderString = orderString;//Order string is the order encoded by the android app
            this.table = this.orderString.Split('|')[0].Split('{')[1].Split('}')[0].Split(':')[1].Trim();//decoding the table of the order
            this.price = price; //Price of the order is calculated by the android app 
            this.orderId = orderId; //Order id is a unique number of each order given by the database
            this.loaded = loaded; //1 if order has been opened again 0 if not
            this.printed = printed; //1 if order has been printed 0 if not
            this.orderIndex = orderIndex; //The index of the order starting by the first order since that table opened
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("pprnm", 285, 600);//The size of the print page
            this.dateTime = dateTime;
        }

        class Product
        {
            //When the order is decoded it is passed in an array of Product objects

            public String name;
            public String price;
            public String quantity;
            public String toppings;
            public String comments;

            public Product(String name, String price, String quantity, String toppings, String comments)
            {
                //Product constructor
                this.name = name;
                this.price = price;
                this.quantity = quantity;
                this.toppings = toppings;
                this.comments = comments;
            }
            public String GetString()
            {
                //Get string method returns the string of the product
                //When order is displayed to the screen each line
                //is the product string consisting of the product
                //quantity, name price and toppings, comments if
                //they exist
                //
                //Product string format
                //
                //[Product.quantity] [Product.name]               [Product.price]
                //product.toppings
                //Comments: product.comments 
                //

                String result;//The string the result is going to stored in

                if (this.toppings.Equals(String.Empty))//If the product has toppings
                {
                    result = this.quantity + " " + this.name; // + " " + this.toppings;
                    for (int j = 0; j <= (20 - this.name.Length); j++)
                    {
                        result += " ";
                    }
                }
                else//If the product has no toppings
                {
                    result = this.quantity + " " + this.name;
                    for (int j = 0; j <= (20 - this.name.Length); j++)
                    {
                        result += " ";
                    }
                }
                result += (float.Parse(this.price)) / 10;//Append price of the product of the string, [TO FIX] price has to be divided by 10 to be right
                if ((float.Parse(this.price) / 10).ToString().Contains(",")) //if price is has decimal add a 0
                    result += "0";                                           //else add ,00           
                else result += ",00";
                result += " €"; //Append the currency sign
                if (!this.toppings.Equals(String.Empty))
                    result += "\nΥλικα: " + this.toppings;
                result += "\nΣχολια: " + alignComments(this.comments);
                return result;
            }
            public string getHeaderProgramString()
            {
                //This method returns the string to be printed by the order printer
                //Each header of the product consists of its quantity, name and price
                //Format is same as the product string first line and the number
                //of " " depends on the size of the page

                string header = "";  //String the header is going to be stored
                int pageLength = 25; //Length of the page
                header = this.quantity + " " + this.name;
                for (int i = 0; i < (pageLength - (this.quantity.Length + this.name.Length + 5)); i++)
                {
                    header += " ";
                }
                this.price = this.price.Replace(".", ","); //Replace . with , depends on the country standard system
                header += this.price;
                if (price.Contains(",")) //if price has decimal add 0
                    header += "0";
                header += "€"; //Add currency sign
                return header;
            }
            public string alignComments(string comments)
            {
                //This method fixes the comments          
                //depending to the comments string 
                //length by inserting "\n"
                //every time its gonna surpass page length

                String str = comments;

                if (str.Length > 30)
                {
                    str = str.Insert(30, "\n");
                    if (str.Length > 67)
                        str = str.Insert(68, "\n");
                    return str;
                }
                return str;
            }

        }
        int getOrderStrings()
        {
            //This method gets every order string of the same table that is marked as open
            //it adds it to the Order.orderstirng String ArrayList and returns the number
            //of the strings
            orderStrings.Clear(); // Empty the orderStrings ArrayList
            string sql_command = "SELECT * FROM orders WHERE closed=0 AND order_table=" + "'" + this.table + "'" + "ORDER BY order_index DESC"; //MySQL query
            DataRow[] rows = OrdersSQLDatabase.getRowsArray(sql_command);
            foreach (DataRow row in rows)
            { 
                //Add row to orderStrings
                orderStrings.Add(row["order_string"].ToString() + "#" + row["order_index"]); //us # as splitter between order_string and order_index
            }
            return rows.Length;
        }
        
        
        public ArrayList getUnprintedIndices()
        {
            ArrayList indices = new ArrayList();
            DataRow[] rows = OrdersSQLDatabase.getRowsArray("SELECT order_index FROM orders WHERE order_table='" + this.table + "' AND printed=0 AND closed=0");
            foreach(DataRow row in rows)
            {
                indices.Add((int)row["order_index"]);
            }
            return indices;
        }
        private Product DecodeProduct(string productString)
        {
            //This method decodes the product string and adds to the products to the Order.order Product ArrayList
            //Product Encoding format:
            // Name: [Product]& price [Product price] &quantity: [product quantity]
            // &<[comments]>[[product1 name]<[product1 comments]>([product1 quantity])$[product1 price]$_
            // &<[comments]>[[product1 name]<[product1 comments2]>([product1 quantity2])$[product1 price2]$_
            // &<[comments]>[[product1 name]<[product1 comments3]>([product1 quantity3])$[product1 price3]$_]
            //this a part of the encoded order and it can be found at the createOrderString() method in the Order class
            //at the android app source
            //github repo: https://www.github.com/komoro13/Ravaisi_PDA 
            

            //TODO: Encode the encoding protocol to a file
            if (productString.Split('[')[1].Split(']')[0].Equals(String.Empty)) //if product string has no comments and no
                if (productString.Split('<')[1].Split('>')[0].Equals(String.Empty))
                    return new Product(productString.Split('+')[0].Split(':')[1], productString.Split('+')[1].Split(':')[1], productString.Split('+')[3].Split(':')[1], "", "");
                else return new Product(productString.Split('+')[0].Split(':')[1], productString.Split('+')[1].Split(':')[1], productString.Split('+')[3].Split(':')[1], "", productString.Split('<')[1].Split('>')[0]);

            else
            {
                foreach (String item in productString.Split('[')[1].Split(']')[0].Split('_'))
                {
                    if (item == String.Empty)
                        break;
                    if (item.Split('<')[1].Split('>')[0] != String.Empty)
                        return new Product(productString.Split('+')[0].Split(':')[1], productString.Split('+')[1].Split(':')[1], item.Split('(')[1].Split(')')[0], item.Split('<')[0], productString.Split('<')[1].Split('>')[0]);
                    else
                        return new Product(productString.Split('+')[0].Split(':')[1], productString.Split('+')[1].Split(':')[1], item.Split('(')[1].Split(')')[0], item.Split('<')[0], "");
                }
                return null;
            }
        }
        public void mergeOrders()
        {
            if (mergedOrders) return;
            //This method decodes and adds every order string to the Product ArrayList Order.order      
            getOrderStrings(); //load order strings to orderStrings ArrayList
            string pdString; //String where the product string is going to be saved
            ArrayList prods = new ArrayList(); 
            foreach (string ordString in this.orderStrings)
            {
                prods = new ArrayList();
                pdString = ordString.Split('|')[1].Split('#')[0]; 
                foreach (string prod in pdString.Split('}'))
                {
                    if (prod.Equals("&amp;"))
                        break;
                    prods.Add(prod.Split('{')[1].Replace("&amp;", "+").Replace("&lt;", "<").Replace("&gt;", ">"));

                }

                foreach (String pd in prods)
                {
                        this.order.Add(DecodeProduct(pd));           
                }

            }
            mergedOrders = true;
        }

        public String getOrderString(ArrayList ord)
        {
            //This method returns the final order strng to be displayed
            //to the order tab containing all the product strings
            //and the total price
            //[TO FIX] This method calculates the price however the price is calculated by the android app as well
            float price; //float to store the price
            price = 0; 
            String orderStr = "";
            foreach (Product pr in ord)
            {
                //add each product to the string and sum the price of each product to get the total
                orderStr = orderStr + pr.GetString() + "\n";
                price += float.Parse(pr.price);
            }
            price = price / 10; // [TO FIX] Price has to be divided by 10 to be displayed correctly 
            orderStr += "Συνολο: ";
            orderStr += price.ToString(); 
            if (price.ToString().Contains(","))
                orderStr += "0"; //if price is decimal add a 0
            orderStr += " €"; //add currency symbol
            return orderStr;
        }

        public List<Label> GetLabels()
        {
            List<Label> labels = new List<Label>();
            mergeOrders();
            foreach(Product product in order)
            {
                labels.Add(CopontentsConstructors.createLabel(product.GetString(), "label", new FontFamily("Arial"), 15));
            }
            return labels;
        }
        public float getPrice()
        {
            //This method return the price of the order in float type
            float sum = 0;
            DataRow[] rows = OrdersSQLDatabase.getRowsArray("SELECT price FROM orders WHERE order_table='" + table + "' AND closed=0");
            foreach (DataRow row in rows)
            {
                sum += float.Parse(row["price"].ToString());
            }
            return sum;
        }

        public int getSize()
        {
            int size = 0;
            mergeOrders();
            foreach (Product product in order)
                size += int.Parse(product.quantity);
            return size;
        }
        public ArrayList getAddedOrder(string ordStr)
        {
            //This method adds a single order to the current order that we want to display 
            string ordString = ordStr.Split('#')[0]; 
            string prodString;  
            ArrayList order1 = new ArrayList();
            ArrayList prods = new ArrayList();
            prodString = ordString.Split('|')[1]; //get the product string from the order string
            foreach (string prod in prodString.Split('}'))
            {

                if (prod.Equals("&amp;"))
                    break;
                prods.Add(prod.Split('{')[1].Replace("&amp;", "+").Replace("&lt;", "<").Replace("&gt;", ">"));
            }

            foreach (String pd in prods)
            {
                order1.Add(DecodeProduct(pd));
            }
            return order1;
        }
        public void preview(String tab)
        {
            //This functions shows a dialog that previews the printed order
            //this functionality is mostly used for debugging
            //we use the WinForms PrintPreviewDialog to make things easier
            if (tab.Equals("ALL")) //if the tab with the whole order is selected 
            {                       
                mergeOrders();     //Call the mergeOrders method to load all the orders 
                stringToPrint = getOrderString(this.order); //load the string to be printed
                printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 245 + ((int)(getOrderLines() * (font.Size * 2))));
            }
            else//if a specific section of the order is selected get the specific record from the database
            {
                string sql_command = "SELECT order_string FROM orders WHERE closed=0 AND order_table='" + this.table + "'" + " AND order_index=" + tab;
                this.order.Add(OrdersSQLDatabase.GetString(sql_command)); //adding the selected order of the selected tab to order
                printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 245 + ((int)(getOrderLines() * (font.Size * 2)))); //initializing the printDocument
                stringToPrint = getOrderString(this.order); //getting the string to print 
            }
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog(); 
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage); //adding the event handler to the PrintPage event 
            printPreviewDialog.Document = printDocument; //setting the print document as the document to be prited
            printPreviewDialog.Show();
        }
        public void Print(String tab)
        {
            //This method prints the document
            String filename;
            PrintDialog printDialog = new PrintDialog();
            if (tab.Equals("ALL"))
            {
                mergeOrders();
                printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 235 + ((int)(getOrderLines() * (font.Size * 2))));
                printDocument.DocumentName = CreateDocument(this.table, this.order, tab);
                stringToPrint = getOrderString(this.order);
                setHeader = false;
                printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                printDocument.Print();
                setPrintedAsTrue(tab);
                return;
            }
            String sql_command = "SELECT order_string FROM orders WHERE closed=0 AND order_table='" + this.table + "'";
            if (!tab.Equals("ALL"))
                sql_command += " AND order_index=" + tab;
            string result = OrdersSQLDatabase.GetString(sql_command);
            this.order = getAddedOrder(result);
            stringToPrint = getOrderString(this.order);
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 195 + (int)(getOrderLines() * font.Size));
            printDocument.DocumentName = CreateDocument(this.table, this.order, tab);
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            printDocument.Print();
            setPrintedAsTrue(tab);
            return;

        }
        private string CreateDocument(string orderTable, ArrayList order, string index)
        {
            //This method creates a document of the order to be printed
            return "C:\\Users\\tbogi\\Desktop\\orders\\" + orderTable + "_" + index + ".docx";

        }
        public int getOrderLines()
        {
            //This method gets the lined of the order in order to calculate the length of the page
            int lines = 0;
            for (int x = 0; x < getOrderString(this.order).ToCharArray().Length; x++)
            {
                if (getOrderString(this.order).ToCharArray()[x] == '\n')
                    lines = lines + 1;
            }
            return lines;
        }

        private void printDocument_PrintPage(Object sender, PrintPageEventArgs e)

        {
            //this method prints the order using a printer connected to the machine
            //it is based to an EPSON TM-120II printer
            //it prints a header with info of the buisness and below it prints the order

            int charactersOnPage = 0;
            int linesPerPage = 0;

            if (!setHeader)
            {

                e.Graphics.DrawString("Ραβαΐσι", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, new Point(90, 20));
                e.Graphics.DrawString("Καλλιμασια, Χίος", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new Point(20, 55));
                e.Graphics.DrawString("Τηλεφωνο: 2271103598", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new Point(20, 80));
                e.Graphics.DrawString("Αριθμος παραγγελιας: " + orderId.ToString(), new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 125));
                e.Graphics.DrawString("Τραπεζι: " + table, new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Point(20, 160));
                setHeader = true;//flag set true when header is printed
            }

            //e.Graphics.DrawString(stringToPrint, font, Brushes.Black, new Point(5, 200));


            int y = 200;
            int line_offset = 25;

            foreach (Product product in order)
            {
                e.Graphics.DrawString(product.getHeaderProgramString(), new Font(System.Drawing.FontFamily.GenericMonospace, 12, FontStyle.Bold), Brushes.Black, new Point(5, y));
                y += line_offset;
                if (!product.toppings.Equals(""))//if toppings is not empty print toppings
                {
                    e.Graphics.DrawString(product.toppings, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(5, y));
                    y += line_offset;
                }
                if (!product.comments.Equals(""))//if comments is not empty print comments
                {
                    e.Graphics.DrawString("Σχολια:" + product.comments, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(5, y));
                    y += line_offset * 2;
                }
            }
           
            e.HasMorePages = false;// clear has more pages when finished
            setHeader = false; //when finish reset flag

        }

        private void setPrintedAsTrue(String order_id)
        {
            //This method sets the order as printed to the database
            String sql_command = "UPDATE orders SET printed=1 where closed=0 AND order_table='" + this.table + "'";
            if (order_id != "ALL")
                sql_command += " AND order_index=" + order_id;
            OrdersSQLDatabase.Post(sql_command);
        }
        public void closeOrder()
        {
            //This method sets the order as closed in the database
            String sql_command = "UPDATE orders SET closed=1 where closed=0 AND order_table=" + "'" + this.table + "'";
            OrdersSQLDatabase.Post(sql_command);
        }

        public void setOrderAsLoaded(bool loaded)
        {
            //This method sets order as loaded in the database
            String sql_command;
            if (loaded)
            {
                sql_command = "UPDATE orders SET loaded=1 where closed=0 AND order_table=" + "'" + this.table + "'";
            }
            else
            {
                sql_command = "UPDATE orders SET loaded=0 where closed=0 AND order_table=" + "'" + this.table + "'";
            }
            OrdersSQLDatabase.Post(sql_command);
        }
    }

}