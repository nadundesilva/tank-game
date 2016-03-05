using System.Collections.Generic;

using Assets.Game.GameEntities;

namespace Assets.Game
{
    class GameEngine
    {
        #region Variables
        /*
         * Lists of game objects
         * These lists are used for improving the animation performance
         * Stores references to the same game objects in the Map[,]
        */
        #region Game object lists
        private List<BrickWall> brickWalls;
        public List<BrickWall> BrickWalls
        {
            get
            {
                return brickWalls;
            }
            set
            {
                brickWalls = value;
                foreach (BrickWall b in brickWalls)
                {
                    map[b.PositionX,b.PositionX] = b;
                }
            }
        }

        private List<StoneWall> stoneWalls;
        public List<StoneWall> StoneWalls
        {
            get
            {
                return stoneWalls;
            }
            set
            {
                stoneWalls = value;
                foreach (StoneWall s in stoneWalls)
                {
                    map[s.PositionX,s.PositionY] = s;
                }
            }
        }

        private List<Water> water;
        public List<Water> Water
        {
            get
            {
                return water;
            }
            set
            {
                water = value;
                foreach (Water w in water)
                {
                    map[w.PositionX,w.PositionY] = w;
                }
            }
        }

        private List<Tank> tanks;
        public List<Tank> Tanks
        {
            get
            {
                return tanks;
            }
        }

        private List<Bullet> bullets;
        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
        }

        private List<CoinPile> coinPiles;
        public List<CoinPile> CoinPiles
        {
            get
            {
                return coinPiles;
            }
        }

        private List<LifePack> lifePacks;
        public List<LifePack> LifePacks
        {
            get
            {
                return lifePacks;
            }
        }
        #endregion

        /*
         * A 2D array to store game objects
         * Maps the x and y coordinates on the game map to the [x,y] on the Map variable
         * Used to improve AI performance in the search algorithm
         * [0,0] refers to the top left corner square of the map
        */
        private GameObject[,] map;
        public GameObject[,] Map
        {
            get
            {
                return map;
            }
        }

        /*
         * The player number of the tank owned by this client
        */
        private int playerNumber;
        public int PlayerNumber
        {
            get
            {
                return playerNumber;
            }
            set
            {
                playerNumber = value;
            }
        }

        /*
         * Life time left per one game play
        */
        private int[] gameTime;
        public int[] GameTime
        {
            get
            {
                return gameTime;
            }
        }
        #endregion

        public GameEngine()
        {
            #region Initializing variables
            Constants constants = Constants.Instance;   // Loaded to be used for initializing

            brickWalls = new List<BrickWall>();
            stoneWalls = new List<StoneWall>();
            water = new List<Water>();

            tanks = new List<Tank>();

            bullets = new List<Bullet>();

            coinPiles = new List<CoinPile>();
            lifePacks = new List<LifePack>();

            int mapSize = constants.MapSize;
            map = new GameObject[mapSize, mapSize];

            gameTime = new int[] { constants.GameTimeMinutes, constants.GameTimeSeconds };

            playerNumber = -1;  // Set to -1 to indicate that it is not yet assigned by the server
            #endregion
        }

        /*
         * Used to move bullets, reduce the time left in collectibles and reduce the time left in the game
         * Called before each game update message sent by server
        */
        public void Clock()
        {
            // Reducing time of collectibles
            foreach (CoinPile coinPile in coinPiles)
                coinPile.ReduceTime();
            foreach (LifePack lifePack in lifePacks)
                lifePack.ReduceTime();

            // Reduce the time left in the game
            if (gameTime[0] != 0 || gameTime[1] != 0)
            {
                gameTime[1]--;
                if (gameTime[1] == -1)
                {
                    gameTime[0] -= 1;
                    gameTime[1] = 59;
                }
            }
        }

        /*
         * Used remove collectibles if they are collected or if the time had run out
         * Called after eachgame update message sent by server
        */
        public void UpdateGame()
        {
            // Removing coin piles if the time runs out or a tank moves to the location of the coin pile
            int i = 0;
            while (i < coinPiles.Count)
            {
                CoinPile coinPile = coinPiles[i];
                if (map[coinPile.PositionX, coinPile.PositionY] == null)
                {
                    coinPiles.RemoveAt(i);
                }
                else if (!(map[coinPile.PositionX, coinPile.PositionY] is CoinPile))
                {
                    coinPiles.RemoveAt(i);
                }
                else if (coinPile.TimeLeft <= 0)
                {
                    coinPiles.RemoveAt(i);
                    map[coinPile.PositionX, coinPile.PositionY] = null;
                }
                i++;
            }

            // Removing life packs if the time runs out or a tank moves to the location of the life pack
            i = 0;
            while (i < lifePacks.Count)
            {
                LifePack lifePack = lifePacks[i];
                if (map[lifePack.PositionX, lifePack.PositionY] == null)
                {
                    lifePacks.RemoveAt(i);
                }
                else if (!(map[lifePack.PositionX, lifePack.PositionY] is LifePack))
                {
                    lifePacks.RemoveAt(i);
                }
                else if (lifePack.TimeLeft <= 0)
                {
                    lifePacks.RemoveAt(i);
                    map[lifePack.PositionX, lifePack.PositionY] = null;
                }
                i++;
            }

            // Moving the bullets
            foreach (Bullet bullet in bullets)
                if (!bullet.Move())
                    bullets.Remove(bullet);
        }

        /*
         * Adders for tanks, bullets, coin piles and life packs
         * Adds to the relevant object list
         * Adds to the map except for bullets
        */
        #region Adders to lists
        public void AddTank(Tank tank)
        {
            map[tank.PositionX, tank.PositionY] = tank;
            tanks.Add(tank);
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Insert(0, bullet);
        }

        public void AddCoinPile(CoinPile coinPile)
        {
            map[coinPile.PositionX, coinPile.PositionY] = coinPile;
            coinPiles.Add(coinPile);
        }

        public void AddLifePack(LifePack lifePack)
        {
            map[lifePack.PositionX, lifePack.PositionY] = lifePack;
            lifePacks.Add(lifePack);
        }
        #endregion
    }
}
