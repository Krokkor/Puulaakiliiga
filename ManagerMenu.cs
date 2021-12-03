using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Puulaakiliiga
{
    public class ManagerMenu
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PuulaakiManageriDBtest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        private readonly DataManager dataManager;

        public ManagerMenu(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        private readonly FileHandler fileHandler;
        public ManagerMenu(FileHandler fileHandler)
        {
            this.fileHandler = fileHandler;
        }


        public bool MainMenu()
        {
            //RANKKAREISTA PITÄÄ TEHÄ JUTUT VIELÄ
            
            //siirtele eri menuihin jutut ja niiden alle niihin liittyvät metodit kun kaikki toimii. Jos se näyttäis selkeämmältä
            Console.Clear();
            Console.WriteLine("\n                                 --VALITSE TOIMINTO--       \n");
            Console.WriteLine(" ======================================================================================");
            Console.WriteLine("||   1. Näytä kaikki pelaajat liigassa.           14. Lisää uusi valmentaja.          ||");
            Console.WriteLine("||   2. Näytä kaikki tiimit liigassa.             15. Näytä valmentaja(t) x tiimissä. ||");
            Console.WriteLine("||   3. Lisää uusi pelaaja.                       16. Lisää valmentaja tiimille.      ||");
            Console.WriteLine("||   4. Lisää uusi joukkue.                       17. Muokkaa valmentajan tietoja.    ||");
            Console.WriteLine("||   5. Muokkaa pelaajan tietoja.                 18. Muokkaa joukkueen tietoja.      ||");
            Console.WriteLine("||   6. Muokkaa pelaajan pelipaikka.              19. Näytä rangaistukset.            ||");
            Console.WriteLine("||   7. Poista pelaaja liigasta.                  20. Lisää uusi rangaistus.          ||");
            Console.WriteLine("||   8. Katso ottelun tulokset ID:llä.            21. Poista rangaistus.              ||");   
            Console.WriteLine("||   9. Kirjaa ottelu. (muokkauksen alla)         22. Lisää rangaistus pelaajalle. (kesken)    ||");   
            Console.WriteLine("||  10. Näytä valmentajien tiedot.                23.                                 ||");
            Console.WriteLine("||  11. Näytä pelaajat x tiimissä.                24.                                 ||");
            Console.WriteLine("||  12. Lisää pelaaja x tiimiin.                  25. Kirjaus testi (kesken)          ||");
            Console.WriteLine("||  13. Poista pelaaja tiimistä.                  26.                                 ||");
            Console.WriteLine("||                                                                                    ||");
            Console.WriteLine("||                                                                                    ||");
            Console.WriteLine("||   100. Pelaaja menu(tänne tulee pelaajajutut)                                      ||");
            Console.WriteLine("||   101. Valmentaja menu                                                             ||");
            Console.WriteLine("||   0. Exit.                                444. Tallenna      555. Lataa            ||");
            Console.WriteLine(" ======================================================================================");

            //stringinä ei haittaa jos painaa menussa vahingossa enter. Inttinä se nakkas errorin
            string valinta = Console.ReadLine();

            switch (valinta)
            {
                case "444":
                    if (AskYesOrNo("Tallennetaanko kaikki pelaajat?"))
                    {
                        //Sanoo että filehandler on null
                        fileHandler.SaveAllPlayers(dataManager.pelaajat);
                        Console.WriteLine("Pelaajat tallennettu.");
                    }
                    else
                        Console.WriteLine("Pelaajia ei tallennettu");
                    Console.ReadKey();
                    break;

                case "555":
                    
                    break;

                case "100":
                    PelaajaMenu();
                    break;

                case "101":
                    ValmentajaMenu();
                    break;

                case "1":
                    //ShowPlayerList();
                    ShowPlayerListSQL();
                    Console.ReadKey();
                    break;

                case "2":
                    //ShowTeamList();
                    ShowTeamListSQL();
                    Console.ReadKey();
                    break;

                case "3":
                    Pelaajat player = CreateNewPlayer();
                    Console.WriteLine("\nLuotu pelaaja:");
                    PrintPlayer(player);
                    if (AskYesOrNo("Tallennetaanko Pelaaja tietokantaan?"))
                    {
                        dataManager.pelaajat.Add(player);
                        Console.WriteLine("Pelaaja lisätty.");
                        Console.ReadKey();
                    }
                    break;

                case "4":
                    Tiimit tiimi = CreateNewTeam();
                    Console.WriteLine("\nLuotu tiimi:");
                    PrintTeam(tiimi);
                    if (AskYesOrNo("Haluatko tallentaa uuden joukkueen?"))
                    {
                        dataManager.joukkueet.Add(tiimi);
                        Console.WriteLine("Joukkue lisätty.");
                        Console.ReadKey();
                    }
                    break;

                case "5":
                    //EditPlayerWithID();
                    dataManager.EditPlayerInfo();
                    Console.WriteLine("Pelaajan tiedot päivitetty");
                    Console.ReadKey();
                    break;

                case "6":
                    ChangePlayerPosition();
                    Console.WriteLine("Pelipaikka päivitetty.");
                    Console.ReadKey();
                    break;

                case "7":
                    Console.WriteLine("Valitse poistettava pelaaja:");
                    Pelaajat player1 = SelectPlayerFromList();
                    if (AskYesOrNo("Haluatko varmasti poistaa pelaajan?"))
                    {
                        dataManager.pelaajat.Remove(player1);
                        Console.WriteLine("Pelaaja poistettu.");
                    }else
                        Console.WriteLine("Pelaajaa ei poistettu");
                        Console.ReadKey();
                    break;

                case "8":
                    ShowMatchResult();
                    Console.ReadKey();
                    break;

                case "9":
                    //tähän uusi matsin kirjaus
                    break;

                case "10":
                    //ShowCoachList();
                    ShowcoachListSQL();
                    Console.ReadKey();
                    break;

                case "11":
                    ShowPlayersInATeamWithID();
                    Console.ReadKey();
                    break;

                case "12":
                    AddPlayerToATeam();
                    Console.WriteLine("Pelaaja lisätty tiimiin.");
                    Console.ReadKey();
                    break;

                case "13":
                    RemovePlayerFromATeam();
                    Console.WriteLine("Pelaaja poistettu tiimistä.");
                    Console.ReadKey();
                    break;

                case "14":
                    Valmentajat koutsi = CreateNewCoach();
                    Console.WriteLine("Luotu valmentaja:");
                    PrintCoach(koutsi);
                    if (AskYesOrNo("Haluatko tallentaa tämän valmentajan?"))
                    {
                        dataManager.valmentajat.Add(koutsi);
                        Console.WriteLine("Valmentaja tallennettu.");
                    }else
                    Console.WriteLine("Valmentajaa ei tallennettu");
                    Console.ReadKey();
                    break;

                case "15":
                    ShowCoachInATeamWithID();
                    Console.ReadKey();
                    break;

                case "16":
                    AddCoachToATeam();
                    Console.WriteLine("Valmentaja lisätty tiimiin.");
                    Console.ReadKey();
                    break;

                case "17":
                    EditCoachWithID();
                    Console.WriteLine("Valmentajan tiedot päivitetty.");
                    Console.ReadKey();
                    break;

                case "18":
                    EditTeamWithID();
                    Console.WriteLine("Joukkueen tiedot päivitetty.");
                    Console.ReadKey();
                    break;

                case "19":
                    ShowPenaltyList();
                    Console.ReadKey();
                    break;

                case "20":
                    Rangaistukset penalty = CreatePenalty();
                    PrintPenalty(penalty);
                    if (AskYesOrNo("Tallennetaanko uusi rangaistus?"))
                    {
                        dataManager.rangaistukset.Add(penalty);
                        Console.WriteLine("Rangaistus tallennettu.");
                    }else
                        Console.WriteLine("Rangaistusta ei talennettu");
                    Console.ReadKey();
                    break;

                case "21":
                    Console.WriteLine("Valitse poistettava rangaistus:");
                    Rangaistukset rangaistus = SelectPenaltyFromList();
                    if (AskYesOrNo("Haluatko varmasti poistaa tämän rangaistuksen?"))
                    {
                        dataManager.rangaistukset.Remove(rangaistus);
                        Console.WriteLine("Rangaistus poistettu");
                    }else
                        Console.WriteLine("Rangaistusta ei poistettu");
                    Console.ReadKey();
                    break;

                case "22":
                    AddPenaltyToPlayer(); //kesken
                    Console.WriteLine("Rangaistus lisätty pelaajalle.");
                    Console.ReadKey();
                    break;

                case "23":
                    
                    Console.ReadKey();
                    break;

                case "24":
                    
                    break;

                case "25":
                    
                    Console.ReadKey();
                    break;

                case "0":
                    return false;

                default:
                    return true;
            }

            return true;
        }


        public Maali FileNewMatch() // KESKEN 
        {
            Console.WriteLine("Lisätään uusi tulos");
            Console.WriteLine("");
            return null; //vaihda sit
        }

        public void ShowMatchResult() //Showmatchlistiä pitää parantaa
        {
            Console.WriteLine("Syötä ottelun ID, jonka tuloksen haluat nähdä");
            ShowMatchList();
            int valinta = Convert.ToInt32(Console.ReadLine());

            dataManager.PrintGameResult(valinta);
        }

        public void ShowMatchList()
        {
            List<Ottelutulokset> matches = dataManager.ottelut;
            for (int i = 0; i < matches.Count; i++)
            {
                Ottelutulokset matsi = matches[i];
                Console.WriteLine($"{i+1}. Ottelun ID:{matsi.Id}"); //tarttis jonku nimeämisjutun tai päivämäärän matseille
            }
        }
        

        public string PrintPlayerWithID(int pelaajaID)
        {
            Pelaajat peluri = dataManager.GetPlayerWithID(pelaajaID);

            string pelaaja = peluri.KokoNimi;
            
            return pelaaja;
        }

        public string PrintTeamWithID(int teamID)
        {
            Tiimit tiimi = dataManager.GetTeamWithID(teamID);

            string joukkue = tiimi.name;
            return joukkue;
        }

        public void AddPenaltyToPlayer()
        {

        }


        public Rangaistukset SelectPenaltyFromList()
        {
            List<Rangaistukset> list = dataManager.rangaistukset;
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i+1}. {list[i].rangaistuksenNimi}");
            }
            Console.WriteLine("Syötä rangaistuksen numero:");
            int valinta = Convert.ToInt32(Console.ReadLine());
            Rangaistukset penaltyX = list[valinta - 1];
            return penaltyX;
        }


        public void PrintPenalty(Rangaistukset penalty)
        {
            Console.WriteLine($"{penalty.rangaistuksenNimi}");
        }


        public Rangaistukset CreatePenalty()
        {
            Console.WriteLine("Lisätään uusi rangaistus:");
            Console.WriteLine("Rangaistuksen nimi:");
            string penaltyName = Console.ReadLine();

            Rangaistukset rangaistus = new Rangaistukset(penaltyName);
            return rangaistus;
        }


        public void ShowPenaltyList()
        {
            List<Rangaistukset> penalties = dataManager.rangaistukset;
            for (int i = 0; i < penalties.Count; i++)
            {
                Rangaistukset penalty = penalties[i];
                Console.WriteLine($"{i+1}. {penalty.rangaistuksenNimi}, {penalty.kortti}");
            }
        }


        public void EditTeamWithID()
        {
            List<Tiimit> tiimit = dataManager.joukkueet;
            Console.WriteLine("\nValitse editoitavan joukkueen ID:\n");
            PrintTeamsWithID();

            int syotettyID = Convert.ToInt32(Console.ReadLine());

            tiimit.Where(x => x.JoukkueID == syotettyID).Select(x =>
            {
                x.name = null;
                while (CheckString(x.name) == false)
                {
                    Console.WriteLine("Syötä uusi joukkoeen nimi:");
                    x.name = Console.ReadLine();
                }
                return x;
            }).ToList();
        }


        public void EditCoachWithID()
        {
            List<Valmentajat> valmentajat = dataManager.valmentajat;
            Console.WriteLine("\nValitse editoitavan valmentajan ID:\n");
            PrintCoachesWithID();

            int valittuID = Convert.ToInt32(Console.ReadLine());

            valmentajat.Where(x => x.valmentajaID == valittuID).Select(x =>
            {
                x.etuNimi = null;
                while (CheckString(x.etuNimi) == false)
                {
                    Console.WriteLine("Syötä uusi etunimi:");
                    x.etuNimi = Console.ReadLine();
                }
                x.sukuNimi = null;
                while (CheckString(x.sukuNimi) == false)
                {
                    Console.WriteLine("Syötä uusi sukunimi:");
                    x.sukuNimi = Console.ReadLine();
                }
                Console.WriteLine("Syötä uusi puhelinnumero:");
                x.puhelinNumero = Console.ReadLine(); //vois ehkä tehdä eri checkstringin ku tähän tarvii numeroita
                return x;
            }).ToList();
        }


        public void AddCoachToATeam()
        {
            Console.WriteLine("\nValitse joukkue johon haluat lisätä valmentajan:\n");
            Tiimit tiimiX = SelectTeamFromList();

            Console.WriteLine("\nValitse valmentaja jonka haluat lisätä valittuun tiimiin:\n");
            Valmentajat coachX = SelectCoachFromList();

            tiimiX.AddCoachToATeam(coachX);
        }

        
        private void ShowCoachList()
        {
            List<Valmentajat> valmentajat = dataManager.valmentajat;
            for (int i = 0; i < valmentajat.Count; i++)
            {
                Valmentajat coach = valmentajat[i];
                Console.WriteLine($"{i+1}. {coach.ValmentajaKokoNimi}. Puh: {coach.puhelinNumero}");
            }
        }

        private void ShowcoachListSQL()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<Valmentajat> valmentajat = connection.Query<Valmentajat>("SELECT * FROM Valmentajat").ToList();
                for (int i = 0; i < valmentajat.Count; i++)
                {
                    Valmentajat coach = valmentajat[i];
                    Console.WriteLine($"{i+1}. {coach.ValmentajaKokoNimi}. Puhelinnumero: {coach.puhelinNumero}");
                }
            }
        }


        public Valmentajat SelectCoachFromList()
        {
            List<Valmentajat> list = dataManager.valmentajat;
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{(i+1)}. Nimi: {list[i].ValmentajaKokoNimi}");
            }
            Console.WriteLine("Syötä valmentajan numero:");
            int valittu = Convert.ToInt32(Console.ReadLine());
            Valmentajat coachX = list[valittu - 1];
            return coachX;
        }


        public Tiimit SelectTeamFromList()
        {
            List<Tiimit> list = dataManager.joukkueet;
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i+1}. Joukkue: {list[i].name}");
            }
            Console.WriteLine("Syötä joukkueen numero:");
            int valittu = Convert.ToInt32(Console.ReadLine());
            Tiimit tiimiX = list[valittu-1];
            return tiimiX;
        }


        private void ShowPlayersInATeamWithID()
        {
            Console.WriteLine("Valitse tiimin ID jonka pelaajat tahdot nähdä");
            ShowTeamList();
            int valintaID = Convert.ToInt32(Console.ReadLine());

            dataManager.PrintPlayerList(dataManager.GetPlayersWithTeamID(valintaID));
        }

        private void ShowCoachInATeamWithID()
        {
            Console.WriteLine("Valitse tiimin ID jonka valmentajan tahdot nähdä");
            ShowTeamList();
            int valintaID = Convert.ToInt32(Console.ReadLine());

            dataManager.PrintCoachList(dataManager.GetCoachesWithTeamID(valintaID));
        }

        private void PrintCoach(Valmentajat koutsi)
        {
            Console.WriteLine($"Nimi: {koutsi.ValmentajaKokoNimi}. Puh: {koutsi.puhelinNumero}\n");
        }

        private void PrintTeam(Tiimit tiimi)
        {
            Console.WriteLine($"Joukkueen nimi: {tiimi.name}?\n");
        }


        private void ShowTeamList()
        {
            List<Tiimit> tiimit = dataManager.joukkueet;
            for (int i = 0; i < tiimit.Count; i++)
            {
                Tiimit tiimi = tiimit[i];
                Console.WriteLine($"{i+1}. {tiimi.name}. Joukkueen ID: {tiimi.JoukkueID}");
            }
        }

        private void ShowTeamListSQL()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<Tiimit> tiimit = connection.Query<Tiimit>("SELECT * FROM Tiimit").ToList();
                for (int i = 0; i < tiimit.Count; i++)
                {
                    Tiimit tiimi = tiimit[i];
                    Console.WriteLine($"{i + 1}. {tiimi.name}. Joukkueen ID: {tiimi.JoukkueID}");
                }
            }
        }


        public Tiimit CreateNewTeam()
        {
            Console.WriteLine("Lisätään joukkue.");
            Console.WriteLine("Anna joukkueen nimi:");
            string teamName = Console.ReadLine();

            Tiimit tiimi = new Tiimit(teamName);
            return tiimi;
        }

        public Valmentajat CreateNewCoach()
        {
            Console.WriteLine("Lisätään uusi valmenta");
            Console.WriteLine("Anna Valmentajan etunimi:");
            string etunimi = Console.ReadLine();
            Console.WriteLine("Anna valmentajan sukunimi:");
            string sukunimi = Console.ReadLine();
            Console.WriteLine("Anna valmentajan puhelinnumero:");
            string puhNum = Console.ReadLine();

            Valmentajat koutsi = new Valmentajat(etunimi, sukunimi, puhNum);
            return koutsi;
        }


        public Pelaajat SelectPlayerFromList()
        {
            List<Pelaajat> list = dataManager.pelaajat;
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{(i+1)}. {list[i].KokoNimi}");
            }
            Console.WriteLine("Syötä pelaajan numero:");
            int selected = Convert.ToInt32(Console.ReadLine());
            Pelaajat player = list[selected-1];
            return player;
        }

        public void PrintTeamsWithID()
        {
            List<Tiimit> tiimit = dataManager.joukkueet;
            for (int i = 0; i < tiimit.Count; i++)
            {
                Tiimit tiimi = tiimit[i];
                Console.WriteLine($"{i+1}. {tiimi.name}. ID: {tiimi.JoukkueID}");
            }
        }

        public void PrintCoachesWithID()
        {
            List<Valmentajat> koutsit = dataManager.valmentajat;
            for (int i = 0; i < koutsit.Count; i++)
            {
                Valmentajat valmentaja = koutsit[i];
                Console.WriteLine($"{i+1}. {valmentaja.ValmentajaKokoNimi}. ID: {valmentaja.valmentajaID}");
            }
        }

        public void PrintPlayersWithID()
        {
            List<Pelaajat> players = dataManager.pelaajat;
            for (int i = 0; i < players.Count; i++)
            {
                Pelaajat player = players[i];
                Console.WriteLine($"{i + 1}. {player.KokoNimi}. Pelaajan ID: {player.PelaajaID}");
            }
        }


        public bool CheckString(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }
            char[] chars = str.ToCharArray();
            char[] nondesireables = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '%', '@', '!', '?', '=', ',', '.', ';', ':', '"' };
            foreach (char c in chars)
            {
                foreach (char undesirable in nondesireables)
                {
                    if(c == undesirable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckInt(string input)
        {
            
            if (Int32.TryParse(input, out int num))
            {
                return false;
            }
            return true;
        }


        public bool AskYesOrNo(string question)
        {
            ConsoleKey keyPressed = ConsoleKey.Spacebar;
            bool ask = true;
            while (ask)
            {
                Console.WriteLine(question);
                Console.WriteLine("Paina K/E tai Y/N");
                keyPressed = Console.ReadKey().Key;

                Console.WriteLine();
                if (keyPressed == ConsoleKey.Y || keyPressed == ConsoleKey.K)
                {
                    return true;
                }

                if (keyPressed == ConsoleKey.Y || keyPressed == ConsoleKey.N || keyPressed == ConsoleKey.E || keyPressed == ConsoleKey.K)
                {
                    ask = false;
                }else
                {
                    Console.WriteLine("Paina siis K tai E tai Y tai N");
                }
            }

            return false;
        }



        public bool PelaajaMenu()
        {
            Console.Clear();
            Console.WriteLine("\n                                 --Pelaaja Menu--       \n");
            Console.WriteLine(" =====================================================================================");
            Console.WriteLine("||   1. Näytä kaikki pelaajat liigassa.           6. Näytä pelaaja x tiimissä.       ||");
            Console.WriteLine("||   2. Lisää uusi pelaaja.                       7. Lisää pelaaja x tiimiin.        ||");
            Console.WriteLine("||   3. Muokkaa pelaajan tietoja                  8. Poista pelaaja tiimistä.        ||");
            Console.WriteLine("||   4. Muokkaa pelaajan pelipaikka                                                  ||");
            Console.WriteLine("||   5. Poista pelaaja liigasta                                                      ||");
            Console.WriteLine("||                                                                                   ||");
            Console.WriteLine("||                                                                                   ||");
            Console.WriteLine("||   0. Main menu.                                                                   ||");
            Console.WriteLine(" =====================================================================================");

            string valinta = Console.ReadLine();

            switch (valinta)
            {
                case "1":
                    //ShowPlayerList();
                    ShowPlayerListSQL();
                    Console.ReadKey();
                    break;

                case "2":
                    Pelaajat player = CreateNewPlayer();
                    Console.WriteLine("\nLuotu pelaaja:");
                    PrintPlayer(player);
                    if (AskYesOrNo("Tallennetaanko Pelaaja tietokantaan?"))
                    {
                        dataManager.pelaajat.Add(player);
                        Console.WriteLine("Pelaaja lisätty.");
                        Console.ReadKey();
                    }
                    break;

                case "3":
                    //EditPlayerWithID();
                    dataManager.EditPlayerInfo();
                    Console.WriteLine("Pelaajan tiedot päivitetty");
                    Console.ReadKey();
                    break;

                case "4":
                    ChangePlayerPosition();
                    Console.WriteLine("Pelipaikka päivitetty.");
                    Console.ReadKey();
                    break;

                case "5":
                    Console.WriteLine("Valitse poistettava pelaaja:");
                    Pelaajat player1 = SelectPlayerFromList();
                    if (AskYesOrNo("Haluatko varmasti poistaa pelaajan?"))
                    {
                        dataManager.pelaajat.Remove(player1);
                        Console.WriteLine("Pelaaja poistettu.");
                    }
                    else
                        Console.WriteLine("Pelaajaa ei poistettu");
                    Console.ReadKey();
                    break;

                case "6":
                    ShowPlayersInATeamWithID();
                    Console.ReadKey();
                    break;

                case "7":
                    AddPlayerToATeam();
                    Console.WriteLine("Pelaaja lisätty tiimiin.");
                    Console.ReadKey();
                    break;

                case "8":
                    RemovePlayerFromATeam();
                    Console.WriteLine("Pelaaja poistettu tiimistä.");
                    Console.ReadKey();
                    break;

                case "0":
                    return false;

                default:
                    return PelaajaMenu();
            }
            return PelaajaMenu();
        }

        private void ShowPlayerListSQL()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<Pelaajat> pelaajat = connection.Query<Pelaajat>("SELECT * FROM Pelaajat").ToList();
                for (int i = 0; i < pelaajat.Count; i++)
                {
                    Pelaajat player = pelaajat[i];
                    Console.WriteLine($"{i + 1}. {player.FirstName} {player.LastName}. Pelaajapaikalla: {player.Position}");
                }
            }
        }

        private void ShowPlayerList()
        {
            List<Pelaajat> players = dataManager.pelaajat;
            for (int i = 0; i < players.Count; i++)
            {
                Pelaajat player = players[i];
                Console.WriteLine($"{i + 1}. {player.FirstName} {player.LastName}. Pelaajapaikalla: {player.Position}");
            }
        }

        public Pelaajat CreateNewPlayer()
        {
            Console.WriteLine("Lisätään pelaaja.");
            string firstName = null;
            while(CheckString(firstName) == false)
            {
                Console.WriteLine("Anna pelaajan etunimi");
                firstName = Console.ReadLine();
            }
            string lastName = null;
            while (CheckString(lastName) == false)
            {
                Console.WriteLine("Anna pelaajan sukunimi");
                lastName = Console.ReadLine();
            }
            
            Console.WriteLine("Anna pelaajan pelinumero");
            int peliNum = Convert.ToInt32(Console.ReadLine());
            
            Console.WriteLine("Anna pelaajan ikä");
            int age = Convert.ToInt32(Console.ReadLine());

            Pelaajat player = new Pelaajat(firstName, lastName, peliNum, age);

            return player;
        }



        public void EditPlayerWithID()
        {
            List<Pelaajat> players = dataManager.pelaajat;
            Console.WriteLine("\nValitse editoitavan pelaajan ID\n");
            PrintPlayersWithID();

            int syotettuPelaajaID = Convert.ToInt32(Console.ReadLine()); //vois olla joku että jos painaa vaan enteriä nii se kysyy uudestaan

            //linq etsii pelaaja ID:n syötetyllä intillä joka vastaa ID:tä ja sitten voidaan käydä muokkaamassa sille kaikki jutut.
            players.Where(x => x.PelaajaID == syotettuPelaajaID).Select(x => 
            {
                x.FirstName = null;
                while (CheckString(x.FirstName) == false)
                {
                    Console.WriteLine("Syötä uusi etunimi:");
                    x.FirstName = Console.ReadLine(); 
                }
                x.LastName = null;
                while (CheckString(x.LastName) == false)
                {
                    Console.WriteLine("Syötä uusi sukunimi"); 
                    x.LastName = Console.ReadLine(); 
                } //lisää pelaajanumeron ja iän muokkaus jos tarvii. Ehkä viimeistään sillon ku int check toimii.
                return x; 
            }).ToList();
        }


        public void ChangePlayerPosition()
        {
            List<Pelaajat> players = dataManager.pelaajat;
            Console.WriteLine("Valitse pelaaja:");
            PrintPlayersWithID();
            int syotettuPelaaID = Convert.ToInt32(Console.ReadLine());

            players.Where(x => x.PelaajaID == syotettuPelaaID).Select(x =>
            {
                Console.WriteLine("Valitse numerolla uusi pelaajapaikka.");
                Console.WriteLine($"Pelaajapaikat: \n" +
                                    $"1. {(Pelaajat.position)1}, 2. {(Pelaajat.position)2}, 3. {(Pelaajat.position)3}, 4. {(Pelaajat.position)4},\n" +
                                    $"5. {(Pelaajat.position)5}, 6. {(Pelaajat.position)6}, 7. {(Pelaajat.position)7}, 8. {(Pelaajat.position)8}");
                int valinta = Convert.ToInt32(Console.ReadLine());
                if (valinta == 1)
                {
                    x.Position = (Pelaajat.position)1;
                }else if (valinta == 2)
                {
                    x.Position = (Pelaajat.position)2;
                }else if (valinta == 3)
                {
                    x.Position = (Pelaajat.position)3;
                }else if (valinta == 4)
                {
                    x.Position = (Pelaajat.position)4;
                }else if (valinta == 5)
                {
                    x.Position = (Pelaajat.position)5;
                }else if (valinta == 6)
                {
                    x.Position = (Pelaajat.position)6;
                }else if (valinta == 7)
                {
                    x.Position = (Pelaajat.position)7;
                }else if (valinta == 8)
                {
                    x.Position = (Pelaajat.position)8;
                }else
                    Console.WriteLine("Syötä numero!");
            return x;
            }).ToList();
        }


        //Tästä ois kiva saada versio joka toimii ID:llä
        private void AddPlayerToATeam()
        {
            Console.WriteLine("\nValitse joukkue johon haluat lisätä pelaajan:\n");
            Tiimit tiimiX = SelectTeamFromList();

            Console.WriteLine("\nValitse pelaaja jonka haluat lisätä valittuun tiimiin\n");
            Pelaajat pelaajaX = SelectPlayerFromList();

            tiimiX.AddPlayerToATeam(pelaajaX);
        }


        private void RemovePlayerFromATeam()
        {
            Console.WriteLine("\nValitse joukkue josta haluat poistaa pelaajan:\n");
            ShowTeamList();
            int tiimiValinta = Convert.ToInt32(Console.ReadLine());
            Tiimit tiimiX = dataManager.GetTeamWithID(tiimiValinta);
            
            Console.WriteLine("\nValitse pelaaja jonka haluat poistaa tiimistä:\n");
            dataManager.PrintPlayerList(dataManager.GetPlayersWithTeamID(tiimiValinta));
            int valinta = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            if (AskYesOrNo("Haluatko varmasti poistaa pelaajan?"))
            {
                tiimiX.RemovePlayerfromATeam(valinta-1); 
            }
        }


        public void PrintPlayer(Pelaajat player)
        {
            Console.WriteLine($"Nimi: {player.KokoNimi}");
            Console.WriteLine($"Ikä: {player.age}");
            Console.WriteLine($"Pelinumero: {player.PlayerNumber}");
            Console.WriteLine($"Joukkue: {player.joukkue}");
            Console.WriteLine($"Pelipaikka: {player.Position}");
        } 
        
        public bool ValmentajaMenu ()
        {
            Console.Clear();
            Console.WriteLine("\n                                 --Valmentaja Menu--       \n");
            Console.WriteLine(" ======================================================================================");
            Console.WriteLine("||   1. Näytä kaikki valmentajat.                 4. Näytä valmentajat tiimissä X.    ||");
            Console.WriteLine("||   2. Lisää uusi valmentaja.                    5. Muokkaa valmentajan tietoja.     ||");
            Console.WriteLine("||   3. Lisää valmentaja joukkueelle.                                                 ||");
            Console.WriteLine("||                                                                                    ||");
            Console.WriteLine("||                                                                                    ||");
            Console.WriteLine("||   0. Main menu.                                                                    ||");
            Console.WriteLine(" ======================================================================================");

            string valinta = Console.ReadLine();

            switch (valinta)
            {
                case "0":
                    return false;

                case "1":
                    ShowCoachList();
                    Console.ReadKey();
                    break;

                case "2":
                    Valmentajat koutsi = CreateNewCoach();
                    Console.WriteLine("Luotu valmentaja:");
                    PrintCoach(koutsi);
                    if (AskYesOrNo("Haluatko tallentaa tämän valmentajan?"))
                    {
                        dataManager.valmentajat.Add(koutsi);
                        Console.WriteLine("Valmentaja tallennettu.");
                    }
                    else
                        Console.WriteLine("Valmentajaa ei tallennettu");
                    Console.ReadKey();
                    break;

                case "3":
                    AddCoachToATeam();
                    Console.WriteLine("Valmentaja lisätty tiimiin.");
                    Console.ReadKey();
                    break;

                case "4":
                    ShowCoachInATeamWithID();
                    Console.ReadKey();
                    break;

                case "5":
                    EditCoachWithID();
                    Console.WriteLine("Valmentajan tiedot päivitetty.");
                    Console.ReadKey();
                    break;

                default:
                   return ValmentajaMenu();
            }

            return ValmentajaMenu();
        }
    }
}
