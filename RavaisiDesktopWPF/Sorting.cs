using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace RavaisiDesktopWPF
{
    class Sorting
    {

        public static List<Order> Price(List<Order> list)
        {                       
            bool swapped;
            Order temp;
            int i, j;
            for (i = 0; i < list.Count-1; i++) 
            {
                swapped = false;
                for (j=0;  j < list.Count-1; j++)
                {                   
                    if (((Order)list[j]).getPrice() > ((Order)list[j+1]).getPrice())
                    {
                        temp = (Order)list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped)
                    break;
            }
            return list;
        }
    }
}
