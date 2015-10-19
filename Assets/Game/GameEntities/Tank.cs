using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class Tank : MovingObject
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

        //Health possessed by a tank
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

        //Coins possessed by a tank
        private int coins;
        public int Coins
        {
            get
            {
                return coins;
            }
            set
            {
                coins = value;
            }
        }

        //Points possessed by a tank
        private int points;
        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }
        #endregion

        public Tank (int playerNumber, int positionX, int positionY, Direction direction) : base(positionX, positionY, direction)
        {
            this.playerNumber = playerNumber;
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet(positionX, positionY, direction);
            GameManager.Instance.GameEngine.AddBullet(bullet);
        }
    }
}
