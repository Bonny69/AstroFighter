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

namespace _10_progetto_finale
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image main = new Image();
        DispatcherTimer shoot = new DispatcherTimer(), enemy = new DispatcherTimer();
        Random rand = new Random();
        Point point = new Point();
        double height = 0, width;
        int angolo = 0, controllo, movx, movy;
        bool pause = false;
        public MainWindow()
        {
            InitializeComponent();
            shoot.Interval = new TimeSpan(0, 0, 0, 0, 100);
            shoot.Tick += Shoot_Tick;
        }
        private void Shoot_Tick(object sender, EventArgs e)
        {
            for (int i = canvas.Children.Count - 1; i > 0; i--)
            {
                Canvas.SetTop(canvas.Children[i], Canvas.GetTop(canvas.Children[i]) + movx);
                Canvas.SetLeft(canvas.Children[i], Canvas.GetLeft(canvas.Children[i]) + movy);
                if((Canvas.GetTop(canvas.Children[i])>canvas.ActualHeight|| Canvas.GetTop(canvas.Children[i])<0)||(Canvas.GetLeft(canvas.Children[i])>canvas.ActualWidth|| Canvas.GetLeft(canvas.Children[i])<0))
                {
                    canvas.Children.RemoveAt(i);
                }
            }
            if (canvas.Children.Count - 1 == 0)
                shoot.Stop();
            else
                shoot.Start();
        }
        private void GameStart(object sender, MouseButtonEventArgs e)
        {
            if(point.Equals(new Point()))
            {
                start_screen.Visibility = Visibility.Hidden;
                point = new Point((canvas.ActualWidth / 2), (canvas.ActualHeight / 2));
            }
            else
            {
                Shoot();
            }
            //vita1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/cuore_vuoto.png"));
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(!point.Equals(new Point()))
            {

                if (!pause)
                {
                    switch (e.Key)
                    {
                        case Key.D:
                            Muovi(10);
                            break;
                        case Key.Right:
                            Muovi(10);
                            break;
                        case Key.A:
                            Muovi(-10);
                            break;
                        case Key.Left:
                            Muovi(-10);
                            break;
                        case Key.Space:
                            Shoot();
                            break;
                        case Key.Escape:
                            shoot.Stop();
                            enemy.Stop();
                            pause_screen.Visibility = Visibility.Visible;
                            pause = true;
                            break;
                        case Key.U:
                            angolo = 0;
                            Muovi(0);
                            break;
                    }
                }
                else
                {
                    if (e.Key.Equals(Key.Escape))
                    {
                        shoot.Start();
                        //enemy.Start();
                        pause_screen.Visibility = Visibility.Hidden;
                        pause = false;
                    }
                }
            }
        }
        void Muovi(int movimento)
        {
            angolo += movimento;
            switch (angolo)
            {
                case 360:
                    angolo = 0;
                    break;
                case -360:
                    angolo = 0;
                    break;
            }
            RotateTransform rotated = new RotateTransform();
            rotated.Angle = angolo;
            main.RenderTransformOrigin = new Point(0.5, 0.5);
            main.RenderTransform = rotated;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double cat1 = e.GetPosition(this).X - point.X;
            double cat2 = e.GetPosition(this).Y - point.Y;
            double ipo = Math.Sqrt(Math.Pow(cat1, 2) + Math.Pow(cat2, 2));
            double sin = cat1 / ipo;
            int angolo_matematico = (int)((Math.Asin(sin) * 180) / Math.PI);
            while (angolo > 360)
            {
                angolo -= 360;
            }
            while (angolo < -360)
            {
                angolo += 360;
            }
            if (cat2 > 0)
                angolo_matematico += ((90 - angolo_matematico) * 2);
            if (!controllo.Equals(angolo_matematico))
            {
                angolo = 0;
                Muovi(angolo_matematico);
                controllo= angolo_matematico;
            }
        }
        void Shoot()
        {
            if(!shoot.IsEnabled)
            {
                Ellipse raggio = new Ellipse();
                raggio.Width = 20;
                raggio.Height = 20;
                raggio.Fill = Brushes.Red;
                double angolo_matematico = angolo * Math.PI / 180, x=Canvas.GetLeft(main),y=Canvas.GetTop(main)+(main.Height/2);
                switch(angolo)
                {
                    case 0:
                        x += (main.Width / 2);
                        y -= (main.Height / 2);
                        movx = -20;
                        movy = 0;
                        break;
                    case 90:
                        x += main.Width;
                        movy = -20;
                        movx = 0;
                        break;
                    case 180:
                        movx = 20;
                        movy = 0;
                        break;
                    case -90:
                        x += main.Width;
                        movy = 20;
                        movx = 0;
                        break;
                    default:
                        if (angolo > 0)
                        {
                            if (angolo > 90)
                            {
                                if (angolo > 180)
                                {
                                    movy = -20;
                                    movx = 20;
                                    //MessageBox.Show("1");
                                }
                                else
                                {
                                    movy = 20;
                                    movx = 20;
                                    //MessageBox.Show("2");
                                    x += main.Width;
                                }
                            }
                            else
                            {
                                movx = -10;
                                movy = 20;
                                //MessageBox.Show("3");
                                x += main.Width;
                            }
                        }
                        else
                        {
                            movx = -20;
                            movy = -20;
                            //MessageBox.Show("4");
                        }
                        break;
                }
                Canvas.SetTop(raggio, y);
                Canvas.SetLeft(raggio, x);
                temp.Text = x.ToString() + "|" + y;
                canvas.Children.Add(raggio);
                shoot.Start();
            }
        }
        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (shoot.IsEnabled)
                shoot.Stop();
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                Canvas.SetTop(canvas.Children[i], (canvas.ActualHeight * Canvas.GetTop(canvas.Children[i])) / height);
                Canvas.SetLeft(canvas.Children[i], (canvas.ActualWidth * Canvas.GetLeft(canvas.Children[i])) / width);
                if(i>0)
                {
                    (canvas.Children[i] as Ellipse).Height = (canvas.ActualHeight * (canvas.Children[i] as Ellipse).Height) / height;
                    (canvas.Children[i] as Ellipse).Width = (canvas.ActualWidth * (canvas.Children[i] as Ellipse).Width) / width;
                }
                else
                {
                    main.Height= (canvas.ActualHeight * main.Height) / height;
                    main.Width = (canvas.ActualWidth * main.Width) / width;
                }
            }
            if(!height.Equals(0))
            {
                score.FontSize = (canvas.ActualHeight * score.FontSize) / height;
                score.Width = (canvas.ActualHeight * score.Width) / height;
                start.FontSize = (canvas.ActualHeight * start.FontSize) / height;
                start.Width = (canvas.ActualHeight * start.Width) / height;
                point = new Point((canvas.ActualWidth/2), (canvas.ActualHeight/2));
                if (canvas.Children.Count > 1)
                    shoot.Start();
            }
            height = canvas.ActualHeight;
            width = canvas.ActualWidth;
        }
        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/Resources/main.png");
            img.EndInit();
            main.Height = canvas.ActualHeight / 3.5;
            main.Width = main.Height;
            main.Source = img;
            canvas.Children.Add(main);
            Canvas.SetTop(main, (canvas.ActualHeight / 2) - (main.Height / 2));
            Canvas.SetLeft(main, (canvas.ActualWidth / 2) - (main.Height / 2));
        }
    }
}
