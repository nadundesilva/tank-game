using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    abstract class Collectible : GameObject
    {
        protected int timeLeft;
        public int TimeLeft
        {
            get
            {
                return timeLeft;
            }
        }

        public Collectible(int positionX, int positionY, int timeLeft) : base(positionX, positionY)
        {
            this.timeLeft = timeLeft;
        }

        public abstract void ReduceTime();
    }
}
