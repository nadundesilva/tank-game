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
    }
}
