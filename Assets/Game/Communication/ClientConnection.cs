using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.Communication
{
    public class ClientConnection
    {
        private string serverIP;
        public string ServerIP
        {
            get
            {
                return serverIP;
            }
        }

        private int serverPort;
        public int ServerPort
        {
            get
            {
                return serverPort;
            }
        }

        public EventHandler MessageReceived { get; set; }

        public ClientConnection()
        {
            serverIP = "127.0.0.1";
            serverPort = 6000;
        }



        private void FireConnectionChangedEvent(string message)
        {
            EventHandler handler = MessageReceived;
            if (handler != null)
            {
                MessageReceivedEventArgs eventArgs = new MessageReceivedEventArgs();
                eventArgs.Message = message;
                handler(this, eventArgs);
            }
        }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
