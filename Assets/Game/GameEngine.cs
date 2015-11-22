using System;
using System.Collections.Generic;

using Assets.Game.GameEntities;
using System.Timers;

namespace Assets.Game
{
    class GameEngine
    {
        #region Variables
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

        private GameObject[,] map;
        public GameObject[,] Map
        {
            get
            {
                return map;
            }
        }

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
        #endregion

        public GameEngine()
        {
            brickWalls = new List<BrickWall>();
            stoneWalls = new List<StoneWall>();
            water = new List<Water>();

            tanks = new List<Tank>();

            bullets = new List<Bullet>();

            coinPiles = new List<CoinPile>();
            lifePacks = new List<LifePack>();

            map = new GameObject[10, 10];

            playerNumber = 0;
        }

        public void Clock()
        {
            //Moving the bullets
            foreach (Bullet bullet in bullets)
                if (!bullet.Move())
                    bullets.Remove(bullet);

            //Reducing time of collectibles
            foreach (CoinPile coinPile in coinPiles)
                coinPile.ReduceTime();
            foreach (LifePack lifePack in lifePacks)
                lifePack.ReduceTime();
        }

        public void UpdateGame()
        {
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
        }

        #region Adders lists
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
