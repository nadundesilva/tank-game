using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class Tank : KinematicObject
    {
        #region Variables
        //Player number assigned by the server
        private int playerNumber;
        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }
        }

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
        #endregion

        public Tank (int playerNumber, int positionX, int positionY, Direction direction) : base(positionX, positionY, direction)
        {
            this.playerNumber = playerNumber;
        }

        public void shoot()
        {
            Bullet bullet = new Bullet(positionX, positionY, direction);
        }
    }
}
