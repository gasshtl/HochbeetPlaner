using System;
using System.Collections.Generic;
using System.Text;

namespace Hochbeet_Planer.Models
{
    public class Pflanze //infos Pflanze
    {   //Name Pflanze
        public string Name { get; set; }

        //Wie viele Zellen (breite länge?)
        public int BreiteInZellen { get; set; }
        public int LaengeInZellen { get; set; }

        //Farben im Beet (RGB Werte 0-255)
        public byte FarbeR { get; set; }
        public byte FarbeG { get; set; }
        public byte FarbeB { get; set; }

        //Abstand zwischen Pflanzen
        public int Abstand { get; set; }


    }
}
