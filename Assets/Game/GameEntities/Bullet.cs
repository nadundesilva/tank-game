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

        public void Move()
        {
            GameEngine ge = GameManager.Instance.GameEngine;

            //Moving the bullet forward three squares depending on the direction it is turned to
            if (direction == Direction.NORTH)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionX < 19)
                    {
                        positionX += 1;
                        GameObject go = ge.Map[positionX][positionY];
                        if (go != null &&
                            !(go is CoinPile || go is LifePack || go is Water || go is Bullet))
                        {
                            ge.RemoveBullet(this);
                        }
                    }
                    else
                    {
                        ge.RemoveBullet(this);
                    }
                }
            }
            else if (direction == Direction.EAST)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionY < 19)
                    {
                        positionY += 1;
                        GameObject go = ge.Map[positionX][positionY];
                        if (go != null &&
                            !(go is CoinPile || go is LifePack || go is Water || go is Bullet))
                        {
                            ge.RemoveBullet(this);
                        }
                    }
                    else
                    {
                        ge.RemoveBullet(this);
                    }
                }
            }
            else if (direction == Direction.SOUTH)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionX > 0)
                    {
                        positionX -= 1;
                        GameObject go = ge.Map[positionX][positionY];
                        if (go != null &&
                            !(go is CoinPile || go is LifePack || go is Water || go is Bullet))
                        {
                            ge.RemoveBullet(this);
                        }
                    }
                    else
                    {
                        ge.RemoveBullet(this);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (positionY > 0)
                    {
                        positionX -= 1;
                        GameObject go = ge.Map[positionX][positionY];
                        if (go != null &&
                            !(go is CoinPile || go is LifePack || go is Water || go is Bullet))
                        {
                            ge.RemoveBullet(this);
                        }
                    }
                    else
                    {
                        ge.RemoveBullet(this);
                    }
                }
            }
        }
    }
}
