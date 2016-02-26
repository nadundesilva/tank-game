using Assets.Game.GameEntities;
using System;
using System.Collections.Generic;

namespace Assets.Game
{
    internal class AI
    {
        /*
         * The depth of the tree created for the min max algorithm
        */
        private const int gridSize = 10;
        private int treeDepth;
        private GameEngine gameEngine;
        private GameObject[,] map;
        private Tank ownedTank;
        List<string> coinPileSteps = null;//= new List<string>();
        List<string> healthPileSteps = null;//= new List<string>();
        private CoinPile targetCoinPile = null;
        private LifePack targetLifePack = null;



        #region Variables for storing references to important objects
        #endregion

        public AI()
        {
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
        public Tank isVulnerableTOBullet(int x, int y)
        {

            //checks if the current position is vulanerable to a bullet attack

            //checks if any other tank is in line with our tank with the appropiate direction. 
            //current position of my tank
            int distanceLimit = 4;

            Tank tank = null;

            for (int i = 0; i < gameEngine.Tanks.Count; i++)
            {
                if (i == ownedTank.PlayerNumber) { continue; }
                int opX = gameEngine.Tanks[i].PositionX;
                int opY = gameEngine.Tanks[i].PositionY;
                if (x == opX)
                {
                    if (Math.Abs(y - opY) < distanceLimit)
                    {
                        if (y < opY && gameEngine.Tanks[i].Direction == Direction.NORTH)
                        {
                            distanceLimit = Math.Abs(y - opY);
                            tank = gameEngine.Tanks[i];
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

        private bool hasWater(int x, int y)
        {
            for (int i = 0; i < gameEngine.Water.Count; i++)
            {
                if (x == gameEngine.Water[i].PositionX && y == gameEngine.Water[i].PositionY)
                {
                    //next move is killing by water :(
                    return true;

                }
            }
            return false;
        }
        private bool hasBrick(int x, int y)
        {
            for (int i = 0; i < gameEngine.BrickWalls.Count; i++)
            {
                if (x == gameEngine.BrickWalls[i].PositionX && y == gameEngine.BrickWalls[i].PositionY)
                {
                    //next move is killing by water :(
                    return true;

                }
            }
            return false;
        }
        private bool hasStone(int x, int y)
        {
            for (int i = 0; i < gameEngine.StoneWalls.Count; i++)
            {
                if (x == gameEngine.StoneWalls[i].PositionX && y == gameEngine.StoneWalls[i].PositionY)
                {
                    //next move is killing by water :(
                    return true;

                }
            }
            return false;
        }
        public String isNextMoveVulnerable(string move)
        {
            //checks the next place is vulnerable to bullet attack or water or does it hit a stone wall or a brick wall
            //1. if the 
            if (move.Equals("UP"))
            {
                if (ownedTank.Direction == Direction.NORTH)
                {

                    //1st check if the nect cell contains water
                    //2nd check whether the nect cell contains a stone wall
                    //if the next cell contains a brick wall
                    //if the next cell is vulnarable to a bullet attack
                    int x = ownedTank.PositionX;
                    int y = ownedTank.PositionY - 1;

                    if (y < 0)
                    {
                        return "INVALID"; // this move is invalid
                    }
                    else
                    {

                        if (hasBrick(x, y))
                        {
                            return "BRICK";
                        }
                        if (hasStone(x, y))
                        {
                            return "STONE";
                        }
                        if (hasWater(x, y))
                        {
                            return "WATER";
                        }

                        //check for a possible bullet attack
                        Tank t = isVulnerableTOBullet(x, y);
                        if (t != null)
                        {
                            return "NEXT " + t.PlayerNumber + "";
                        }
                    }


                }
                else
                {
                    Tank t = isVulnerableTOBullet(ownedTank.PositionX, ownedTank.PositionY);
                    if (t != null)
                    {
                        return "CURRENT" + t.PlayerNumber + "";
                    }
                }
            }


            if (move.Equals("DOWN"))
            {
                if (ownedTank.Direction == Direction.SOUTH)
                {

                    //1st check if the nect cell contains water
                    //2nd check whether the nect cell contains a stone wall
                    //if the next cell contains a brick wall
                    //if the next cell is vulnarable to a bullet attack
                    int x = ownedTank.PositionX;
                    int y = ownedTank.PositionY + 1;

                    if (y >= gridSize)
                    {
                        return "INVALID"; // this move is invalid
                    }
                    else
                    {

                        if (hasBrick(x, y))
                        {
                            return "BRICK";
                        }
                        if (hasStone(x, y))
                        {
                            return "STONE";
                        }
                        if (hasWater(x, y))
                        {
                            return "WATER";
                        }

                        //check for a possible bullet attack
                        Tank t = isVulnerableTOBullet(x, y);
                        if (t != null)
                        {
                            return "NEXT " + t.PlayerNumber + "";
                        }
                    }


                }
                else
                {
                    Tank t = isVulnerableTOBullet(ownedTank.PositionX, ownedTank.PositionY);
                    if (t != null)
                    {
                        return "CURRENT" + t.PlayerNumber + "";
                    }
                }
            }

            if (move.Equals("RIGHT"))
            {
                if (ownedTank.Direction == Direction.EAST)
                {

                    //1st check if the nect cell contains water
                    //2nd check whether the nect cell contains a stone wall
                    //if the next cell contains a brick wall
                    //if the next cell is vulnarable to a bullet attack
                    int x = ownedTank.PositionX + 1;
                    int y = ownedTank.PositionY;

                    if (x >= gridSize)
                    {
                        return "INVALID"; // this move is invalid
                    }
                    else
                    {

                        if (hasBrick(x, y))
                        {
                            return "BRICK";
                        }
                        if (hasStone(x, y))
                        {
                            return "STONE";
                        }
                        if (hasWater(x, y))
                        {
                            return "WATER";
                        }

                        //check for a possible bullet attack
                        Tank t = isVulnerableTOBullet(x, y);
                        if (t != null)
                        {
                            return "NEXT " + t.PlayerNumber + "";
                        }
                    }


                }
                else
                {
                    Tank t = isVulnerableTOBullet(ownedTank.PositionX, ownedTank.PositionY);
                    if (t != null)
                    {
                        return "CURRENT" + t.PlayerNumber + "";
                    }
                }
            }



            if (move.Equals("LEFT"))
            {
                if (ownedTank.Direction == Direction.WEST)
                {

                    //1st check if the nect cell contains water
                    //2nd check whether the nect cell contains a stone wall
                    //if the next cell contains a brick wall
                    //if the next cell is vulnarable to a bullet attack
                    int x = ownedTank.PositionX - 1;
                    int y = ownedTank.PositionY;

                    if (x < 0)
                    {
                        return "INVALID"; // this move is invalid
                    }
                    else
                    {

                        if (hasBrick(x, y))
                        {
                            return "BRICK";
                        }
                        if (hasStone(x, y))
                        {
                            return "STONE";
                        }
                        if (hasWater(x, y))
                        {
                            return "WATER";
                        }

                        //check for a possible bullet attack
                        Tank t = isVulnerableTOBullet(x, y);
                        if (t != null)
                        {
                            return "NEXT " + t.PlayerNumber + "";
                        }
                    }


                }
                else
                {
                    Tank t = isVulnerableTOBullet(ownedTank.PositionX, ownedTank.PositionY);
                    if (t != null)
                    {
                        return "CURRENT" + t.PlayerNumber + "";
                    }
                }
            }
            return "NO DANGER";

        }




        private int max(int x, int y)
        {
            return x > y ? x : y;
        }
        public bool canShoot()
        {
            //shooting is done when the direction is aligned and the distance is set
            //tells to shoot or not , direction is not changed
            int x = ownedTank.PositionX;
            int y = ownedTank.PositionY;

            for (int i = 0; i < gameEngine.Tanks.Count; i++)
            {
                if (i == ownedTank.PlayerNumber) { continue; }
                else if (gameEngine.Tanks[i].PositionX == x)
                {
                    if (y > gameEngine.Tanks[i].PositionY)
                    {
                        if (ownedTank.Direction == Direction.NORTH && Math.Abs(y - gameEngine.Tanks[i].PositionY) < 2)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        if (ownedTank.Direction == Direction.SOUTH && Math.Abs(y - gameEngine.Tanks[i].PositionY) < 2)
                        {
                            return true;
                        }

                    }

                }
                if (gameEngine.Tanks[i].PositionY == y)
                {
                    if (x > gameEngine.Tanks[i].PositionX)
                    {
                        if (ownedTank.Direction == Direction.WEST && Math.Abs(x - gameEngine.Tanks[i].PositionX) < 2)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        if (ownedTank.Direction == Direction.EAST && Math.Abs(x - gameEngine.Tanks[i].PositionX) < 2)
                        {
                            return true;
                        }

                    }

                }
            }





            return false;

        }

        private List<string> calculatePath(int x, int y, List<string> usedCells, int limit, List<string> coinPileList, Direction direction)
        {



            List<string> answer1 = null;
            List<string> answer2 = null;
            List<string> answer3 = null;
            List<string> answer4 = null;






            if (!(limit < 0))
            {




                usedCells.Add(x + " " + y);
                for (int i = 0; i < gameEngine.CoinPiles.Count; i++)
                {
                    if (gameEngine.CoinPiles[i].PositionX == x && gameEngine.CoinPiles[i].PositionY == y)
                    {
                        if (gameEngine.CoinPiles[i].TimeLeft > coinPileList.Count)
                        {
                            return coinPileList;
                        }
                        return null;

                    }
                }
                if (isNextMoveVulnerable("UP") != "INVALID" && isNextMoveVulnerable("UP") != "BRICK" && isNextMoveVulnerable("UP") != "STONE" && isNextMoveVulnerable("UP") != "WATER" && !usedCells.Contains(x + " " + (y - 1)))
                {
                    List<string> coinPileListUp = new List<string>();
                    for (int i = 0; i < coinPileList.Count; i++)
                    {
                        coinPileListUp.Add(coinPileList[i]);
                    }
                    if (direction == Direction.NORTH)
                    {
                        coinPileListUp.Add("UP");
                    }
                    else
                    {
                        coinPileListUp.Add("UP");
                        coinPileListUp.Add("UP");
                    }


                    answer1 = calculatePath(x, y - 1, usedCells, limit - 1, coinPileListUp, Direction.NORTH);
                }
                if (isNextMoveVulnerable("DOWN") != "INVALID" && isNextMoveVulnerable("DOWN") != "BRICK" && isNextMoveVulnerable("DOWN") != "STONE" && isNextMoveVulnerable("DOWN") != "WATER" && !usedCells.Contains(x + " " + (y + 1)))
                {
                    List<string> coinPileListDown = new List<string>();
                    for (int i = 0; i < coinPileList.Count; i++)
                    {
                        coinPileListDown.Add(coinPileList[i]);
                    }
                    if (direction == Direction.SOUTH)
                    {
                        coinPileListDown.Add("DOWN");
                    }
                    else
                    {
                        coinPileListDown.Add("DOWN");
                        coinPileListDown.Add("DOWN");
                    }
                    answer2 = calculatePath(x, y + 1, usedCells, limit - 1, coinPileListDown, Direction.SOUTH);
                }
                if (isNextMoveVulnerable("RIGHT") != "INVALID" && isNextMoveVulnerable("RIGHT") != "BRICK" && isNextMoveVulnerable("RIGHT") != "STONE" && isNextMoveVulnerable("RIGHT") != "WATER" && !usedCells.Contains((x + 1) + " " + y))
                {
                    List<string> coinPileListRight = new List<string>();
                    for (int i = 0; i < coinPileList.Count; i++)
                    {
                        coinPileListRight.Add(coinPileList[i]);
                    }
                    if (direction == Direction.EAST)
                    {
                        coinPileListRight.Add("RIGHT");
                    }
                    else
                    {
                        coinPileListRight.Add("RIGHT");
                        coinPileListRight.Add("RIGHT");
                    }
                    answer3 = calculatePath(x + 1, y, usedCells, limit - 1, coinPileListRight, Direction.EAST);
                }
                if (isNextMoveVulnerable("LEFT") != "INVALID" && isNextMoveVulnerable("LEFT") != "BRICK" && isNextMoveVulnerable("LEFT") != "STONE" && isNextMoveVulnerable("LEFT") != "WATER" && !usedCells.Contains((x - 1) + " " + y))
                {
                    List<string> coinPileListLeft = new List<string>();
                    for (int i = 0; i < coinPileList.Count; i++)
                    {
                        coinPileListLeft.Add(coinPileList[i]);
                    }
                    if (direction == Direction.WEST)
                    {
                        coinPileListLeft.Add("LEFT");
                    }
                    else
                    {
                        coinPileListLeft.Add("LEFT");
                        coinPileListLeft.Add("LEFT");
                    }
                    answer4 = calculatePath(x - 1, y, usedCells, limit - 1, coinPileListLeft, Direction.WEST);
                }

                int count1 = (answer1 != null) ? answer1.Count : 100;
                int count2 = (answer2 != null) ? answer2.Count : 100;
                int count3 = (answer3 != null) ? answer3.Count : 100;
                int count4 = (answer4 != null) ? answer4.Count : 100;

                int min = count1;
                if (count2 < min) { min = count2; }

                if (count3 < min) { min = count3; }

                if (count4 < min) { min = count4; }
                if (min == 100) { return null; }
                if (min == count1) { return answer1; }
                if (min == count2) { return answer2; }
                if (min == count3) { return answer3; }
                if (min == count4) { return answer4; }
                return null;
            }
            else
            {
                return coinPileList;
            }


        }

        private CoinPile findTargetCoinPile(List<string> pathList)
        {
            if (pathList != null)
            {
                int x = ownedTank.PositionX;
                int y = ownedTank.PositionY;
                Direction direction = ownedTank.Direction;
                for (int i = 0; i < pathList.Count; i++)
                {
                    if (pathList[i] == "UP")
                    {
                        if (direction == Direction.NORTH)
                        {
                            y = y - 1;
                        }
                        else
                        {
                            direction = Direction.NORTH;
                        }

                    }
                    else if (pathList[i] == "DOWN")
                    {
                        if (direction == Direction.SOUTH)
                        {
                            y = y + 1;
                        }
                        else
                        {
                            direction = Direction.SOUTH;
                        }


                    }
                    else if (pathList[i] == "LEFT")
                    {
                        if (direction == Direction.WEST)
                        {
                            x = x - 1;
                        }
                        else
                        {
                            direction = Direction.WEST;
                        }

                    }
                    else if (pathList[i] == "RIGHT")
                    {

                        if (direction == Direction.EAST)
                        {
                            x = x + 1;
                        }
                        else
                        {
                            direction = Direction.EAST;
                        }

                    }

                }
                for (int i = 0; i < gameEngine.CoinPiles.Count; i++)
                {
                    if (gameEngine.CoinPiles[i].PositionX == x && gameEngine.CoinPiles[i].PositionY == y)
                    {
                        return gameEngine.CoinPiles[i];
                    }
                } return null;
            }
            else
            {
                return null;
            }
        }

        public void CalculateMove()
        {
            treeDepth = Constants.Instance.AITreeDepth;
            //
            gameEngine = GameManager.Instance.GameEngine;
            map = gameEngine.Map;
            ownedTank = gameEngine.Tanks[gameEngine.PlayerNumber];
            if (canShoot())
            {
                GameManager.Instance.CurrentTank.Shoot();
            }
            else if (ownedTank.Health >= 3)
            {
                //go for a life pack
                //

            }
            else
            {
                //go for a coin pile
                //
                if (coinPileSteps == null)
                {
                    coinPileSteps = new List<string>();
                    List<string> usedCells = new List<string>();
                    coinPileSteps = calculatePath(ownedTank.PositionX, ownedTank.PositionY, usedCells, treeDepth, coinPileSteps, ownedTank.Direction);
                    targetCoinPile = findTargetCoinPile(coinPileSteps);

                    if (coinPileSteps == null)
                    {
                        targetCoinPile = null;

                        if (isNextMoveVulnerable("UP") != "INVALID" && isNextMoveVulnerable("UP") != "BRICK" && isNextMoveVulnerable("UP") != "STONE" && isNextMoveVulnerable("UP") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveUp();
                        }
                        else if (isNextMoveVulnerable("DOWN") != "INVALID" && isNextMoveVulnerable("DOWN") != "BRICK" && isNextMoveVulnerable("DOWN") != "STONE" && isNextMoveVulnerable("DOWN") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveDown();
                        }
                        else if (isNextMoveVulnerable("LEFT") != "INVALID" && isNextMoveVulnerable("LEFT") != "BRICK" && isNextMoveVulnerable("LEFT") != "STONE" && isNextMoveVulnerable("LEFT") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveLeft();
                        }
                        else if (isNextMoveVulnerable("RIGHT") != "INVALID" && isNextMoveVulnerable("RIGHT") != "BRICK" && isNextMoveVulnerable("RIGHT") != "STONE" && isNextMoveVulnerable("RIGHT") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveRight();
                        }
                        else
                        {
                            GameManager.Instance.CurrentTank.Shoot();
                        }
                    }
                    else if (coinPileSteps.Count == 0)
                    {
                        coinPileSteps = null;
                        targetCoinPile = null;
                        if (isNextMoveVulnerable("UP") != "INVALID" && isNextMoveVulnerable("UP") != "BRICK" && isNextMoveVulnerable("UP") != "STONE" && isNextMoveVulnerable("UP") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveUp();
                        }
                        else if (isNextMoveVulnerable("DOWN") != "INVALID" && isNextMoveVulnerable("DOWN") != "BRICK" && isNextMoveVulnerable("DOWN") != "STONE" && isNextMoveVulnerable("DOWN") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveDown();
                        }
                        else if (isNextMoveVulnerable("LEFT") != "INVALID" && isNextMoveVulnerable("LEFT") != "BRICK" && isNextMoveVulnerable("LEFT") != "STONE" && isNextMoveVulnerable("LEFT") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveLeft();
                        }
                        else if (isNextMoveVulnerable("RIGHT") != "INVALID" && isNextMoveVulnerable("RIGHT") != "BRICK" && isNextMoveVulnerable("RIGHT") != "STONE" && isNextMoveVulnerable("RIGHT") != "WATER")
                        {
                            GameManager.Instance.CurrentTank.MoveRight();
                        }
                        else
                        {
                            GameManager.Instance.CurrentTank.Shoot();
                        }

                    }
                    else
                    {
                        targetCoinPile = findTargetCoinPile(coinPileSteps);
                        if (targetCoinPile != null)
                        {
                            String move = coinPileSteps[0];
                            coinPileSteps.RemoveAt(0);
                            if (move == "UP")
                            {
                                GameManager.Instance.CurrentTank.MoveUp();
                            }
                            else if (move == "DOWN")
                            {
                                GameManager.Instance.CurrentTank.MoveDown();
                            }
                            else if (move == "RIGHT")
                            {
                                GameManager.Instance.CurrentTank.MoveRight();
                            }
                            else if (move == "LEFT")
                            {
                                GameManager.Instance.CurrentTank.MoveLeft();
                            }

                            if (coinPileSteps.Count == 0)
                            {
                                coinPileSteps = null;
                                healthPileSteps = null;
                                targetCoinPile = null;
                                targetLifePack = null;
                            }
                        }
                        else
                        {
                            coinPileSteps = null;
                            healthPileSteps = null;
                            targetCoinPile = null;
                            targetLifePack = null;
                        }
                    }
                }
                else
                {
                    targetCoinPile = findTargetCoinPile(coinPileSteps);
                    if (targetCoinPile != null)
                    {
                        String move = coinPileSteps[0];
                        coinPileSteps.RemoveAt(0);
                        if (move == "UP")
                        {
                            GameManager.Instance.CurrentTank.MoveUp();
                        }
                        else if (move == "DOWN")
                        {
                            GameManager.Instance.CurrentTank.MoveDown();
                        }
                        else if (move == "RIGHT")
                        {
                            GameManager.Instance.CurrentTank.MoveRight();
                        }
                        else if (move == "LEFT")
                        {
                            GameManager.Instance.CurrentTank.MoveLeft();
                        }

                        if (coinPileSteps.Count == 0)
                        {
                            coinPileSteps = null;
                            healthPileSteps = null;
                            targetCoinPile = null;
                            targetLifePack = null;
                        }
                    }
                    else
                    {
                        coinPileSteps = null;
                        healthPileSteps = null;
                        targetCoinPile = null;
                        targetLifePack = null;
                    }
                }

            }



        }
    }
}
