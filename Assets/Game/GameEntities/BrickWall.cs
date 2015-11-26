namespace Assets.Game.GameEntities
{
    class BrickWall : GameObject
    {
        /*
         * Damage done to the brick wall
         * Has 0 at start
         * 4 - 100% damage
        */
        private int damage;
        public int Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value;
            }
        }

        public BrickWall(int positionX, int positionY) : base(positionX, positionY)
        {
            damage = 0;
        }
    }
}
