using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class LifePack : Collectible
    {
        public LifePack(int positionX, int positionY, int timeLeft) : base(positionX, positionY, timeLeft)
        {

        }

        public override void ReduceTime()
        {
            timeLeft -= 1;
            GameManager.Instance.GameEngine.RemoveLifePack(positionX, positionY);
        }
    }
}
