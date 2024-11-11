using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavaisiDesktopWPF
{
    class XAMPP
    {
        static private string XAMPPPath;
        static private void ReadPath()
        {
            //This method reads the path to XAMPP that is stored in a file named filenames.txt
            string path = "./filenames.txt";
            string[] lines = File.ReadAllLines(path);
            XAMPPPath = lines[0].Split('#')[0].Trim();
        }
        public static void Start()
        {
            //This method starts XAMPP
            ReadPath();
            System.Diagnostics.Process XAMPP = new System.Diagnostics.Process(); 
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;//hides the XAMPP window
            startInfo.FileName = XAMPPPath;
            XAMPP.StartInfo = startInfo;//storing the properties to startinfo 
            XAMPP.Start();
        }
    }
    
}
