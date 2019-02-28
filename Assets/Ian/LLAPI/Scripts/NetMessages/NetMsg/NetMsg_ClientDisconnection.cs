using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_ClientDisconnection : NetMsg
    {
        public NetMsg_ClientDisconnection()
        {
            OP = NetOP.DISCONNECTION;
        }

        public int ConnectionID { get; set; }
    }
}
