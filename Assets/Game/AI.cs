using Assets.Game.GameEntities;

namespace Assets.Game
{
    internal class AI
    {
        /*
         * The depth of the tree created for the min max algorithm
        */
        private int treeDepth;
        private GameEngine gameEngine ;
        private GameObject[,] map ;
        private Tank ownedTank ;

        #region Variables for storing references to important objects
        #endregion

        public AI()
        {
            treeDepth = Constants.Instance.AITreeDepth;
            gameEngine = GameManager.Instance.GameEngine;
            map = gameEngine.Map;
            ownedTank = gameEngine.Tanks[gameEngine.PlayerNumber];
        }

        /*
         * Calculates the best move based on the state of the game
        */

            

        //all possible moves :
            /*
             *up
             *down
             *left 
             *right
             *shoot
             *one move goes to turn to another direction
             */
        /*
         Objectives
         * survive from bullet
         * avoid drawning in water
         * avoid hitting with a stone wall
         * colect a health pile
         * collect coins
         * shoot others
         * destroy brick wall
         * no movement
         * 
                 
         */
        function 

        public void CalculateMove()
        {
            
            



        }
    }
}
