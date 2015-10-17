using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.GameEntities
{
    abstract class GameObject
    {
        #region Variables
        protected int positionX;
        public int PositionX
        {
            get
            {
                return positionX;
            }

            set
            {
                positionX = value;
            }
        }

        protected int positionY;
        public int PositionY
        {
            get
            {
                return positionY;
            }

            set
            {
                positionY = value;
            }
        }
        #endregion

        public GameObject(int positionX, int positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }
    }

    enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }
}
