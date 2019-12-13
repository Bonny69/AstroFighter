using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _10_progetto_finale
{
    /// <summary>
    /// Logica di interazione per Enemy.xaml
    /// </summary>
    public partial class Enemy : UserControl
    {
        string path = "pack://application:,,,/Resources/";
        public int score;
        public double angolo, sin, cos;
        public Enemy(string source)
        {
            path += source + ".png";
            InitializeComponent();
            switch(source)
            {
                case "asteroid":
                    score = 10;
                    break;
                case "drone":
                    score = 25;
                    break;
                default:
                    score = 0;
                    path = "pack://application:,,,/Resources/MissingNo.png";
                    break;
            }
            element.Source = new BitmapImage(new Uri(path));
        }
    }
}
