using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RavaisiDesktopWPF
{
    class Searching
    {
        public static bool Find(ArrayList list, string value)
        {
            if (list == null) return false;          
            foreach (string item in list)
            {
                if (item == value)
                    return true;
            }
            return false;
        }
    }
}
