using AstroFighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        Image main = new Image(), projectile = new Image();
        DispatcherTimer shoot = new DispatcherTimer(), enemy = new DispatcherTimer(), spawn = new DispatcherTimer();
        Random rand = new Random();
        Point point = new Point();
        double height = 0, width, movx, movy;
        int angolo = 0, controllo, vite = 3;
        List<Enemy> enemies = new List<Enemy>();
        public MainWindow()
        {
            InitializeComponent();
            shoot.Interval = new TimeSpan(0, 0, 0, 0, 10);
            shoot.Tick += Shoot_Tick;
            spawn.Interval = new TimeSpan(0, 0, 0, 2);
            spawn.Tick += Spawning;
            enemy.Interval = new TimeSpan(0, 0, 0, 0, 100);
            enemy.Tick += Enemy_Tick;
        }
        private void Enemy_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                int x = 1, y = 1;
                Canvas.SetTop(enemies[i], Canvas.GetTop(enemies[i]) + (enemies[i].cos * -20));
                Canvas.SetLeft(enemies[i], Canvas.GetLeft(enemies[i]) + (enemies[i].sin * -20));
                if (enemies[i].cos >= -1 && enemies[i].cos <= 0)
                {
                    x = -1;
                }
                if (enemies[i].sin >= -1 && enemies[i].sin <= 0)
                {
                    y = -1;
                }
                if (((Canvas.GetTop(enemies[i]) + enemies[i].Height / 2) - point.Y) * x <= 0 && ((Canvas.GetLeft(enemies[i]) + enemies[i].Width / 2) - point.X) * y <= 0)
                {
                    canvas.Children.Remove(enemies[i]);
                    enemies.RemoveAt(i);
                    Suono("esplosione");
                    switch (vite)
                    {
                        case 1:
                            vita1.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/cuore_vuoto.png"));
                            pause_screen.Visibility = Visibility.Visible;
                            shoot.Stop();
                            enemy.Stop();
                            spawn.Stop();
                            text.Text = "*GAME OVER*";
                            vite--;
                            break;
                        case 2:
                            vita2.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/cuore_vuoto.png"));
                            vite--;
                            break;
                        case 3:
                            vita3.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/cuore_vuoto.png"));
                            vite--;
                            break;
                    }
                }
            }
        }
        private void Spawning(object sender, EventArgs e)
        {
            double cat1, cat2, ipo;
            string source = "asteroid";
            if (int.Parse(score.Text) >= 100)
            {
                if (rand.Next(0, 2).Equals(1))
                    source = "drone"
                if((2000 - (int.Parse(score.Text)))>=500)
                {
                    spawn.Interval = new TimeSpan(0, 0, 0, 0, (2000 - (int.Parse(score.Text))));
                }
                if (100 - (int.Parse(score.Text) / 10) >= 1)
                {
                    enemy.Interval = new TimeSpan(0, 0, 0, 0, (100 - (int.Parse(score.Text) / 10)));
                }
            }
            Enemy element = new Enemy(source);
            element.Height = canvas.ActualHeight / 7;
            element.Width = element.Height;
            Canvas.SetTop(element, rand.Next(0, (int)(canvas.ActualHeight - element.Height) + 1));
            if (Canvas.GetTop(element).Equals(0) || Canvas.GetTop(element).Equals((int)(canvas.ActualHeight - element.Height)))
            {
                Canvas.SetLeft(element, rand.Next(0, (int)(canvas.ActualWidth - element.Width)));
            }
            else
            {
                if (rand.Next(0, 2).Equals(0))
                    Canvas.SetLeft(element, 0);
                else
                    Canvas.SetLeft(element, canvas.ActualWidth - element.Width);
            }
            cat1 = (Canvas.GetLeft(element) + (element.Width / 2)) - point.X;
            cat2 = (Canvas.GetTop(element) + (element.Height / 2)) - point.Y;
            ipo = Math.Sqrt(Math.Pow(cat1, 2) + Math.Pow(cat2, 2));
            element.sin = cat1 / ipo;
            element.cos = cat2 / ipo;
            element.angolo = (int)((Math.Asin(element.sin) * 180) / Math.PI);
            if (cat2 < 0)
                element.angolo += (90 - element.angolo) * 2;
            RotateTransform rotated = new RotateTransform();
            rotated.Angle = (element.angolo * -1);
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = rotated;
            canvas.Children.Add(element);
            enemies.Add(element);
            if (!enemy.IsEnabled)
                enemy.Start();
        }
        private void Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content.ToString().ToLower())
            {
                case "crediti":
                    Credits credits = new Credits();
                    container.Children.Add(credits);
                    break;
                case "salva e chiudi":
                    container.Children.Add(new Save());
                    break;
            }
        }
        private void Shoot_Tick(object sender, EventArgs e)
        {
            if (canvas.Children.Count - 1 > 0)
            {
                Canvas.SetTop(projectile, Canvas.GetTop(projectile) + movx);
                Canvas.SetLeft(projectile, Canvas.GetLeft(projectile) + movy);
                if ((Canvas.GetTop(projectile) > canvas.ActualHeight || Canvas.GetTop(projectile) < 0) || (Canvas.GetLeft(projectile) > canvas.ActualWidth || Canvas.GetLeft(projectile) < 0))
                {
                    canvas.Children.Remove(projectile);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    Point element = new Point((Canvas.GetLeft(enemies[i]) + enemies[i].Width/2), (Canvas.GetTop(enemies[i]) + enemies[i].Height/2)), sparo = new Point((Canvas.GetLeft(projectile) + projectile.Width/2), (Canvas.GetTop(projectile) + projectile.Height/2));
                    if (sparo.X>element.X-(enemies[i].Width/4) && sparo.X<element.X+ (enemies[i].Width / 4) && sparo.Y>element.Y- (enemies[i].Height / 4) && sparo.Y<element.Y+ (enemies[i].Height / 4))
                    {
                        Suono("esplosione");
                        canvas.Children.Remove(enemies[i]);
                        GestioneFile.score = int.Parse(score.Text) + enemies[i].score;
                        score.Text = GestioneFile.score.ToString();
                        enemies.RemoveAt(i);
                        canvas.Children.Remove(projectile);
                    }
                }
                if (!canvas.Children.Contains(projectile))
                    shoot.Stop();
            }
        }
        private void CaricaFile(object sender, RoutedEventArgs e)
        {
            List<string> temp = GestioneFile.Read();
            if(temp.Count>0)
            {
                if(temp.Count>1)
                {
                    if(temp.Count>2)
                    {
                        nome3.Text = temp[temp.Count - 3].Split(';')[0];
                        punti3.Text = temp[temp.Count - 3].Split(';')[1];
                    }
                    nome2.Text = temp[temp.Count - 2].Split(';')[0];
                    punti2.Text = temp[temp.Count - 2].Split(';')[1];
                }
                nome1.Text = temp[temp.Count - 1].Split(';')[0];
                punti1.Text = temp[temp.Count - 1].Split(';')[1];
            }
        }
        private void GameStart(object sender, MouseButtonEventArgs e)
        {
            if (start_screen.Visibility.Equals(Visibility.Visible))
            {
                start_screen.Visibility = Visibility.Hidden;
                point = new Point((canvas.ActualWidth / 2), (canvas.ActualHeight / 2));
                spawn.Start();
            }
            else
            {
                Shoot();
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!start_screen.Visibility.Equals(Visibility.Visible) && vite > 0)
            {

                if (!pause_screen.Visibility.Equals(Visibility.Visible))
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
                            spawn.Stop();
                            pause_screen.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    if (e.Key.Equals(Key.Escape))
                    {
                        shoot.Start();
                        spawn.Start();
                        enemy.Start();
                        pause_screen.Visibility = Visibility.Hidden;
                    }
                }
            }
        }
        /// <summary>
        /// questo metodo ruota il personaggio in base all'angolo inserito
        /// </summary>
        /// <param name="movimento">il valore da sommare all'angolo</param>
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
            if (!pause_screen.Visibility.Equals(Visibility.Visible))
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
                    controllo = angolo_matematico;
                }
            }
        }
        void Shoot()
        {
            if (!shoot.IsEnabled && !pause_screen.Visibility.Equals(Visibility.Visible))
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/Resources/projectile_main.png");
                img.EndInit();
                projectile.Source = img;
                projectile.Width = 15;
                projectile.Height = 15;
                double angolo_matematico = angolo * Math.PI / 180, x = (canvas.ActualWidth / 2 - 10), y = (canvas.ActualHeight / 2 - 10);
                movx = -20 * Math.Cos(angolo_matematico);
                movy = 20 * Math.Sin(angolo_matematico);
                Canvas.SetTop(projectile, y);
                Canvas.SetLeft(projectile, x);
                canvas.Children.Add(projectile);
                Suono("sparo");
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
                if (canvas.Children[i] is Image)
                {
                    (canvas.Children[i] as Image).Height = (canvas.ActualHeight * (canvas.Children[i] as Image).Height) / height;
                    (canvas.Children[i] as Image).Width = (canvas.ActualWidth * (canvas.Children[i] as Image).Width) / width;
                }
                else
                {
                    (canvas.Children[i] as Enemy).Height = (canvas.ActualHeight * (canvas.Children[i] as Enemy).Height) / height;
                    (canvas.Children[i] as Enemy).Width = (canvas.ActualWidth * (canvas.Children[i] as Enemy).Width) / width;
                }
            }
            if (!height.Equals(0))
            {
                score.FontSize = (canvas.ActualHeight * score.FontSize) / height;
                score.Width = (canvas.ActualHeight * score.Width) / height;
                nome1.FontSize = (canvas.ActualHeight * nome1.FontSize) / height;
                nome1.Width = (canvas.ActualHeight * nome1.Width) / height;
                nome2.FontSize = nome1.FontSize;
                nome2.Width = nome1.Width;
                nome3.FontSize = nome1.FontSize;
                nome3.Width = nome1.Width;
                punti1.FontSize = (canvas.ActualHeight * punti1.FontSize) / height;
                punti1.Width = (canvas.ActualHeight * punti1.Width) / height;
                punti2.FontSize = punti1.FontSize;
                punti3.FontSize = punti1.FontSize;
                punti2.Width = punti1.Width;
                punti3.Width = punti1.Width;
                start.FontSize = (canvas.ActualHeight * start.FontSize) / height;
                start.Width = (canvas.ActualHeight * start.Width) / height;
                point = new Point((canvas.ActualWidth / 2), (canvas.ActualHeight / 2));
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
            main.Height = canvas.ActualHeight / 4;
            main.Width = main.Height;
            main.Source = img;
            canvas.Children.Add(main);
            Canvas.SetTop(main, (canvas.ActualHeight / 2) - (main.Height / 2));
            Canvas.SetLeft(main, (canvas.ActualWidth / 2) - (main.Height / 2));
        }
        void Suono(string traccia)
        {
            SoundPlayer snd=new SoundPlayer();
            switch (traccia)
            {
                case "esplosione":
                    snd = new SoundPlayer(AstroFighter.Properties.Resources.eplosione);
                    break;
                case "sparo":
                    snd = new SoundPlayer(AstroFighter.Properties.Resources.sparo);
                    break;
            } 
            snd.Play();
        }
    }
}
