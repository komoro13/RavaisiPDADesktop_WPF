using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        public static bool FindT(string[] array,  string value)
        {
            if (array == null) return false;
            foreach (string item in  array)
            {
                if (item.Trim() == value)
                    return true;
            }
            return false;
        }
    }
}
