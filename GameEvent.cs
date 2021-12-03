using System;
using System.Collections.Generic;
using System.Text;

namespace Puulaakiliiga
{
    public class GameEvent
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private static int idCounter = 0;

        public GameEvent()
        {
            this.id = GetGameEventId();
        }

        private int GetGameEventId()
        {
            idCounter += 1;
            return idCounter;
        }
        

    }
}
