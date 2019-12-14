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
using System.Windows.Threading;

namespace AstroFighter
{
    /// <summary>
    /// Logica di interazione per Credits.xaml
    /// </summary>
    public partial class Credits : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        public Credits()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            (VisualTreeHelper.GetParent(this) as Grid).Children.Remove(this);
            timer.Stop();
        }
    }
}