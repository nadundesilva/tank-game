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

    /*
     * Used to indicate a direction on the map
     * The top left corner of the map is [0,0]
     * NORTH is directed towards the top of the board
    */
    enum Direction
    {
        NORTH, EAST, SOUTH, WEST
    }
}
