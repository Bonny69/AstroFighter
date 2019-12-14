using _10_progetto_finale;
using Microsoft.Win32;
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

namespace AstroFighter
{
    /// <summary>
    /// Logica di interazione per Save.xaml
    /// </summary>
    public partial class Save : UserControl
    {
        public Save()
        {
            InitializeComponent();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            List<Giocatore> giocatori = new List<Giocatore>();
            List<string> temp = GestioneFile.Read();
            temp.Add(nome.Text + ";" + GestioneFile.score);
            temp.ForEach(x => giocatori.Add(new Giocatore(x.Split(';')[0], x.Split(';')[1])));
            giocatori = giocatori.OrderBy(x => x.Punteggio).ToList();
            for(int i=0;i<giocatori.Count;i++)
            {
                temp[i] = giocatori[i].Salvataggio;
            }
            GestioneFile.Write(temp);
            Window.GetWindow(this).Close();
        }
        private void Nome_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.Equals(Key.Enter))
                Click(sender, e);
        }
    }
}