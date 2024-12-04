using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RavaisiDesktopWPF
{
    class OrderNew
    {
        string table;
        string price;
        List<Product> products;
        public OrderNew(string table, string price)
        {
            this.table = table;
            this.price = price;
        }
        
        public void addProduct(Product product)
        {
            products.Add(product);
        }
        
    }
}
