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
            set
            {
                foreach (Tank t in tanks)
                {
                    map[t.PositionX,t.PositionY] = null;
                }
                tanks = value;
                foreach (Tank t in tanks)
                {
                    map[t.PositionX,t.PositionY] = t;
                }
            }
        }

        private List<Bullet> bullets;
        public List<Bullet> Bullets
        {
            get
            {
                return bullets;
            }
            set
            {
                bullets = value;
                foreach (Bullet b in bullets)
                {
                    map[b.PositionX,b.PositionY] = b;
                }
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
            foreach (Bullet b in bullets)
            {
                b.Move();
            }

            //reducing time left of collectibles
            int i = 0;
            while (i < coinPiles.Count)
            {
                coinPiles[i].ReduceTime();
                if (coinPiles[i].TimeLeft == 0)
                {
                    coinPiles.Remove(coinPiles[i]);
                    map[coinPiles[i].PositionX, coinPiles[i].PositionY] = null;
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
                if (lifePacks[i].TimeLeft == 0)
                {
                    lifePacks.Remove(lifePacks[i]);
                    map[lifePacks[i].PositionX, lifePacks[i].PositionY] = null;
                } else
                {
                    i++;
                }
            }
        }

        public void UpdateTankPosition()
        {
            int i = 0;
            while (i < tanks.Count)
            {
                GameObject go = map[tanks[i].PositionX, tanks[i].PositionY];
                if (go != null && (go is CoinPile || go is LifePack))
                {
                    GameManager.Instance.GameEngine.RemoveCoinPile(tanks[i].PositionX, tanks[i].PositionY);
                }
                i++;
            }
        }

        #region Adders and Removers for lists
        public void AddTank(Tank tank)
        {
            tanks.Add(tank);
            map[tank.PositionX,tank.PositionY] = tank;
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
            map[bullet.PositionX,bullet.PositionY] = bullet;
        }

        public void AddCoinPile(CoinPile coinPile)
        {
            coinPiles.Add(coinPile);
            map[coinPile.PositionX,coinPile.PositionY] = coinPile;
        }

        public void AddLifePack(LifePack lifePack)
        {
            lifePacks.Add(lifePack);
            map[lifePack.PositionX,lifePack.PositionY] = lifePack;
        }

        public void RemoveBullet(Bullet bullet)
        {
            bullets.Remove(bullet);
        }

        public void RemoveCoinPile(int x, int y)
        {
            coinPiles.Remove((CoinPile)map[x,y]);
            map[x,y] = null;
        }

        public void RemoveLifePack(int x, int y)
        {
            lifePacks.Remove((LifePack)map[x,y]);
            map[x,y] = null;
        }
        #endregion
    }
}
