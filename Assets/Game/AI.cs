using Assets.Game.GameEntities;
using System;

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
        public Tank isVulnerableTOBullet() {

            //checks if the current position is vulanerable to a bullet attack

            //checks if any other tank is in line with our tank with the appropiate direction. 
            //current position of my tank
            int distanceLimit = 4;
            int x = ownedTank.PositionX;
            int y = ownedTank.PositionY;
            Tank tank = null;

            for (int i = 0; i < gameEngine.Tanks.Count;i++ ) 
            {
                if (i == ownedTank.PlayerNumber) { continue; }
                int opX = gameEngine.Tanks[i].PositionX;
                int opY = gameEngine.Tanks[i].PositionY;
                if(x == opX){
                    if(Math.Abs(y-opY)<distanceLimit ) {
                        if(y<opY && gameEngine.Tanks[i].Direction==Direction.NORTH){
                            distanceLimit = Math.Abs(y - opY);
                            tank =  gameEngine.Tanks[i];                            
                        }
                        else if (y > opY && gameEngine.Tanks[i].Direction == Direction.SOUTH)
                        {
                            distanceLimit = Math.Abs(y - opY);
                            tank = gameEngine.Tanks[i];
                        }
                    }
                }
                if (y == opY)
                {
                    if (Math.Abs(x - opX) < distanceLimit)
                    {
                        if (x < opX && gameEngine.Tanks[i].Direction == Direction.WEST)
                        {
                            return gameEngine.Tanks[i];
                        }
                        if (x > opX && gameEngine.Tanks[i].Direction == Direction.EAST)
                        {
                            return gameEngine.Tanks[i];
                        }
                    }
                }
              
                //possible improvements : this methods returns the first matching tank , but in reallity al the tanks should be checked to find the most important tank
                
            }



            return null;
        }
        public bool isNextPlaceVulnerable(int x , int y) {
            //checks the next place is vulnerable to bullet attack or water or does it hit a stone wall or a brick wall
            return false;
        }

        public LifePack getNearestLifePack() {
            //return the neaarest life pack 
            return null;
        }
        public CoinPile getNearestCoinPile() {
            //return the nearest coin pile
            return null;
        }
        public string getShootableDirection() { 
            //return the easiest direction to shoot the nearest player or brick wall
            return null;
            
        }


        public void CalculateMove()
        {
            
            



        }
    }
}
