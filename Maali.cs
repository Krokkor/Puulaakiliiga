using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class Maali : GameEvent
    {

        private int pelaajaId;

        public int PelaajaId
        {
            get { return pelaajaId; }
            set { pelaajaId = value; }
        }

        private MatchTime time;

        public MatchTime Time
        {
            get { return time; }
            set { time = value; }
        }

        private int teamId;

        public int TeamId
        {
            get { return teamId; }
            set { teamId = value; }
        }

        public Maali()
        {

        }

        public Maali(int pelaajaId, int teamId)
        {
            this.PelaajaId = pelaajaId;
            this.teamId = teamId;
        }

        public Maali(int pelaajaId, int teamId, MatchTime timeScored)
        {
            this.Time = timeScored;
            this.PelaajaId = pelaajaId;
            this.TeamId = teamId;
        }


    }
}
