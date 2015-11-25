namespace Assets.Game
{
    class AI
    {
        private int treeDepth;

        // Calculates the best move based on the state of the game
        public void CalculateMove()
        {
            treeDepth = Constants.AITreeDepth;
        }
    }
}
