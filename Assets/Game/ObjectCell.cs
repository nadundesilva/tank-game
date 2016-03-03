using Assets.Game.GameEntities;
using System;
using System.Collections.Generic;

namespace Assets.Game
{
    class ObjectCell
    {
        public string category;//"S / B / W / N"
        public Direction direction;
        public List<string> moves;
        public ObjectCell predesessor;
        public int count;
        public String state;// S : started , N: not reached , E : exhausted
        public ObjectCell()
        {
            this.category = "N";
            moves = new List<string>();
            predesessor = null;
            count = Int32.MaxValue;
            state = "N";


        }

    }
}
