using Assets.Game.GameEntities;
using System;

namespace Assets.Game
{   
    internal class AI
    {
        /*
         * The depth of the tree created for the min max algorithm
        */
        private const int gridSize = 10;    
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
                            distanceLimit = Math.Abs(x - opX);
                            tank = gameEngine.Tanks[i];
                        }
                        if (x > opX && gameEngine.Tanks[i].Direction == Direction.EAST)
                        {
                            distanceLimit = Math.Abs(x - opX);
                            tank = gameEngine.Tanks[i];
                        }
                    }
                }
                return tank;
              
                //possible improvements : this methods returns the first matching tank , but in reallity al the tanks should be checked to find the most important tank
                
            }

            return null;
        }

        private bool hasWater(int x , int y){
            for (int i = 0; i < gameEngine.Water.Count; i++ ) {
                if (x == gameEngine.Water[i].PositionX && y == gameEngine.Water[i].PositionY) {
                //next move is killing by water :(
                return true;
                
                }
            }
            return false;
        }
        private bool hasBrick(int x , int y){
            for (int i = 0; i < gameEngine.BrickWalls.Count; i++ ) {
                if (x == gameEngine.BrickWalls[i].PositionX && y == gameEngine.BrickWalls[i].PositionY) {
                //next move is killing by water :(
                return true;
                
                }
            }
            return false;
        }
        private bool hasStone(int x , int y){
            for (int i = 0; i < gameEngine.StoneWalls.Count; i++ ) {
                if (x == gameEngine.StoneWalls[i].PositionX && y == gameEngine.StoneWalls[i].PositionY) {
                //next move is killing by water :(
                return true;
                
                }
            }
            return false;
        }

        public bool isNextMoveVulnerable(string move) {
            //checks the next place is vulnerable to bullet attack or water or does it hit a stone wall or a brick wall
            //1. if the 
            if(move.Equals("UP")){
                if (ownedTank.Direction == Direction.NORTH)
                {

                    //1st check if the nect cell contains water
                    //2nd check whether the nect cell contains a stone wall
                    //if the next cell contains a brick wall
                    //if the next cell is vulnarable to a bullet attack
                    int x = ownedTank.PositionX;
                    int y = ownedTank.PositionY -1;

                    if (y < 0)
                    {
                        return true; // this move is invalid
                    }
                    else {
                        
                        for (int i = 0; i < gameEngine.BrickWalls.Count; i++)
                        {
                            if (x == gameEngine.BrickWalls[i].PositionX && y == gameEngine.BrickWalls[i].PositionY)
                            {
                                //next move is going to be hit by a brick :(
                                return true;
                            }
                        }
                        for (int i = 0; i < gameEngine.StoneWalls.Count; i++)
                        {
                            if (x == gameEngine.StoneWalls[i].PositionX && y == gameEngine.StoneWalls[i].PositionY)
                            {
                                //next move is going to be hit by a stone wall :(
                                return true;
                            }
                        }

                        //check for a possible bullet attack
                        int distanceLimit = 4;
                        for (int i = 0; i < gameEngine.Tanks.Count;i++ ) 
                         {
                                if (i == ownedTank.PlayerNumber) { continue; }
                                int opX = gameEngine.Tanks[i].PositionX;
                                int opY = gameEngine.Tanks[i].PositionY;
                                if(x == opX){
                                    if(Math.Abs(y-opY)<distanceLimit ) {
                                        if(y<opY && gameEngine.Tanks[i].Direction==Direction.NORTH){
                                            distanceLimit = Math.Abs(y - opY);
                                            return true;                           
                                        }
                                        else if (y > opY && gameEngine.Tanks[i].Direction == Direction.SOUTH)
                                        {
                                            distanceLimit = Math.Abs(y - opY);
                                            return true;
                                        }
                                    }
                                }   
                                if (y == opY)
                                {
                                    if (Math.Abs(x - opX) < distanceLimit)
                                    {
                                        if (x < opX && gameEngine.Tanks[i].Direction == Direction.WEST)
                                        {
                                            distanceLimit = Math.Abs(x - opX);
                                            return true;
                                        }
                                        if (x > opX && gameEngine.Tanks[i].Direction == Direction.EAST)
                                        {
                                            distanceLimit = Math.Abs(x - opX);
                                            return true;
                                        }
                                    }
                                }



                        }


                    
                    


                    //he can move
                }
                else { 
                    //he cant move
                    return (isVulnerableTOBullet()==null)? false : true ;
                }
            }else if (move.Equals("DOWN")){}
            else if(move.Equals("RIGHT")){}
            else if (move.Equals("LEFT")) { }
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
