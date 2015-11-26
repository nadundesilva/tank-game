namespace Assets.Game.GameEntities
{
    class CoinPile : Collectible
    {
        /*
         * Value of the coin pile
         * Any tank that collects this will receive points and coins equal to the value
        */
        protected int value;
        public int Value
        {
            get
            {
                return value;
            }
        }

        public CoinPile(int positionX, int positionY, int timeLeft, int value) : base(positionX, positionY, timeLeft)
        {
            this.value = value;
        }
    }
}
