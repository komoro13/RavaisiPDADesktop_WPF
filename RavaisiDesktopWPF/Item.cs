using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavaisiDesktopWPF
{
    class Item
    {
        int id;
        string name;
        float extra;
        public String toppings;
        String comments;
        float productPrice;
        string quantity;
        string price;
        public Item(int id, String name, float extra, String toppings, String comments, string quantity, float price)
        {
            this.id = id;
            this.name = name;
            this.toppings = toppings;
            this.comments = comments;
            this.extra = extra;
            this.quantity = quantity;
            this.productPrice = price;
        }

        public Item(String toppings, String comments, String price, String quantity)
        {
            this.toppings = toppings;
            this.comments = comments;
            this.price = price;
            this.quantity = quantity;
        }

        public float calculatePrice()
        {
            float price;
            price = (productPrice + extra) * int.Parse(quantity);
            return price;
        }
        public String getItemString()
        {
            return this.name + "\n" + this.toppings; //+ "<" + this.comments + ">";
        }
        public String getQuantity()
        {
            return quantity.ToString();
        }

        public String getItemToppings()
        { return this.toppings; }

    }
}

