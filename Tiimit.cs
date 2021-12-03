using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Tiimit
    {
        public string name;
        public string valmentaja;
        public int maalit;
        public bool kotiJoukkue;
        private int joukkueID;
        public List<int> pelaajaIDs = new List<int>();
        public List<int> valmentajaIDs = new List<int>();
        private static int idTeamCounter = 0;


        public int JoukkueID
        {
            get { return joukkueID; }
            set { joukkueID = value; }
        }

        public Tiimit()
        {
            this.JoukkueID = GetTeamID();
        }

        public Tiimit(string joukkueenNimi)
        {
            this.name = joukkueenNimi;
            this.JoukkueID = GetTeamID();
        }

        public Tiimit(string joukkueenNimi, string valmentaja, int JoukkueID)
        {
            this.name = joukkueenNimi;
            this.valmentaja = valmentaja;
            this.JoukkueID = GetTeamID();
        }

        private int GetTeamID()
        {
            idTeamCounter += 1;
            return idTeamCounter;
        }

        public void AddPlayerToATeam(Pelaajat peluri)
        {
            pelaajaIDs.Add(peluri.PelaajaID);
        }

        public void AddCoachToATeam(Valmentajat koutsi)
        {
            valmentajaIDs.Add(koutsi.valmentajaID);
        }

        public void RemovePlayerfromATeam(int pelaaja)
        {
            pelaajaIDs.RemoveAt(pelaaja);
        }
    }
}
