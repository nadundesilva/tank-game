using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class BrickWall : GameObject
    {
        //Health possessed by an object inherited from this class
        private int health;
        public int Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        public BrickWall(int positionX, int positionY) : base(positionX, positionY)
        {

        }
    }
}
