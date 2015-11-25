using System.Net.Sockets;

using Assets.Game.Communication;

namespace Assets.Game
{
    class GameManager
    {
        #region Variables
        private GameEngine gameEngine;
        public GameEngine GameEngine
        {
            get
            {
                return gameEngine;
            }
        }

        private Connection connection;
        public string ServerIP
        {
            get
            {
                return connection.ServerIP;
            }
        }
        public int ServerPort
        {
            get
            {
                return connection.ServerPort;
            }
        }

        private Parser parser;

        private AI ai;
        public AI AI
        {
            get
            {
                return ai;
            }
        }

        private TankMover currentTank;
        public TankMover CurrentTank
        {
            get
            {
                return currentTank;
            }
        }

        private GameMode mode;
        public GameMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

        private GameState state;
        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        private ServerMessage message;
        public ServerMessage Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        private GameError error;
        public GameError Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }
        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null) instance = new GameManager();
                return instance;
            }
        }
        private GameManager()
        {
            Initialize();
        }
        #endregion

        private void Initialize()
        {
            gameEngine = new GameEngine();
            connection = new Connection();
            parser = new Parser();
            ai = new AI();

            mode = GameMode.AUTO;
            state = GameState.IDLE;
            message = ServerMessage.NO_ISSUES;
            error = GameError.NO_ERROR;

            /*
             * For collecting all the object discarded
             ** invoked garbage collector to  improve the performance after the game start
            */
            System.GC.Collect();
        }

        public void JoinServer(string ip, int port)
        {
            // Connecting the messageReceived event to the handler
            connection.MessageReceived += parser.OnMessageReceived;

            connection.ServerIP = ip;
            connection.ServerPort = port;
            try {
                connection.StartConnection();
                connection.StartReceiving();
                error = GameError.NO_ERROR;
            } catch(SocketException) {
                error = GameError.NO_SERVER_DETECTED;
            }
        }

        public void RestartGame()
        {
            Initialize();
        }

        /*
         * For moving the tank
         * Used by AI or by the key press handler to move the tank owned by this client
        */
        public struct TankMover
        {
            public void MoveUp()
            {
                GameManager.Instance.connection.SendData("UP#");
            }

            public void MoveRight()
            {
                GameManager.Instance.connection.SendData("RIGHT#");
            }

            public void MoveDown()
            {
                GameManager.Instance.connection.SendData("DOWN#");
            }

            public void MoveLeft()
            {
                GameManager.Instance.connection.SendData("LEFT#");
            }

            public void Shoot()
            {
                GameManager.Instance.connection.SendData("SHOOT#");
            }
        }
    }

    /* 
     * For indicating the game Mode
     * AUTO - tank owned by the client is driven by the AI
     * MANUAL - tank owned by the client is driven by the user
    */
    public enum GameMode {
        AUTO,
        MANUAL
    }

    // For indicating the state of the game
    public enum GameState {
        IDLE,
        STARTED,
        INITIATED,
        PROGRESSING,
        ENDED
    }

    // For indicating the last server message
    public enum ServerMessage {
        NO_ISSUES,
        PLAYERS_FULL,
        ALREADY_ADDED,
        GAME_ALREADY_STARTED,
        OBSTACLE,
        CELL_OCCUPIED,
        DEAD,
        TOO_QUICK,
        INVALID_CELL,
        GAME_HAS_FINISHED,
        GAME_NOT_STARTED_YET,
        NOT_A_VALID_CONTESTANT,
        GAME_FINISHED,
        PITFALL
    }

    // For indicating errors
    public enum GameError {
        NO_ERROR,
        NO_SERVER_DETECTED
    }
}
