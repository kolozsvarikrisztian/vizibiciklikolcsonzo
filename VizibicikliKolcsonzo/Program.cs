using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vizibiciklikolcsonzo
{
    class kolcsonzes
    {
        public string nev { get; set; }
        public char jazon { get; set; }
        public int eora { get; set; }
        public int eperc { get; set; }
        public int vora { get; set; }
        public int vperc { get; set; }
        public kolcsonzes(string sor)
        {
            string[] sorelemek = sor.Split(';');
            this.nev = sorelemek[0];
            this.jazon = Convert.ToChar(sorelemek[1]);
            this.eora = Convert.ToInt32(sorelemek[2]);
            this.eperc = Convert.ToInt32(sorelemek[3]);
            this.vora = Convert.ToInt32(sorelemek[4]);
            this.vperc = Convert.ToInt32(sorelemek[5]);
        }
    }
    class Program
    {
        public static List<kolcsonzes> adatok = new List<kolcsonzes>();
        static void Main(string[] args)
        {
            string[] fajl = File.ReadAllLines("kolcsonzesek.txt", Encoding.UTF8);
            for (int i = 1; i < fajl.Length; i++)
            {
                adatok.Add(new kolcsonzes(fajl[i]));
            }
            int kolcsonzesekszama = adatok.Count();

            Console.WriteLine("5. feladat: Napi kölcsönzések száma: {0}", kolcsonzesekszama);
            
            Console.Write("6. feladat: Kérek egy nevet: ");
            string keresettnev = Console.ReadLine();
            int db = 0;
            Console.WriteLine("\t{0} kölcsönzései:", keresettnev);
            for (int i = 0; i < kolcsonzesekszama; i++)
            {
                if (adatok[i].nev == keresettnev)
                {
                    Console.WriteLine("\t{0:00}:{1:00}-{2:00}:{3:00}",
                        adatok[i].eora, adatok[i].eperc, adatok[i].vora, adatok[i].vperc);
                    db++;
                }
            }
            if (db == 0) Console.WriteLine("\tNem volt ilyen nevű kölcsönző!");
            
            Console.Write("7. feladat: Kérek egy időpontot óra:perc alakban: ");
            string oraperc = Console.ReadLine();
            Console.WriteLine("\tA vízen lévő járművek:");
            var tmp = oraperc.Split(':');
            int elvitelperc, visszaperc, keresettperc;
            foreach (var item in adatok)
            {
                elvitelperc = item.eora * 60 + item.eperc;
                visszaperc = item.vora * 60 + item.vperc;
                keresettperc = Convert.ToInt32(tmp[0]) * 60 + Convert.ToInt32(tmp[1]);
                if (elvitelperc <= keresettperc && visszaperc >= keresettperc)
                {
                    Console.WriteLine("\t{0:00}:{1:00}-{2:00}:{3:00} : {4}",
                        item.eora, item.eperc, item.vora, item.vperc, item.nev);
                }
            }
            
            int ossz = 0;
            int felorakszama = 0;
            foreach (var item in adatok)
            {
                ossz = ((item.vora - item.eora) * 60) + (item.vperc - item.eperc);

                felorakszama += (ossz + 29) / 30;
            }
            Console.WriteLine("8. feladat: Napi bevétel: {0} Ft", felorakszama * 2400);
            
            FileStream fnev = new FileStream("f.txt", FileMode.Create);
            StreamWriter fajlba = new StreamWriter(fnev, System.Text.Encoding.UTF8);
            foreach (var item in adatok)
            {
                if (item.jazon == 'F')
                {
                    fajlba.WriteLine("{0:00}:{1:00}-{2:00}:{3:00} : {4}",
                        item.eora, item.eperc, item.vora, item.vperc, item.nev);
                }
            }
            fajlba.Close();
            fnev.Close();
            
            Console.WriteLine("10. feladat: Statisztika");
            adatok.GroupBy(x => x.jazon).OrderBy(x => x.Key).ToList().ForEach(x => Console.WriteLine("\t{0} : {1}", x.Key, x.Count()));
            Console.ReadKey();
        }
    }
}