namespace Assets.Game
{
    class AI
    {
        /*
         * The depth of the tree created for the min max algorithm
        */
        private int treeDepth;

        /*
         * Calculates the best move based on the state of the game
        */
        public void CalculateMove()
        {
            treeDepth = Constants.Instance.AITreeDepth;
        }
    }
}
