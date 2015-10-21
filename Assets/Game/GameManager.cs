using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Assets.Game.Communication;
using Assets.Game.GameEntities;

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

        private bool gameStarted;
        public bool GameStarted
        {
            get
            {
                return gameStarted;
            }
        }

        private bool gameEnded;
        public bool GameEnded
        {
            get
            {
                return gameEnded;
            }
        }

        //clock pulse is generated for moving bullets and reducing the time left for collectibles
        private Timer clockPulseGenerator;
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

            //Instatiating the timer and connecting the timer to the method which hadles the moving of bullets and timers on coin piles and life packs
            clockPulseGenerator = new Timer();
            clockPulseGenerator.Interval = 1000;
            clockPulseGenerator.Elapsed += gameEngine.Clock;

            gameStarted = false;
            gameEnded = false;
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
            clockPulseGenerator.Start();
            gameStarted = true;
        }

        public void EndGame()
        {
            clockPulseGenerator.Stop();
            gameStarted = false;
            gameEnded = true;
        }
    }
}
