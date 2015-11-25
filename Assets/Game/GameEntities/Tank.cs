namespace Assets.Game.GameEntities
{
    class Tank : MovingObject
    {
        #region Variables
        // Player number assigned by the server
        private int playerNumber;
        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }
        }

        // Health possessed by a tank
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

        // Coins possessed by a tank
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

        // Points possessed by a tank
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

            Constants constants = new Constants();
            speed = constants.TankSpeed;
        }

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
