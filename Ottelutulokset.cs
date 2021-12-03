using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Ottelutulokset
    {
        public List<Maali> maalit = new List<Maali>();

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private static int idCounter = 1;

        public Ottelutulokset()
        {
            this.Id = idCounter;
            idCounter++;
        }

        private int kotiJoukkueId;

        public int KotiJoukkueId
        {
            get { return kotiJoukkueId; }
            set { kotiJoukkueId = value; }
        }

        private int vierasJoukkueId;

        public int VierasJoukkueId
        {
            get { return vierasJoukkueId; }
            set { vierasJoukkueId = value; }
        }

        private List<int> gameEvents = new List<int>();

        public List<int> GameEvents
        {
            get { return gameEvents; }
            set { gameEvents = value; }
        }

        public void InsertGameEvent(GameEvent gameEvent)
        {
            this.gameEvents.Add(gameEvent.Id);
        }

    }

    public struct MatchTime
    {
        public MatchTime(int minutes = 00, int seconds = 00)
        {
            this.minutes = minutes;
            this.seconds = seconds;
        }
        public int minutes { get; set; }
        public int seconds { get; set; }
        
    }
}
