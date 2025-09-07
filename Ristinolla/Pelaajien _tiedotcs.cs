using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ristinolla
{
    public struct Pelaajien_tiedotcs 
    {
        public string Etunimi { get; set; }
            
        public string Sukunimi { get; set; }
        public string Syntymavuosi { get; set; }

        public int Voitot { get; set; }
        public int TasaPelit { get; set; }
        public int Haviot { get; set; }
        public double Peliaika { get; set; }

       
        // Määrittää muodon, missä struckti näkyy listwiewssä
        public override string ToString()
        {
            return $"Pelaaja :{Etunimi} {Sukunimi}, Syntynyt:{Syntymavuosi}, Voitot: {Voitot}, Häviöt: {Haviot}, Tasapelit: {TasaPelit}, Pelattu aika: {Peliaika} min ";
        }

    }
}
