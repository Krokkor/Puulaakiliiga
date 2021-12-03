using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Valmentajat 
    {
        public string etuNimi;
        public string sukuNimi;
        public string puhelinNumero;
        public List<Tiimit> joukkue = new List<Tiimit>();
        public int valmentajaID;
        private static int coachIdCounter = 0;

        public string ValmentajaKokoNimi { get => etuNimi + " " + sukuNimi; }

        public Valmentajat()
        {

        }

        public Valmentajat(string nimi, string sukuNimi)
        {
            this.etuNimi = nimi;
            this.sukuNimi = sukuNimi;
            this.valmentajaID = GetCoachID();
        }

        public Valmentajat(string nimi, string sukuNimi, string puhNum)
        {
            this.etuNimi = nimi;
            this.sukuNimi = sukuNimi;
            this.puhelinNumero = puhNum;
            this.valmentajaID = GetCoachID();
        }

        private int GetCoachID()
        {
            coachIdCounter += 1;
            return coachIdCounter;
        }
    }
}
