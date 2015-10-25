using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Game.GameEntities;

namespace Assets.Game.Communication
{
    class Parser
    {
        private Direction[] direction = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };
        //only 4 directions are allowed
        private void Parse(string message)
        {
            if (message.Substring(message.Length - 1) == "#" && message.Length > 2)
            {
                if (message.Substring(0, 1) == "S")
                {
                    ParseStartUpMessage(message.Substring(2, message.Length - 3).Split(':'));
                }
                else if (message.Substring(0, 1) == "P")
                {
                    //players full
                }
                else if (message.Substring(0, 1) == "A")
                {
                    //already added
                }
                else if (message.Substring(0, 4) == "GAME")
                {
                    //The player tries to join an already started game 

                }
                else if (message.Substring(0, 1) == "I")
                {
                    ParseInitializeMessage(message.Substring(2, message.Length - 3).Split(':'));
                }
                else if (message.Substring(0, 3) == "G:P")
                {
                    ParseGameMessage(message.Substring(2, message.Length - 3).Split(':'));
                }
                else if (message.Substring(0, 1) == "C")
                {
                    ParseCoinPileMessage(message.Substring(2, message.Length - 3).Split(':'));
                }
                else if (message.Substring(0, 1) == "L")
                {
                    ParseLifePackMessage(message.Substring(2, message.Length - 3).Split(':'));
                }
                else
                {
                    throw new System.ArgumentException("Invalid start in the message from the server : " + message, "Message");
                }
            }
            else
            {
                throw new System.ArgumentException("Invalid ending in the message from the server : " + message, "Message");
            }
        }



        private void ParseStartUpMessage(string[] tokens)
        {
            //placing the client's tank on the map
            foreach (string s in tokens)
            {
                string[] playerData = s.Split(';');
                string[] location = playerData[1].Split(',');
                Tank tank = new Tank(int.Parse(playerData[0].Substring(1)), int.Parse(location[0]), int.Parse(location[1]), direction[int.Parse(playerData[2])]);
                GameManager.Instance.GameEngine.AddTank(tank);
            }
        }

        private void ParseInitializeMessage(string[] tokens)
        {
            //placing the brick walls on the map
            string[] brickWallStrings = tokens[1].Split(';');
            List<BrickWall> brickWalls = new List<BrickWall>();
            foreach (string s in brickWallStrings)
            {
                string[] location = s.Split(',');
                BrickWall b = new BrickWall(int.Parse(location[0]), int.Parse(location[1]));
                brickWalls.Add(b);
            }
            GameManager.Instance.GameEngine.BrickWalls = brickWalls;

            //placing the stone walls on the map
            string[] stoneWallStrings = tokens[2].Split(';');
            List<StoneWall> stoneWalls = new List<StoneWall>();
            foreach (string s in stoneWallStrings)
            {
                string[] location = s.Split(',');
                StoneWall b = new StoneWall(int.Parse(location[0]), int.Parse(location[1]));
                stoneWalls.Add(b);
            }
            GameManager.Instance.GameEngine.StoneWalls = stoneWalls;

            //placing water on the map
            string[] waterStrings = tokens[3].Split(';');
            List<Water> water = new List<Water>();
            foreach (string s in waterStrings)
            {
                string[] location = s.Split(',');
                Water b = new Water(int.Parse(location[0]), int.Parse(location[1]));
                water.Add(b);
            }
            GameManager.Instance.GameEngine.Water = water;
        }

        private void ParseGameMessage(string[] tokens)
        {
            //changing the location of the players on the map
            int i = 0;
            while (i < tokens.Length - 1)
            {
                string[] playerData = tokens[i].Split(';');
                Tank tank = GameManager.Instance.GameEngine.Tanks[int.Parse(playerData[0].Substring(1))];

                string[] location = playerData[1].Split(',');
                tank.PositionX = int.Parse(location[0]);
                tank.PositionY = int.Parse(location[1]);
                tank.Direction = direction[int.Parse(playerData[2])];

                if (playerData[3] == "1")
                    tank.Shoot();

                tank.Health = int.Parse(playerData[4]);
                tank.Coins = int.Parse(playerData[5]);
                tank.Points = int.Parse(playerData[6]);
                i++;
            }

            //updating the bricks
            string[] brickStrings = tokens[i].Split(';');
            List<BrickWall> brickWalls = new List<BrickWall>();
            foreach (string s in brickStrings)
            {
                string[] brickData = s.Split(',');
                BrickWall b = new BrickWall(int.Parse(brickData[0]), int.Parse(brickData[1]));
                b.Health = int.Parse(brickData[2]);
                brickWalls.Add(b);
            }
            GameManager.Instance.GameEngine.BrickWalls = brickWalls;
        }

        private void ParseCoinPileMessage(string[] tokens)
        {
            //placing the new coin pile on the map
            string[] location = tokens[0].Split(',');
            CoinPile coinPile = new CoinPile(int.Parse(location[0]), int.Parse(location[1]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            GameManager.Instance.GameEngine.AddCoinPile(coinPile);
        }

        private void ParseLifePackMessage(string[] tokens)
        {
            //placing the life pack on the map
            string[] location = tokens[0].Split(',');
            LifePack lifePack = new LifePack(int.Parse(location[0]), int.Parse(location[1]), int.Parse(tokens[1]));
            GameManager.Instance.GameEngine.AddLifePack(lifePack);
        }

        public void OnMessageReceived(object source, EventArgs a)
        {
            string message = ((MessageReceivedEventArgs)a).Message;
            Parse(message.Substring(0, message.Length - 1));

            GameManager.Instance.GameEngine.Clock();
        }
    }
}
