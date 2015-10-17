using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Game.GameEntities;

namespace Assets.Game
{
    class GameEngine
    {
        private List<BrickWall> brickWalls;
        private List<StoneWall> stoneWalls;
        private List<Water> water;
        private Tank[] tanks;
        private List<Bullet> bullets;

        private static GameEngine instance;
        public static GameEngine Instance
        {
            get
            {
                if (instance == null) instance = new GameEngine();
                return instance;
            }
        }
        private GameEngine()
        {
            brickWalls = new List<BrickWall>();
            stoneWalls = new List<StoneWall>();
            water = new List<Water>();
            tanks = new Tank[5];
            bullets = new List<Bullet>();
        }
    }
}
