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
            //this method checks the message type and invoke the nessasary method accordingly
            if (message.Substring(message.Length - 1) == "#" && message.Length > 2)
            {
                if (message.Substring(0, 1) == "S")//startup message
                {
                    ParseStartUpMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.STARTED;
                }
                else if (message.Substring(0, 1) == "I")//initialize game message
                {
                    ParseInitializeMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.INITIATED;
                }
                else if (message.Substring(0, 1) == "G")//parse game message
                {
                    ParseGameMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.PROGRESSING;
                }
                else if (message.Substring(0, 1) == "C")//coin pile message
                {
                    ParseCoinPileMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                }
                else if (message.Substring(0, 1) == "L")// life packet message
                {
                    ParseLifePackMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                }
                else if (message == "PLAYERS_FULL#")//player full exception
                {
                    GameManager.Instance.Message = ServerMessage.PLAYERS_FULL;
                }
                else if (message == "ALREADY_ADDED#")//already added exception
                {
                    GameManager.Instance.Message = ServerMessage.ALREADY_ADDED;
                }
                else if (message == "GAME_ALREADY_STARTED#")//game already stated
                {
                    GameManager.Instance.Message = ServerMessage.GAME_ALREADY_STARTED;
                }
                else if (message == "OBSTACLE#")//obstacle in the square
                {
                    GameManager.Instance.Message = ServerMessage.OBSTACLE;
                }
                else if (message == "CELL_OCCUPIED#")//square already occupied by another tank
                {
                    GameManager.Instance.Message = ServerMessage.CELL_OCCUPIED;
                }
                else if (message == "DEAD#")//player dead
                {
                    GameManager.Instance.Message = ServerMessage.DEAD;
                    GameManager.Instance.State = GameState.ENDED;
                }
                else if (message == "TOO_QUICK#")//not enough time between moves
                {
                    GameManager.Instance.Message = ServerMessage.TOO_QUICK;
                }
                else if (message == "INVALID_CELL#")//invalid square
                {
                    GameManager.Instance.Message = ServerMessage.INVALID_CELL;
                }
                else if (message == "GAME_HAS_FINISHED#")//Game had already finished
                {
                    GameManager.Instance.Message = ServerMessage.GAME_NOT_STARTED_YET;
                    GameManager.Instance.State = GameState.ENDED;
                }
                else if (message == "GAME_NOT_STARTED_YET#")//game not yet started
                {
                    GameManager.Instance.Message = ServerMessage.GAME_HAS_FINISHED;
                }
                else if (message == "NOT_A_VALID_CONTESTANT#")//not a valid contestant
                {
                    GameManager.Instance.Message = ServerMessage.NOT_A_VALID_CONTESTANT;
                }
                else if (message == "GAME_FINISHED#")//game had finished
                {
                    GameManager.Instance.Message = ServerMessage.GAME_FINISHED;
                    GameManager.Instance.State = GameState.ENDED;
                }
                else if (message == "PITFALL#")//game had finished
                {
                    GameManager.Instance.Message = ServerMessage.PITFALL;
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

                GameManager.Instance.GameEngine.Map[tank.PositionX, tank.PositionY] = null;

                string[] location = playerData[1].Split(',');
                tank.PositionX = int.Parse(location[0]);
                tank.PositionY = int.Parse(location[1]);
                tank.Direction = direction[int.Parse(playerData[2])];

                if (playerData[3] == "1")
                    tank.Shoot();

                tank.Health = int.Parse(playerData[4]);
                tank.Coins = int.Parse(playerData[5]);
                tank.Points = int.Parse(playerData[6]);

                GameManager.Instance.GameEngine.Map[tank.PositionX, tank.PositionY] = tank;

                i++;
            }

            //updating the bricks
            string[] brickStrings = tokens[i].Split(';');
            List<BrickWall> brickWalls = new List<BrickWall>();
            foreach (string s in brickStrings)
            {
                string[] brickData = s.Split(',');
                int brickDamage = int.Parse(brickData[2]);
                if (brickDamage < 4) {
                    BrickWall b = new BrickWall(int.Parse(brickData[0]), int.Parse(brickData[1]));
                    b.Damage = brickDamage;
                    brickWalls.Add(b);
                }
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
            GameManager.Instance.GameEngine.Clock();

            string message = ((MessageReceivedEventArgs)a).Message;
            Parse(message.Substring(0, message.Length - 1));

            GameManager.Instance.GameEngine.UpdateGame();

            if (GameManager.Instance.Mode == GameMode.AUTO)
                GameManager.Instance.AI.CalculateMove();
        }
    }
}
