namespace Assets.Game.GameEntities
{
    class StoneWall : GameObject
    {
        /*
         * Only used as a shell to indicate a stone wall
         * Seperated from other obstacles because the bricks possess health
        */
        public StoneWall(int positionX, int positionY) : base(positionX, positionY)
        {

        }
    }
}
