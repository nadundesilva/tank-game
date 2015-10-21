using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class CoinPile : Collectible
    {
        protected int value;
        public int Value
        {
            get
            {
                return value;
            }
        }

        public CoinPile(int positionX, int positionY, int timeLeft, int value) : base(positionX, positionY, timeLeft)
        {
            this.value = value;
        }

        public override void ReduceTime()
        {
            timeLeft -= 1;
        }
    }
}
