namespace Assets.Game.GameEntities
{
    class LifePack : Collectible
    {
        /*
         * Only used as a shell to indicate a life pack
         * Seperated from other collectibles because the coin piles possess a value
         * collecting a coin pile adds 10 health points
        */
        public LifePack(int positionX, int positionY, int timeLeft) : base(positionX, positionY, timeLeft)
        {

        }
    }
}
