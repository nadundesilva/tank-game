using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Game.Communication
{
    public class ClientConnection
    {
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
