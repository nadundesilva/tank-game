using Assets.Game.Communication;
using System.Net.Sockets;

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

        private Connection client;
        public Connection Client
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

        public void Initialize()
        {
            gameEngine = new GameEngine();
            client = new Connection();
            parser = new Parser();
            ai = new AI();

            mode = GameMode.AUTO;
            state = GameState.IDLE;
            message = ServerMessage.NO_ISSUES;
            error = GameError.NO_ERROR;
        }

        public void JoinServer(string ip, int port)
        {
            //connecting the messageReceived event to the handler
            client.MessageReceived += parser.OnMessageReceived;

            client.ServerIP = ip;
            client.ServerPort = port;
            try {
                client.StartConnection();
                client.StartReceiving();
                error = GameError.NO_ERROR;
            } catch(SocketException) {
                error = GameError.NO_SERVER_DETECTED;
            }
        }

        public void RestartServer()
        {
            string ip = client.ServerIP;
            int port = client.ServerPort;
            client = new Connection();
            JoinServer(ip, port);
        }

        public void RestartGame()
        {
            Initialize();
            System.GC.Collect();
        }
    }

    public enum GameMode {
        AUTO,
        MANUAL
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

    public enum GameError {
        NO_ERROR,
        NO_SERVER_DETECTED
    }
}
