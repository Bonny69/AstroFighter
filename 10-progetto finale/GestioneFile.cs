using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace _10_progetto_finale
{
    class GestioneFile
    {
        static public int score;
        static public string path= Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ "punteggi.txt";
        static public List<string> Read()
        {
            List<string> temp = new List<string>();
            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        while (!reader.EndOfStream)
                            temp.Add(reader.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return temp;
        }
        static public void Write(List<string> elementi)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    elementi.ForEach(x => writer.WriteLine(x));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
