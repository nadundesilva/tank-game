namespace Assets.Game.GameEntities
{
    class Water : GameObject
    {
        /*
         * Only used as a shell to indicate water
         * Seperated from other obstacles because the bricks possess health
        */
        public Water(int positionX, int positionY) : base(positionX, positionY)
        {

        }
    }
}
