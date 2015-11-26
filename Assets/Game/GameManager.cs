using System.Net.Sockets;

using Assets.Game.Communication;

namespace Assets.Game
{
    class GameManager
    {
        #region Variables
        /*
         * Game engine used for simulating bullets, collectibles
         * Tanks, obstacles are not simulated and updated from the information received from the server
        */
        private GameEngine gameEngine;
        public GameEngine GameEngine
        {
            get
            {
                return gameEngine;
            }
        }

        /*
         * Used for connecting to the server
         * Listening thread published an event
         * The event handler calls the parser
        */
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

        /*
         * Used for parsing messages sent by the server
         * Called by the event handler for the event published by the connection listener thread
        */
        private Parser parser;

        private AI ai;
        public AI AI
        {
            get
            {
                return ai;
            }
        }

        /*
         * Used for storing the struct containing the methods for moving the tank owned by this client
         * All the methods sends the relevant message to the server
        */
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
            #region Initializing variables
            gameEngine = new GameEngine();
            connection = new Connection();
            parser = new Parser();
            ai = new AI();

            mode = GameMode.AUTO;
            state = GameState.IDLE;
            message = ServerMessage.NO_ISSUES;
            error = GameError.NO_ERROR;
            #endregion

            /*
             * For collecting all the object discarded
             ** invoked garbage collector to  improve the performance after the game start
            */
            System.GC.Collect();
        }

        /*
         * Used for joining the server specified by the ip and port
        */
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

        /*
         * Used for restarting the game
        */
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

    #region Enums
    /* 
     * For indicating the game Mode
     * AUTO - tank owned by the client is driven by the AI
     * MANUAL - tank owned by the client is driven by the user
    */
    public enum GameMode {
        AUTO,
        MANUAL
    }

    /*
     * For indicating the state of the game
     * IDLE - Game has not yet started
     * STARTED - Had received the start message
     * INITIATED - Had received the initialization message and had placed all obstacles
     * PROGRESSING - Game is progressing | Had started to receive the update messages
     * ENDED - Game had ended
    */
    public enum GameState {
        IDLE,
        STARTED,
        INITIATED,
        PROGRESSING,
        ENDED
    }

    /*
     * For indicating the last server message
     * NO_ISSUES - Default message used for indicating that there are no issues indicated in the last server message
     * PLAYERS_FULL - Server is already full and the client cannot enter the game
     * ALREADY_ADDED - The client had already joined the game on the server
     * GAME_ALREADY_STARTED - Game had already started in the server and this client cannot connect
     * OBSTACLE - The cell that the tank owned by this client wanted to move to has a obstacle placed
     * CELL_OCCUPIED - The cell that the tank owned by this client wanted to move to is already occupied by another tank
     * DEAD - The tank owned by this client is already dead
     * TOO_QUICK - The client had tried to send a move bwfore one second had elapsed since the last move
     * INVALID_CELL - The cell that the tank owned by this client wanted to move to is invalid
     * GAME_HAS_FINISHED - The game had already finished
     * GAME_NOT_STARTED_YET - The game had not yet started
     * NOT_A_VALID_CONTESTANT - The client is not a valid contestant participating in the current game
     * GAME_FINISHED - The game finished
     * PITFALL - The tank owned by this client had fallen into a pit of water
    */
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

    /*
     * For indicating errors
     * NO_ERROR - Default value to indicate no errors
     * NO_SERVER_DETECTED - cannot find or connect to the server specified by the user
    */
    public enum GameError {
        NO_ERROR,
        NO_SERVER_DETECTED
    }
    #endregion
}
