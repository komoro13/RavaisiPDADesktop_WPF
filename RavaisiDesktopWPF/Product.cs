using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavaisiDesktopWPF
{
    class Product
    {
        //When the order is decoded it is passed in an array of Product objects

        public String name;
        public String price;
        public String quantity;
        public String toppings;
        public String comments;
        public List<Item> items;
       
        
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
        public void ChangeValues(string toppings, string comments, string quantity)
        {
            this.toppings = toppings;
            this.comments = comments;
            this.quantity = quantity;
        }
        public string getProductString()
        {
            return "Name:" + this.name + "& price:" + this.price + "&category:" + " category" + "&quantity:" + this.quantity + "&<" + comments + ">";
        }
        
        public void addItem(Item item)
        {
            items.Add(item);
        }
    }

}
     

