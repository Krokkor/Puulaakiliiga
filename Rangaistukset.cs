using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Rangaistukset
    {
        public string rangaistuksenNimi;
        public string kortti;
        private int rangaistusID;
        public List<int> rangaistusIDs = new List<int>();
        private static int idCountRankkari = 0;
        public int RangaistusID
        {
            get { return rangaistusID; }
            set { rangaistusID = value; }
        }

        public void AddPenaltyToAPlayer(Pelaajat peluri)
        {
            rangaistusIDs.Add(peluri.PelaajaID);
        }

        public Rangaistukset(string rangaistusNimi)
        {
            this.rangaistuksenNimi = rangaistusNimi;
            this.rangaistusID = GetRankkariId();
        }

        public Rangaistukset(string rangaistusNimi, string kortti)
        {
            this.rangaistuksenNimi = rangaistusNimi;
            this.kortti = kortti;
            this.RangaistusID = GetRankkariId();
        }

        private int GetRankkariId()
        {
            idCountRankkari += 1;
            return idCountRankkari;
        }
    }
}
