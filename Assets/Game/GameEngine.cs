using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                foreach (BrickWall b in brickWalls)
                {
                    map[b.PositionX,b.PositionX] = null;
                }
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

        private Timer timer;
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

            //Timer for updating the time left in collectibles
            timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += updateCollectibles;
        }

        public void Clock()
        {
            //Moving the bullets
            foreach (Bullet b in bullets)
            {
                if (!b.Move())
                    GameManager.Instance.GameEngine.RemoveBullet(b);

            }
        }

        public void UpdateGame()
        {
            UpdateTankPosition();
        }

        public void UpdateTankPosition()
        {
            int i = 0;
            while (i < tanks.Count)
            {
                GameObject go = map[tanks[i].PositionX, tanks[i].PositionY];
                if (go != null && go is CoinPile)
                {
                    GameManager.Instance.GameEngine.RemoveCoinPile(tanks[i].PositionX, tanks[i].PositionY);
                }
                else if (go != null && go is LifePack)
                {
                    GameManager.Instance.GameEngine.RemoveLifePack(tanks[i].PositionX, tanks[i].PositionY);
                }
                i++;
            }
        }

        public void updateCollectibles(object source, EventArgs a)
        {
            int i = 0;
            while (i < coinPiles.Count)
            {
                coinPiles[i].ReduceTime();
                if (coinPiles[i].TimeLeft <= 0)
                {
                    RemoveCoinPile(coinPiles[i].PositionX, coinPiles[i].PositionY);
                }
                else
                {
                    i++;
                }
            }
            i = 0;
            while (i < lifePacks.Count)
            {
                lifePacks[i].ReduceTime();
                if (lifePacks[i].TimeLeft <= 0)
                {
                    RemoveLifePack(lifePacks[i].PositionX, lifePacks[i].PositionY);
                }
                else
                {
                    i++;
                }
            }
        }

        #region Adders and Removers for lists
        public void AddTank(Tank tank)
        {
            map[tank.PositionX, tank.PositionY] = tank;
            tanks.Add(tank);
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
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

        public void RemoveBullet(Bullet bullet)
        {
            bullets.Remove(bullet);
        }

        public void RemoveCoinPile(int x, int y)
        {
            coinPiles.Remove((CoinPile)map[x,y]);
            map[x, y] = null;
        }

        public void RemoveLifePack(int x, int y)
        {
            lifePacks.Remove((LifePack)map[x,y]);
            map[x, y] = null;
        }
        #endregion
    }
}
