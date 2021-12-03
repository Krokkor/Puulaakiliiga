using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Puulaakiliiga
{
    public class DataManager
    {
        public List<Pelaajat> pelaajat = new List<Pelaajat>();
        public List<Tiimit> joukkueet = new List<Tiimit>();
        public List<Valmentajat> valmentajat = new List<Valmentajat>();
        public List<Rangaistukset> rangaistukset = new List<Rangaistukset>();
        public List<Maali> maalit = new List<Maali>();
        public List<GameEvent> gameEvents = new List<GameEvent>();
        public List<Ottelutulokset> ottelut = new List<Ottelutulokset>();
        public List<string> pelaajaID = new List<string>();
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PuulaakiManageriDBtest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public void EditPlayerInfo()
        {
            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Valitse editoitavan pelaajan ID");
                ShowPlayerlistWithID();
                //Pitää saada SQL:stä pelaaja ID
                int pelaajaID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Anna uusi etunimi:");
                string etuNimi = Console.ReadLine();
                Console.WriteLine("Anna uusi sukunimi:");
                string sukuNimi = Console.ReadLine();
                Console.WriteLine("Anna uusi pelaajanumero:");
                int pelaajaNum = Convert.ToInt32(Console.ReadLine());
                //lisää möyhemmin positio jne jos lisäät ne DB:n

                connection.Execute($"UPDATE Pelaajat SET firstName = '{etuNimi}', lastName = '{sukuNimi}', playerNumber = {pelaajaNum} WHERE Id = {pelaajaID}");
            }
        }

        public void ShowPlayerlistWithID()
        {
            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                List<Pelaajat> pelaajat = connection.Query<Pelaajat>("SELECT * FROM Pelaajat").ToList();
                List<string> pelaajaIDs = connection.Query<string>("SELECT * FROM Pelaajat").ToList(); //jostain syystä ottaa vaan ID:n joten käytän sitä vaan näyttämään oikean ID:n SQL:stä
                for (int i = 0; i < pelaajat.Count; i++)
                {
                    Pelaajat pelaaja = pelaajat[i];
                    Console.WriteLine($"{1 + i}. {pelaaja.KokoNimi}, {pelaaja.PlayerNumber}, {pelaaja.Position}. ID: {pelaajaIDs[i]}");
                }
            }
        }


        public int InsertPlayer(Pelaajat pelaaja)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                int IDGiven = connection.QuerySingle<int>("INSERT INTO Pelaajat(firstName,lastName,playerNumber) OUTPUT INSERTED.Id VALUES (@FirstName,@LastName,@PlayerNumber)", pelaaja);
                return IDGiven;
            }
        }

        public void InsertPlayerToTeam(int teamid, int playerid)
        {
            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                int IDGiven = connection.Execute($"INSERT INTO TeamComposition(TeamId,PelaajaId) VALUES ({teamid}, {playerid})");
            }
        }


        public void PrintGameResult(int matchID)
        {
            Ottelutulokset match = this.ottelut.Find(x => x.Id == matchID);
            Tiimit kotiJoukkue = GetTeamWithID(match.KotiJoukkueId);
            Tiimit vierasJoukkue = GetTeamWithID(match.VierasJoukkueId);
            List<Maali> goals = GetGoalsOfTheGame(matchID);
            int goalsHome = goals.Count(x => x.TeamId == kotiJoukkue.JoukkueID);
            int goalsAway = goals.Count(x => x.TeamId == vierasJoukkue.JoukkueID);

            Console.WriteLine("\nOttelun tulos:");
            Console.WriteLine($"{kotiJoukkue.name} {goalsHome} - {goalsAway} {vierasJoukkue.name}");

        }

        public List<Maali> GetGoalsOfTheGame(int matchId)
        {
            //haetaan eventeistä matsiin liittyvät maalit

            Ottelutulokset match = this.ottelut.Find(x => x.Id == matchId);
            List<Maali> goals = new List<Maali>();
            foreach (int gameEventId in match.GameEvents)
            {
                GameEvent gameEvent = this.gameEvents.Find(x => x.Id == gameEventId);
                //verrataan tyyppiä / luokkaa
                if (typeof(Maali) == gameEvent.GetType())
                {
                    //muuttaa gameEventin Maali tyypiksi
                    goals.Add((Maali)gameEvent);
                }
            }

            return goals;
        }


        public Ottelutulokset GetMatchWithID(int id)
        {
            Ottelutulokset ottelu = ottelut.Find(ottelu => ottelu.Id == id);
            return ottelu;
        }


        public Pelaajat GetPlayerWithID(int id)
        {
            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                Pelaajat pelaaja = connection.Query<Pelaajat>("SELECT * FROM Pelaajat WHERE Id=@id", new { id = id }).First();
                return pelaaja;
            } 
        }



        public void TestDBConnection()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<Pelaajat> x = connection.Query<Pelaajat>("SELECT * FROM Pelaajat").ToList();
            }
        }



        public Valmentajat GetCoachWithID(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                Valmentajat koutsi = connection.Query<Valmentajat>("SELECT * FROM Valmentajat WHERE Id=@id", new { id = id }).First();
                return koutsi;
            }
        }

        

        public Tiimit GetTeamWithID(int id)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                Tiimit tiimi = connection.Query<Tiimit>("SELECT * FROM Tiimit WHERE Id=@id", new { id = id }).First();
                return tiimi;
            }
        }



        public Rangaistukset GetPenaltyWithID(int id)
        {
            Rangaistukset penalty = rangaistukset.Find(rankkari => rankkari.RangaistusID == id);
            return penalty;
        }

        public List<Valmentajat> GetCoachesWithTeamID(int teamid)
        {
            Tiimit team = GetTeamWithID(teamid);

            List<Valmentajat> valmentajat = new List<Valmentajat>();
            foreach (int coachId in team.valmentajaIDs)
            {
                Valmentajat koutsi = GetCoachWithID(coachId);
                valmentajat.Add(koutsi);
            }
            return valmentajat;
            
        }

        public List<Pelaajat> GetPlayersWithTeamID(int teamId)
        {
            Tiimit team = GetTeamWithID(teamId);

            List<Pelaajat> players = new List<Pelaajat>();
            foreach (int pelaajaId in team.pelaajaIDs)
            {
                Pelaajat peluri = GetPlayerWithID(pelaajaId);
                players.Add(peluri);
            }
            return players; //.OrderBy(player => player.sukuNimi).ThenBy(player => player.age).ToList();
        }

        public List<Pelaajat> GetPlayersWithPenaltyID(int penaltyId)
        {
            Rangaistukset penalty = GetPenaltyWithID(penaltyId);

            List<Pelaajat> players = new List<Pelaajat>();
            foreach (int rankkariID in penalty.rangaistusIDs)
            {
                Pelaajat peluri = GetPlayerWithID(rankkariID);
                players.Add(peluri);
            }
            return players;
        }

        public void PrintCoachList(List<Valmentajat> list)
        {
            int i = 0;
            Console.WriteLine("\n---VALMENTAJA LISTA---\n");
            foreach (Valmentajat koutsi in list)
            {
                Console.WriteLine($"{(i+1)}. Nimi: {koutsi.ValmentajaKokoNimi}. Puh: {koutsi.puhelinNumero}");
                i++;
            }
        }

        public void PrintPlayerList(List<Pelaajat> list)
        {
            int i = 0;
            Console.WriteLine("\n---PELAAJA LISTA---\n");
            foreach (Pelaajat player in list)
            {
                Console.WriteLine($"{i+1}. Nimi: {player.KokoNimi}, Pelinumero: {player.PlayerNumber}, Ikä: {player.age}");
                i++;
            }
            Console.WriteLine("");
        }

        public void PrintPlayersWithPenalties(List<Pelaajat> pelaajaRangaistukset)
        {
            Console.WriteLine("\n---RANGAISTUKSEN SAANUT---\n");
            foreach (Pelaajat player in pelaajaRangaistukset)
            {
                Console.WriteLine($"Nimi: {player.KokoNimi}, Pelinumero {player.PlayerNumber}");
            }
        }
        public List<Rangaistukset> GetPenaltiesWithPlayerID(int playerId)
        {
            Pelaajat peluri = GetPlayerWithID(playerId);

            List<Rangaistukset> penalty = new List<Rangaistukset>();
            foreach  (int peluriID in peluri.playerIDs)
            {
                Rangaistukset penalti = GetPenaltyWithID(peluriID);
                penalty.Add(penalti);
            }
            return penalty;
        }

        public void PrintPenaltiesWithPlayerID(List<Rangaistukset> penalty)
        {
            foreach (Rangaistukset penalti in penalty)
            {
                Console.WriteLine($"Rangaistus: {penalti.rangaistuksenNimi}, Kortti: {penalti.kortti}");
            }
        }
        

        public void AddTestData()
        {
            //pelaajat
            Pelaajat peluri1 = new Pelaajat("Kalle", "Kukkonen", 46, 15, Pelaajat.position.None);
            Pelaajat peluri2 = new Pelaajat("Antti", "Kananen", 89, 16, Pelaajat.position.None);
            Pelaajat peluri3 = new Pelaajat("Harri", "Mainio", 32, 17, Pelaajat.position.None);
            Pelaajat peluri4 = new Pelaajat("Tauno", "Kauno", 99, 15, Pelaajat.position.None);
            Pelaajat peluri5 = new Pelaajat("Tintti", "Mallinen", 88, 16, Pelaajat.position.None);
            Pelaajat peluri6 = new Pelaajat("Mauno", "Hirvi", 12, 16, Pelaajat.position.None);
            Pelaajat peluri7 = new Pelaajat("Miika", "Alaluoma", 14, 17, Pelaajat.position.None);
            Pelaajat peluri8 = new Pelaajat("Ukko", "Örhönen", 22, 18, Pelaajat.position.None);
            Pelaajat peluri9 = new Pelaajat("Jouni", "Luoma", 71, 15, Pelaajat.position.None);
            Pelaajat peluri10 = new Pelaajat("Reijo", "Alaluoma", 47, 14, Pelaajat.position.None);
            Pelaajat peluri11 = new Pelaajat("Aapeli", "Valo", 03, 16, Pelaajat.position.None);
            Pelaajat peluri12 = new Pelaajat("Ernesti", "Lehtola", 102, 14, Pelaajat.position.None);
            Pelaajat peluri13 = new Pelaajat("Jarkko", "Autokallio", 160, 16, Pelaajat.position.None);
            Pelaajat peluri14 = new Pelaajat("Joel", "Hännimäki", 55, 15, Pelaajat.position.None);
            Pelaajat peluri15 = new Pelaajat("Erkki", "Forma", 17, 16, Pelaajat.position.None);
            Pelaajat peluri16 = new Pelaajat("Kosti", "Porevirta", 122, 17, Pelaajat.position.None);
            Pelaajat peluri17 = new Pelaajat("Antti-Tapani", "Parkkola", 90, 16, Pelaajat.position.None);
            Pelaajat peluri18 = new Pelaajat("Juhana", "Dahl", 63, 17, Pelaajat.position.None);
            Pelaajat peluri19 = new Pelaajat("Uoti", "Pokela", 05, 15, Pelaajat.position.None);
            Pelaajat peluri20 = new Pelaajat("Lari", "Hyrkäs", 100, 14, Pelaajat.position.None);
            Pelaajat peluri21 = new Pelaajat("Tomi", "Jantunen", 08, 16, Pelaajat.position.None);
            Pelaajat peluri22 = new Pelaajat("Jonne-Eemeli", "Huusko", 199, 13, Pelaajat.position.None);

            //Joukkueet, 11 pelaajaa molemmissa
            Tiimit joukkue1 = new Tiimit("Pöllöt");
            joukkue1.AddPlayerToATeam(peluri1);
            joukkue1.AddPlayerToATeam(peluri2);
            joukkue1.AddPlayerToATeam(peluri3);
            joukkue1.AddPlayerToATeam(peluri4);
            joukkue1.AddPlayerToATeam(peluri5);
            joukkue1.AddPlayerToATeam(peluri6);
            joukkue1.AddPlayerToATeam(peluri7);
            joukkue1.AddPlayerToATeam(peluri8);
            joukkue1.AddPlayerToATeam(peluri9);
            joukkue1.AddPlayerToATeam(peluri10);
            joukkue1.AddPlayerToATeam(peluri11);

            Tiimit joukkue2 = new Tiimit("Pulut");
            joukkue2.AddPlayerToATeam(peluri12);
            joukkue2.AddPlayerToATeam(peluri13);
            joukkue2.AddPlayerToATeam(peluri14);
            joukkue2.AddPlayerToATeam(peluri15);
            joukkue2.AddPlayerToATeam(peluri16);
            joukkue2.AddPlayerToATeam(peluri17);
            joukkue2.AddPlayerToATeam(peluri18);
            joukkue2.AddPlayerToATeam(peluri19);
            joukkue2.AddPlayerToATeam(peluri20);
            joukkue2.AddPlayerToATeam(peluri21);
            joukkue2.AddPlayerToATeam(peluri22);

            //valmentajat
            Valmentajat valmentaja1 = new Valmentajat("Erkki", "Kuusisto", "0558829328");
            Valmentajat valmentaja2 = new Valmentajat("Ahti", "Mainio", "0401231312");
            joukkue2.AddCoachToATeam(valmentaja2);

            //rangaistukset
            Rangaistukset rangaistus1 = new Rangaistukset("Kampitus", "Keltainen");
            Rangaistukset rangaistus2 = new Rangaistukset("Kiinnipitäminen", "Keltainen");
            Rangaistukset rangaistus3 = new Rangaistukset("Tappelu", "Punainen");

            //tuloksien kirjausta
            Ottelutulokset matsi1 = new Ottelutulokset(){ KotiJoukkueId=1, VierasJoukkueId=2 };

            Maali goal1 = new Maali(10, 1, new MatchTime(65, 47));
            Maali goal2 = new Maali(15, 2, new MatchTime(89, 34));

            matsi1.InsertGameEvent(goal1);
            matsi1.InsertGameEvent(goal2);


            this.pelaajat.AddRange(new List<Pelaajat>() { peluri1, peluri2, peluri3, peluri4, peluri5, peluri6, peluri7, peluri8, peluri9, peluri10,
                                                        peluri11, peluri12, peluri13, peluri14, peluri15, peluri16, peluri17, peluri18, peluri19,
                                                        peluri20, peluri21, peluri22 });
            this.joukkueet.AddRange(new List<Tiimit>() { joukkue1, joukkue2 });
            this.valmentajat.AddRange(new List<Valmentajat>() { valmentaja1, valmentaja2 });
            this.rangaistukset.AddRange(new List<Rangaistukset>() { rangaistus1, rangaistus2, rangaistus3 });
            this.ottelut.AddRange(new List<Ottelutulokset>() { matsi1 });
            this.maalit.AddRange(new List<Maali>() {goal1, goal2 });
            this.gameEvents.AddRange(new List<GameEvent>() { goal1, goal2 });
        }
    }
}
