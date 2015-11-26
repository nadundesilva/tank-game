using System;
using System.Collections.Generic;

using Assets.Game.GameEntities;

namespace Assets.Game.Communication
{
    class Parser
    {
        /*
         * Used for mapping the integer sent by the server to the enum Direction
        */
        private Direction[] direction = new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };

        /*
         * Routing to the correct method that handles the message or throwing exception if invalid
        */
        private void Parse(string message)
        {
            // This method checks the message type and invoke the nessasary method accordingly
            if (message.Substring(message.Length - 1) == "#" && message.Length > 2)
            {
                if (message.Substring(0, 1) == "S")             // Startup message
                {
                    ParseStartUpMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.STARTED;
                }
                else if (message.Substring(0, 1) == "I")        // Initialize game message
                {
                    ParseInitializeMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.INITIATED;
                }
                else if (message.Substring(0, 1) == "G")        // Parse game message
                {
                    ParseGameMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                    GameManager.Instance.State = GameState.PROGRESSING;
                }
                else if (message.Substring(0, 1) == "C")        // Coin pile message
                {
                    ParseCoinPileMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                }
                else if (message.Substring(0, 1) == "L")        // Life packet message
                {
                    ParseLifePackMessage(message.Substring(2, message.Length - 3).Split(':'));
                    GameManager.Instance.Message = ServerMessage.NO_ISSUES;
                }
                else if (message == "PLAYERS_FULL#")            // Player full exception
                {
                    GameManager.Instance.Message = ServerMessage.PLAYERS_FULL;
                }
                else if (message == "ALREADY_ADDED#")           // Already added exception
                {
                    GameManager.Instance.Message = ServerMessage.ALREADY_ADDED;
                }
                else if (message == "GAME_ALREADY_STARTED#")    // Game already stated
                {
                    GameManager.Instance.Message = ServerMessage.GAME_ALREADY_STARTED;
                }
                else if (message == "OBSTACLE#")                // Obstacle in the square
                {
                    GameManager.Instance.Message = ServerMessage.OBSTACLE;
                }
                else if (message == "CELL_OCCUPIED#")           // Square already occupied by another tank
                {
                    GameManager.Instance.Message = ServerMessage.CELL_OCCUPIED;
                }
                else if (message == "DEAD#")                    // Player dead
                {
                    GameManager.Instance.Message = ServerMessage.DEAD;
                }
                else if (message == "TOO_QUICK#")               // Not enough time between moves
                {
                    GameManager.Instance.Message = ServerMessage.TOO_QUICK;
                }
                else if (message == "INVALID_CELL#")            // Invalid square
                {
                    GameManager.Instance.Message = ServerMessage.INVALID_CELL;
                }
                else if (message == "GAME_HAS_FINISHED#")       // Game had already finished
                {
                    GameManager.Instance.Message = ServerMessage.GAME_NOT_STARTED_YET;
                    GameManager.Instance.State = GameState.ENDED;
                }
                else if (message == "GAME_NOT_STARTED_YET#")    // Game not yet started
                {
                    GameManager.Instance.Message = ServerMessage.GAME_HAS_FINISHED;
                }
                else if (message == "NOT_A_VALID_CONTESTANT#")  // Not a valid contestant
                {
                    GameManager.Instance.Message = ServerMessage.NOT_A_VALID_CONTESTANT;
                }
                else if (message == "GAME_FINISHED#")           // Game had finished
                {
                    GameManager.Instance.Message = ServerMessage.GAME_FINISHED;
                    GameManager.Instance.State = GameState.ENDED;
                }
                else if (message == "PITFALL#")                 // Game had finished
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
        
        /*
         * Handling the game startup message
        */
        private void ParseStartUpMessage(string[] tokens)
        {
            // Placing the client's tank on the map
            foreach (string s in tokens)
            {
                string[] playerData = s.Split(';');
                string[] location = playerData[1].Split(',');
                Tank tank = new Tank(int.Parse(playerData[0].Substring(1)), int.Parse(location[0]), int.Parse(location[1]), direction[int.Parse(playerData[2])]);
                GameManager.Instance.GameEngine.AddTank(tank);
            }
        }

        /*
         * Handling the game initilization message
        */
        private void ParseInitializeMessage(string[] tokens)
        {
            GameManager.Instance.GameEngine.PlayerNumber = int.Parse(tokens[0].Substring(1));

            // Placing the brick walls on the map
            string[] brickWallStrings = tokens[1].Split(';');
            List<BrickWall> brickWalls = new List<BrickWall>();
            foreach (string s in brickWallStrings)
            {
                string[] location = s.Split(',');
                BrickWall b = new BrickWall(int.Parse(location[0]), int.Parse(location[1]));
                brickWalls.Add(b);
            }
            GameManager.Instance.GameEngine.BrickWalls = brickWalls;

            // Placing the stone walls on the map
            string[] stoneWallStrings = tokens[2].Split(';');
            List<StoneWall> stoneWalls = new List<StoneWall>();
            foreach (string s in stoneWallStrings)
            {
                string[] location = s.Split(',');
                StoneWall b = new StoneWall(int.Parse(location[0]), int.Parse(location[1]));
                stoneWalls.Add(b);
            }
            GameManager.Instance.GameEngine.StoneWalls = stoneWalls;

            // Placing water on the map
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

        /*
         * Handling game update messages
        */
        private void ParseGameMessage(string[] tokens)
        {
            // Sending the clock for the game engine
            GameManager.Instance.GameEngine.Clock();

            // Changing the location of the players on the map
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

            // Updating the bricks
            string[] brickStrings = tokens[i].Split(';');
            List<BrickWall> brickWalls = new List<BrickWall>();
            foreach (string s in brickStrings)
            {
                string[] brickData = s.Split(',');
                int brickHealth = int.Parse(brickData[2]);
                if (brickHealth < 4) {
                    BrickWall b = new BrickWall(int.Parse(brickData[0]), int.Parse(brickData[1]));
                    b.Damage = brickHealth;
                    brickWalls.Add(b);
                }
            }
            GameManager.Instance.GameEngine.BrickWalls = brickWalls;

            // Updating the game by removing the collectibles from the map if necessary
            GameManager.Instance.GameEngine.UpdateGame();

            // Calling AI for move if auto mode is on
            if (GameManager.Instance.Mode == GameMode.AUTO)
                GameManager.Instance.AI.CalculateMove();
        }

        /*
         * Placing the new coin pile on the map
        */
        private void ParseCoinPileMessage(string[] tokens)
        {
            string[] location = tokens[0].Split(',');
            CoinPile coinPile = new CoinPile(int.Parse(location[0]), int.Parse(location[1]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            GameManager.Instance.GameEngine.AddCoinPile(coinPile);
        }

        /*
         * Placing the life pack on the map
        */
        private void ParseLifePackMessage(string[] tokens)
        {
            string[] location = tokens[0].Split(',');
            LifePack lifePack = new LifePack(int.Parse(location[0]), int.Parse(location[1]), int.Parse(tokens[1]));
            GameManager.Instance.GameEngine.AddLifePack(lifePack);
        }

        /*
         * Used for handling the event published by the listener thread in the connection class
         * Calls the parse message
        */
        public void OnMessageReceived(object source, EventArgs a)
        {
            string message = ((MessageReceivedEventArgs)a).Message;
            try {
                Parse(message.Substring(0, message.Length - 1));
            }
            catch (FormatException)
            {
                throw new System.ArgumentException("Invalid start in the message from the server : " + message, "Message");
            }
        }
    }
}
