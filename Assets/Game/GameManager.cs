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

            state = GameState.Initiated;
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

        public void StartGame()
        {
            state = GameState.Idle;
        }

        public void EndGame()
        {
            state = GameState.Ended;
        }
    }

    public enum GameState {
        Idle,
        Initiated,
        Progressing,
        Ended
    }
}
