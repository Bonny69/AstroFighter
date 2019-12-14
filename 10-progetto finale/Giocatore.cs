using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroFighter
{
    class Giocatore
    {
        public string Nome { get; set; }
        public int Punteggio { get; set; }
        public Giocatore(string nome, string punteggio)
        {
            if (nome.Equals(""))
                nome = "nessuno";
            Nome = nome;
            try
            {
                Punteggio = int.Parse(punteggio);
            }
            catch
            {
                Punteggio = 0;
            }
        }
        public string Salvataggio
        {
            get { return Nome + ";" + Punteggio; }
        }
    }
}
