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
        private ObjectCell[,] cells = null;
       

        #region Variables for storing references to important objects
        #endregion

        public AI()
        {

        }
        public void calculateArray(int x , int y ){
            cells = new ObjectCell[10, 10];
            for (int i = 0; i < 10; i++ ) {
                for (int j = 0; j < 10; j++ ) {
                    cells[i, j] = new ObjectCell();
                }
            }
            for (int i = 0; i < gameEngine.StoneWalls.Count; i++ ) {
                int xPoint = gameEngine.StoneWalls[i].PositionX;
                int yPoint = gameEngine.StoneWalls[i].PositionY;
                cells[xPoint, yPoint].category = "S";
            }
            for (int i = 0; i < gameEngine.BrickWalls.Count; i++)
            {
                int xPoint = gameEngine.BrickWalls[i].PositionX;
                int yPoint = gameEngine.BrickWalls[i].PositionY;
                cells[xPoint, yPoint].category = "B";
            }
            for (int i = 0; i < gameEngine.Water.Count; i++)
            {
                int xPoint = gameEngine.Water[i].PositionX;
                int yPoint = gameEngine.Water[i].PositionY;
                cells[xPoint, yPoint].category = "W";
            }
            cells[x, y].count = 0;
            cells[x, y].direction= ownedTank.Direction;
            cells[x, y].predesessor = null;
            cells[x, y].state= "S";

            fillArray(x,y);
            
        }
        public void relax(int x, int y){
           
            if(y>0){
                //go up
                if (!(cells[x, y-1].state.Equals("E")))
                {
                    if (cells[x, y].direction == Direction.NORTH && cells[x, y-1].count > cells[x, y].count + 1)
                    {
                        cells[x, y - 1].count = cells[x,y].count + 1;
                        cells[x, y - 1].direction = Direction.NORTH;
                        cells[x, y - 1].predesessor = cells[x, y];
                        cells[x, y - 1].state = "S";
                        for (int i = 0; i < cells[x,y].moves.Count; i++)
                        {
                            cells[x,y-1].moves.Add(cells[x,y].moves[i]);
                        }
                        cells[x, y - 1].moves.Add("UP");
                    }
                    else if (cells[x, y].direction != Direction.NORTH && cells[x, y - 1].count > cells[x, y].count + 2)
                    {
                        cells[x, y - 1].count = cells[x, y].count + 2;
                        cells[x, y - 1].direction = Direction.NORTH;
                        cells[x, y - 1].predesessor = cells[x, y];
                        cells[x, y - 1].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x, y - 1].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x, y - 1].moves.Add("UP");
                        cells[x, y - 1].moves.Add("UP");

                    }
                } 



            }
            if(y<9){
                //go down

                if (!(cells[x, y + 1].state.Equals("E")))
                {
                    if (cells[x, y].direction == Direction.SOUTH && cells[x, y + 1].count > cells[x, y].count + 1)
                    {
                        cells[x, y + 1].count = cells[x, y].count + 1;
                        cells[x, y + 1].direction = Direction.SOUTH;
                        cells[x, y + 1].predesessor = cells[x, y];
                        cells[x, y + 1].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x, y + 1].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x, y + 1].moves.Add("DOWN");
                    }
                    else if (cells[x, y].direction != Direction.SOUTH && cells[x, y + 1].count > cells[x, y].count + 2)
                    {
                        cells[x, y + 1].count = cells[x, y].count + 2;
                        cells[x, y + 1].direction = Direction.SOUTH;
                        cells[x, y + 1].predesessor = cells[x, y];
                        cells[x, y + 1].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x, y + 1].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x, y + 1].moves.Add("DOWN");
                        cells[x, y + 1].moves.Add("DOWN");

                    }
                } 



            }
            if(x>0){
                //go left

                if (!(cells[x-1, y].state.Equals("E")))
                {
                    if (cells[x, y].direction == Direction.WEST && cells[x - 1, y].count > cells[x, y].count + 1)
                    {
                        cells[x - 1, y].count = cells[x, y].count + 1;
                        cells[x - 1, y].direction = Direction.WEST;
                        cells[x - 1, y].predesessor = cells[x, y];
                        cells[x - 1, y].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x - 1, y].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x - 1, y].moves.Add("LEFT");
                    }
                    else if (cells[x, y].direction != Direction.WEST && cells[x - 1, y].count > cells[x, y].count + 2)
                    {
                        cells[x - 1, y].count = cells[x, y].count + 2;
                        cells[x - 1, y].direction = Direction.WEST;
                        cells[x - 1, y].predesessor = cells[x, y];
                        cells[x - 1, y].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x - 1, y].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x - 1, y].moves.Add("LEFT");
                        cells[x - 1, y].moves.Add("LEFT");

                    }
                }



            }
            if(x<9){
                //go right

                if (!(cells[x + 1, y].state.Equals("E")))
                {
                    if (cells[x, y].direction == Direction.EAST && cells[x + 1, y].count > cells[x, y].count + 1)
                    {
                        cells[x + 1, y].count = cells[x, y].count + 1;
                        cells[x + 1, y].direction = Direction.EAST;
                        cells[x + 1, y].predesessor = cells[x, y];
                        cells[x + 1, y].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x + 1, y].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x + 1, y].moves.Add("RIGHT");
                    }
                    else if (cells[x, y].direction != Direction.EAST && cells[x + 1, y].count > cells[x, y].count + 2)
                    {
                        cells[x + 1, y].count = cells[x, y].count + 2;
                        cells[x + 1, y].direction = Direction.EAST;
                        cells[x + 1, y].predesessor = cells[x, y];
                        cells[x + 1, y].state = "S";
                        for (int i = 0; i < cells[x, y].moves.Count; i++)
                        {
                            cells[x + 1, y].moves.Add(cells[x, y].moves[i]);
                        }
                        cells[x + 1, y].moves.Add("RIGHT");
                        cells[x + 1, y].moves.Add("RIGHT");

                    }
                }


            }
            

            
        }

        public void fillArray(int x , int y){
            if (!(cells[x, y].category.Equals("W")) && !(cells[x, y].category.Equals("B")) && !(cells[x, y].category.Equals("S")))
            {
                relax(x, y);
                cells[x, y].state = "E";
            }
            else {
                cells[x, y].state = "E";
            }  
            int min = Int32.MaxValue;
            Boolean found = false;
            int newX = -1 ;
            int newY = -1;
            for(int i = 0 ; i <10 ;i++ ){
                for(int j = 0 ;j<10;j++){
                    if((cells[i,j].state.Equals("S")) && cells[i,j].count<min) {
                        min = cells[i,j].count;
                        found = true;
                        newX = i ;
                        newY = j;
                    }
                }
            }if(found){
                fillArray(newX,newY);                                
            }   
      
        }
         

        /*
         * Calculates the best move based on the state of the game
        */
        
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
        public void makeExcatMove(string move)
        {
            if (move.Equals("UP"))
            {
                GameManager.Instance.CurrentTank.MoveUp();
            }
            else if (move.Equals("DOWN"))
            {
                GameManager.Instance.CurrentTank.MoveDown();
            }
            else if (move.Equals("RIGHT"))
            {
                GameManager.Instance.CurrentTank.MoveRight();
            }
            else if (move.Equals("LEFT"))
            {
                GameManager.Instance.CurrentTank.MoveLeft();
            }
            else if (move.Equals("SHOOT")) {
                GameManager.Instance.CurrentTank.Shoot();
            }

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
                        if (ownedTank.Direction.ToString().Equals(Direction.NORTH.ToString()) && Math.Abs(y - gameEngine.Tanks[i].PositionY) < 2 && gameEngine.Tanks[i].PositionY < y)
                        {
                            return true;
                        }

                    }
                    else
                    {
                        if (ownedTank.Direction.ToString().Equals(Direction.SOUTH.ToString()) && Math.Abs(y - gameEngine.Tanks[i].PositionY) < 2 && gameEngine.Tanks[i].PositionY > y)
                        {
                            return true;
                        }

                    }

                }
                if (gameEngine.Tanks[i].PositionY == y)
                {
                    if (x > gameEngine.Tanks[i].PositionX)
                    {
                        if (ownedTank.Direction.ToString().Equals(Direction.WEST.ToString()) && Math.Abs(x - gameEngine.Tanks[i].PositionX) < 2 && gameEngine.Tanks[i].PositionX < x)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (ownedTank.Direction.ToString().Equals(Direction.EAST.ToString()) && Math.Abs(x - gameEngine.Tanks[i].PositionX) < 2 && gameEngine.Tanks[i].PositionX > x)
                        {
                            return true;
                        }

                    }

                }

            }





            return false;

        }

       public void CalculateMove()
        {

            treeDepth = 10; 
            gameEngine = GameManager.Instance.GameEngine;
            map = gameEngine.Map;
            ownedTank = gameEngine.Tanks[gameEngine.PlayerNumber];
            calculateArray(ownedTank.PositionX,ownedTank.PositionY);
            if (ownedTank.Health < 30)
            {
                //go for a health pile
                int minx = -1;
                int miny = -1;
                int minCount = Int16.MaxValue;
                for (int i = 0; i < gameEngine.LifePacks.Count;i++ ) {
                    int x = gameEngine.LifePacks[i].PositionX;
                    int y = gameEngine.LifePacks[i].PositionY;
                    if(cells[x,y].moves.Count<gameEngine.LifePacks[i].TimeLeft){
                        if (minCount > cells[x, y].moves.Count) {
                            minCount = cells[x, y].moves.Count;
                            minx = x;
                            miny = y;
                        }
                    }

                }
                if(minCount!=Int16.MaxValue){
                    //target is in minx , miny
                    makeExcatMove(cells[minx,miny].moves[0]);
                }
                else if (canShoot())
                {
                    makeExcatMove("SHOOT");
                }

            }
            
            {
                //go for a coin pile
                int minx = -1;
                int miny = -1;
                int minCount = Int16.MaxValue;
                for (int i = 0; i < gameEngine.CoinPiles.Count; i++)
                {
                    int x = gameEngine.CoinPiles[i].PositionX;
                    int y = gameEngine.CoinPiles[i].PositionY;
                    if (cells[x, y].moves.Count < gameEngine.CoinPiles[i].TimeLeft)
                    {
                        if (minCount > cells[x, y].moves.Count)
                        {
                            minCount = cells[x, y].moves.Count;
                            minx = x;
                            miny = y;
                        }
                    }

                }
                if (minCount != Int16.MaxValue)
                {
                    //target is in minx , miny
                    makeExcatMove(cells[minx, miny].moves[0]);
                } if (canShoot())
                {
                    makeExcatMove("SHOOT");
                }

            }
        }            
        
    }
}
