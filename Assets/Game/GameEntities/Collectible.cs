namespace Assets.Game.GameEntities
{
    abstract class Collectible : GameObject
    {
        /*
         * Time left till the collectible disappears
         * Unit - miliseconds
        */
        protected int timeLeft;
        public int TimeLeft
        {
            get
            {
                return timeLeft;
            }
        }

        public Collectible(int positionX, int positionY, int timeLeft) : base(positionX, positionY)
        {
            this.timeLeft = timeLeft;
        }

        public void ReduceTime()
        {
            // Reduces the time left till collectible disapears by one second 
            timeLeft -= 1000;
        }
    }
}
