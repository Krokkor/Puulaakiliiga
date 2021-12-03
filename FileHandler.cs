using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;


//Tää oli vaan harjotus tallentamaan ja lataamaan tiedostoja .txt


namespace Puulaakiliiga
{
    public class FileHandler
    {
        private string currentFile = "Pelaajat";

        public string CurrentFile
        {
            get { return currentFile; }
            set { currentFile = value; }
        }

        private string currentPath = @"E:\Tiedostoja\Visual studio projects\Diginikkarit\Puulaakiliiga\";

        public string CurrentPath
        {
            get { return currentPath; }
            set { currentPath = value; }
        }
        


        private readonly string filePathForTesting = @"E:\Tiedostoja\Visual studio projects\Diginikkarit\Puulaakiliiga\Test.txt"; //@ antaa käyttää / merkkiä stringin sisällä
        private readonly string filePathForPlayers = @"E:\Tiedostoja\Visual studio projects\Diginikkarit\Puulaakiliiga\Pelaajat.txt"; 

        public List<string> lines = new List<string>();

        public string PlayertoString(Pelaajat player)
        {
            lines.Add(player.FirstName);
            lines.Add(player.LastName);
            lines.Add(player.age.ToString());
            lines.Add(player.PlayerNumber.ToString());
            lines.Add(player.PelaajaID.ToString());
            lines.Add(player.Position.ToString());
            //lines.Add(player.joukkue);

            string peluri = $"{player.FirstName},{player.LastName},{player.age},{player.PlayerNumber},{player.Position},{player.PelaajaID}";

            return peluri;
        }


        public void WriteLineToFile(List<string> lists)
        {
            //string.join muuttaa List<T>:n arrayksi ja ennen sitä annetaan parametri jolla se erottelee listan objectit toisistaan
            string save = string.Join("\n", lists.ToArray());
            File.WriteAllText(filePathForPlayers, save);
        }

        
        public List<string> TurnPlayersIntoString(List<Pelaajat> players)
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < players.Count; i++)
            {
               lines.Add(PlayertoString(players[i]));
            }
            return lines;
        }


        public void SaveAllPlayers(List<Pelaajat> listOfPlayers)
        {
            WriteLineToFile(TurnPlayersIntoString(listOfPlayers));  
        }


        public List<string> LoadStringsFromFile(string pathfile)
        {
            List<string> loaded = File.ReadAllLines(pathfile).ToList();
            return loaded;
        }


        public Pelaajat StringToPlayer(string loadedPlayers)
        {
            List<Pelaajat> pelaajat = new List<Pelaajat>();
            Pelaajat peluri = new Pelaajat();
            
            
            string[] entries = loadedPlayers.Split(",");
            peluri.FirstName = entries[0];
            peluri.LastName = entries[1];
            peluri.age = Convert.ToInt32(entries[2]);
            //peluri.joukkue = entries[3];
            peluri.PlayerNumber = Convert.ToInt32(entries[3]);
            //ottaa positionin myös numerosta
            peluri.Position = (Pelaajat.position)Enum.Parse(typeof(Pelaajat.position), entries[4]);
            peluri.PelaajaID = Convert.ToInt32(entries[5]);

            pelaajat.Add(peluri);
            
            return peluri;
        }


        public string GetCurrentFilePath()
        {
            Console.WriteLine("Valitse tiedosto");
            Console.WriteLine("Pelaajat, Tiimit, jne. Muista iso alkukirjain");
            currentFile = Console.ReadLine();
            return currentPath + currentFile + ".txt";
        }

        public List<Pelaajat> LoadPlayerDataFromCurrentFile()
        {
            List<string> loadedStrings = LoadStringsFromFile(GetCurrentFilePath());

            List<Pelaajat> pelaajat = new List<Pelaajat>();

            for (int i = 0; i < loadedStrings.Count; i++)
            {
                pelaajat.Add(StringToPlayer(loadedStrings[i]));
            }

            return pelaajat;
        }
    }
}
