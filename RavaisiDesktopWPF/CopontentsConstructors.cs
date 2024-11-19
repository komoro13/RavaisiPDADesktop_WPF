using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace RavaisiDesktopWPF
{
    class CopontentsConstructors
    {
        static public Button createButton(string text, string name, Point location, int width, int height, Action<object, EventArgs> click)
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
            Canvas.SetLeft(b, location.X);
            Canvas.SetTop(b, location.Y);
            b.Click += new RoutedEventHandler(click);
            return b;
        }
        static public Label createLabel(string text, string name, FontFamily fontFamily, int fontSize)
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
    }
}
