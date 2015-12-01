using Assets.Game.GameEntities;

namespace Assets.Game
{
    internal class AI
    {
        /*
         * The depth of the tree created for the min max algorithm
        */
        private int treeDepth;

        #region Variables for storing references to important objects
        #endregion

        public AI()
        {
            treeDepth = Constants.Instance.AITreeDepth;
        }

        /*
         * Calculates the best move based on the state of the game
        */
        public void CalculateMove()
        {
            GameEngine gameEngine = GameManager.Instance.GameEngine;
            GameObject[,] map = gameEngine.Map;
            Tank ownedTank = gameEngine.Tanks[gameEngine.PlayerNumber];

        }
    }
}
