namespace Assets.Game.GameEntities
{
    abstract class MovingObject : GameObject
    {
        #region Variables
        protected int speed;

        // Direction the tank is turned to
        protected Direction direction;    // Enumeration in GameObject Class
        public Direction Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }
        #endregion

        public MovingObject(int positionX, int positionY, Direction direction) : base(positionX, positionY)
        {
            this.direction = direction;

            speed = 0;
        }
    }
}
