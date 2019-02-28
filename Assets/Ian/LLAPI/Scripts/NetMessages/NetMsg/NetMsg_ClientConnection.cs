using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_ClientConnection : NetMsg
    {
        public NetMsg_ClientConnection()
        {
            OP = NetOP.CONNECTION;
        }

        public string PlayerName { get; set; }
        public int ConnectionId { get; set; }
    }
}