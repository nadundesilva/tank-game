using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    class Bullet : MovingObject
    {
        public Bullet(int positionX, int positionY, Direction direction) : base(positionX, positionY, direction)
        {

        }

        /*
         * Returns false if bullet hits something
         * Returns true if bullet hits nothing
        */
        public bool Move()
        {
            GameEngine ge = GameManager.Instance.GameEngine;

            //Moving the bullet forward three squares depending on the direction it is turned to
            if (direction == Direction.NORTH)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionY > 0)
                    {
                        positionY--;
                        GameObject go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall || go is Tank)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (direction == Direction.EAST)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionX < 9)
                    {
                        positionX++;
                        GameObject go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall || go is Tank)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (direction == Direction.SOUTH)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionY < 9)
                    {
                        positionY++;
                        GameObject go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall || go is Tank)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionX > 0)
                    {
                        positionX--;
                        GameObject go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall || go is Tank)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
