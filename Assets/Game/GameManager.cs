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

        private ClientConnection client;
        public ClientConnection Client
        {
            get
            {
                return client;
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

        public void Initialize()
        {
            gameEngine = new GameEngine();
            parser = new Parser();
            ai = new AI();

            state = GameState.IDLE;
            message = ServerMessage.NO_ISSUES;
        }

        public void JoinServer(string ip, int port)
        {
            //Instatiating client and connecting the messageReceived event to the handler
            client = new ClientConnection();
            client.MessageReceived += parser.OnMessageReceived;

            client.ServerIP = ip;
            client.ServerPort = port;
            client.StartConnection();
            client.StartReceiving();
        }
    }

    public enum GameState {
        IDLE,
        STARTED,
        INITIATED,
        PROGRESSING,
        ENDED
    }

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
}
