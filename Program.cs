using System;
using System.Collections.Generic;
using System.Linq;

namespace Puulaakiliiga
{
    class Program
    {
        static void Main(string[] args)
        {
            DataManager dataManager = new DataManager();
            FileHandler fileHandler = new FileHandler();
            //dataManager.AddTestData();

            ManagerMenu managerMenu = new ManagerMenu(dataManager);
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = managerMenu.MainMenu();
            }

            Console.WriteLine("Kiitos ohjelman käytöstä.");





            //Pelaajat pelaaja = new Pelaajat("Kova","Testi", 6354);
            //int peluri = dataManager.InsertPlayer(pelaaja);
            //dataManager.InsertPlayerToTeam(3, peluri);






            //tallentaa pelaajat
            //List<Pelaaja> pelaajat = new List<Pelaaja>();
            //Pelaaja pelaaja1 = new Pelaaja("asd", "dsa", 99, 125, Pelaaja.position.Keskikentta);
            //Pelaaja pelaaja2 = new Pelaaja("aaa", "beee", 11, 999, Pelaaja.position.Maalivahti);
            //Pelaaja pelaaja3 = new Pelaaja("Toimisko", "tää", 55, 222, Pelaaja.position.Keskuspuolustaja);
            //Pelaaja pelaaja4 = new Pelaaja("Kari", "Grandi", 20, 255, Pelaaja.position.Vasenkeskikentta);
            //pelaajat.AddRange(new List<Pelaaja>() { pelaaja1, pelaaja2, pelaaja3, pelaaja4 });
            //fileHandler.TurnPlayersIntoString(pelaajat);
            //fileHandler.SaveAllPlayers(dataManager.pelaajat);

            //lataa pelaajat
            //List<string> loadedPlayers = fileHandler.LoadStringsFromFile(fileHandler.GetCurrentFilePath());

            //List<Pelaaja> pelaajat = fileHandler.LoadPlayerDataFromCurrentFile();
            //foreach (Pelaaja peluri in pelaajat)
            //{
            //    Console.WriteLine($"{peluri.KokoNimi} {peluri.Position}");
            //}



            //List<Maali> maalit = dataManager.GetGoalsOfTheGame(0);
            //dataManager.PrintGameResult(0);

            //Pelaaja player = new Pelaaja();
            //player.etuNimi = "ASD";
            //player.Position = Pelaaja.position.maalivahti;

            //Console.WriteLine("Eka: " + (Pelaaja.position)2);
            //Console.WriteLine("Eka: " + (int)Pelaaja.position.maalivahti);

            ////haetaan pelaajat listaan
            //List<Pelaaja> pelaajatTiimissaPollot = dataManager.GetPlayersWithTeamID(1);
            ////voidaan tulostaa ne suoraan (GetPlayersWithTeamID laittaa suoraan aakkosjärjestykseen ja ikäjärjestykseen(jos on sama sukunimi))
            //dataManager.PrintPlayerList(dataManager.GetPlayersWithTeamID(1));
            //List<Pelaaja> pelaajatTiimissaPulut = dataManager.GetPlayersWithTeamID(2);
            //dataManager.PrintPlayerList(dataManager.GetPlayersWithTeamID(2));

            //yritys saada penaltyt tulostettua pelaajien kanssa joilla penaltyi. Ekaan penaltyn ID ja toiseen pelaajan ID niin toimii.
            //aika sotkunen, ei toimi monella.
            //dataManager.PrintPlayersWithPenalties(dataManager.GetPlayersWithPenaltyID(3));
            //dataManager.PrintPenaltiesWithPlayerID(dataManager.GetPenaltiesWithPlayerID(11));

            //List<Pelaaja> newList1 = dataManager.pelaaja.FindAll(player => player.age > 15);
            ////muista .Tolist();
            //List<Pelaaja> newlist2 = dataManager.pelaaja.OrderBy(player => player.age).ToList();
            ////voi combottaa
            //List<Pelaaja> newList3 = dataManager.pelaaja.Where(player => player.age > 15 && player.age < 17).ToList();
            //newList3 = newList3.OrderBy(player => player.age).ThenByDescending(player => player.pelaajaNum).ToList();
            ////voidaan käyttää myös näin
            //int ageSum = dataManager.pelaaja.Sum(player => player.age);
            //int memberCount = dataManager.pelaaja.Count();
            //Console.WriteLine($"Pelaajien iän keskiarvo: {ageSum / memberCount}");
            //dataManager.PrintPlayerList(newList3);

            Console.ReadKey();
        }
    }
}
