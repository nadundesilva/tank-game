namespace Assets.Game.GameEntities
{
    class Bullet : MovingObject
    {
        /*
         * Bullet reduces the health of a tank by 10 points
        */
        public Bullet(int positionX, int positionY, Direction direction) : base(positionX, positionY, direction)
        {
            speed = Constants.Instance.BulletSpeed;
        }

        /*
         * Returns false if bullet hits something
         * Returns true if bullet hits nothing
         * Does not remove the bullet from the map
         * Removing the bullet needs to be done by the game engine
        */
        public bool Move()
        {
            GameEngine ge = GameManager.Instance.GameEngine;

            GameObject go = ge.Map[positionX, positionY];
            if (go is BrickWall || go is StoneWall)
            {
                return false;
            }
            else if (go is Tank)
            {
                Tank t = (Tank)go;
                if (t.Health <= 0)
                    ge.AddCoinPile(new CoinPile(PositionX, PositionY, t.Coins / 4));
                return false;
            }

            // Moving the bullet forward three squares depending on the direction it is turned to
            if (direction == Direction.NORTH)
            {
                for (int i = 0; i < speed; i++)
                {
                    if (positionY > 0)
                    {
                        positionY--;
                        go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall)
                        {
                            return false;
                        }
                        else if (go is Tank)
                        {
                            Tank t = (Tank) go;
                            if (t.Health <= 0)
                                ge.AddCoinPile(new CoinPile(PositionX, PositionY, t.Coins / 4));
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
                for (int i = 0; i < speed; i++)
                {
                    if (positionX < Constants.Instance.MapSize)
                    {
                        positionX++;
                        go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall)
                        {
                            return false;
                        }
                        else if (go is Tank)
                        {
                            Tank t = (Tank)go;
                            if (t.Health <= 0)
                                ge.AddCoinPile(new CoinPile(PositionX, PositionY, t.Coins / 4));
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
                for (int i = 0; i < speed; i++)
                {
                    if (positionY < Constants.Instance.MapSize)
                    {
                        positionY++;
                        go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall)
                        {
                            return false;
                        }
                        else if (go is Tank)
                        {
                            Tank t = (Tank)go;
                            if (t.Health <= 0)
                                ge.AddCoinPile(new CoinPile(PositionX, PositionY, t.Coins / 4));
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
                for (int i = 0; i < speed; i++)
                {
                    if (positionX > 0)
                    {
                        positionX--;
                        go = ge.Map[positionX,positionY];
                        if (go is BrickWall || go is StoneWall)
                        {
                            return false;
                        }
                        else if (go is Tank)
                        {
                            Tank t = (Tank)go;
                            if (t.Health <= 0)
                                ge.AddCoinPile(new CoinPile(PositionX, PositionY, t.Coins / 4));
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
