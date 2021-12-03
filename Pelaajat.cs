using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Pelaajat
    {
        public enum position { None=1, Maalivahti, Laitapuolustaja, Keskuspuolustaja, Vasenkeskikentta, Keskikentta, Oikeakeskikentta, Keskushyokkaaja  }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private int playerNumber;

        public int PlayerNumber
        {
            get { return playerNumber; }
            set { playerNumber = value; }
        }

        public int age;
        public string joukkue;
        public List<int> playerIDs = new List<int>();
        private int pelaajaID;
        //juoskeva ID. Pohjalla metodi GetID() joka nostaa sitä.
        private static int idCounter = 0;

        private position playerPosition = position.None;

        public  position Position
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }
        

        public int PelaajaID
        {
            get { return pelaajaID; }
            set { pelaajaID = value; }
        }

        public string KokoNimi { get => firstName + " " + lastName; }



        public Pelaajat()
        {
            this.PelaajaID = GetID();
        }

        public Pelaajat(string etuNimi, string sukuNimi, int pelaajaNum)
        {
            this.firstName = etuNimi;
            this.lastName = sukuNimi;
            this.playerNumber = pelaajaNum;
        }

        public Pelaajat(string etuNimi, string sukuNimi, int pelaajaNum, int ika)
        {
            this.firstName = etuNimi;
            this.lastName = sukuNimi;
            this.playerNumber = pelaajaNum;
            this.age = ika;
            this.PelaajaID = GetID();
        }

        public Pelaajat(string etuNimi, string sukuNimi, int pelaajaNum, int age, position peliPaikka)
        {
            this.firstName = etuNimi;
            this.lastName = sukuNimi;
            this.age = age;
            //this.joukkue = joukkue;
            this.playerNumber = pelaajaNum;
            this.Position = peliPaikka;
            this.PelaajaID = GetID();
        }


        private int GetID()
        {
            idCounter += 1;
            return idCounter;
        }
        public void AddPlayerToAPenalty(Rangaistukset penalty)
        {
            playerIDs.Add(penalty.RangaistusID);
        }

        public static void PrintPlayer(Pelaajat peluri)
        {
            Console.WriteLine($"Nimi: { peluri.KokoNimi}");
            Console.WriteLine($"Joukkue: { peluri.joukkue}");
            Console.WriteLine($"Pelaajanumero: {peluri.playerNumber}\n");
        }

    }
}
