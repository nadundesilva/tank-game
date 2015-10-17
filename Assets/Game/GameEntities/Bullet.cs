using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class Bullet : KinematicObject
    {
        public Bullet(int positionX, int positionY, Direction direction) : base(positionX, positionY, direction)
        {

        }

        public void move()
        {
            //Moving the bullet forward three squares depending on the direction it is turned to
            if (direction == Direction.NORTH)
            {
                if (PositionX < 19) PositionX += 3;
            }
            else if (direction == Direction.EAST)
            {
                if (PositionY < 19) PositionY += 3;
            }
            else if (direction == Direction.SOUTH)
            {
                if (PositionX > 0) PositionX -= 3;
            }
            else
            {
                if (PositionY > 0) PositionY -= 3;
            }
        }
    }
}
