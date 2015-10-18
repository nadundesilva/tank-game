using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.Communication
{
    public class ClientConnection
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public EventHandler MessageReceived { get; set; }

        public ClientConnection()
        {
            ServerIP = "127.0.0.1";
            ServerPort = 6000;
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
