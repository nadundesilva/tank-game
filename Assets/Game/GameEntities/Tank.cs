namespace Assets.Game.GameEntities
{
    class Tank : MovingObject
    {
        #region Variables
        /*
         * Player number assigned by the server
        */
        private int playerNumber;
        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }
        }

        /*
         * Health possessed by a tank
         * Starts with 100 health
         * Can have health higher than 100
         * Having health zero or below will cause the tank to die
        */
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

        /*
         * Coins possessed by a tank
         * Starts with no coins
         * Tank can collect coins by moving to a cell with a coin pile
        */
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

        /*
         * Points possessed by a tank
         * Starts with no points
         * Tank can collect points by collecting coins, shooting bricks and tanks
        */
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

            speed = Constants.Instance.TankSpeed;
        }

        /*
         * Used to shoot a bullet in the direction the tank is turned to
        */
        public void Shoot()
        {
            int x = 0;
            int y = 0;
            if (direction == Direction.NORTH)
                y = -1;
            else if (direction == Direction.EAST)
                x = 1;
            else if (direction == Direction.SOUTH)
                y = 1;
            else
                x = -1;

            Bullet bullet = new Bullet(positionX + x, positionY + y, direction);
            GameManager.Instance.GameEngine.AddBullet(bullet);
        }
    }
}
