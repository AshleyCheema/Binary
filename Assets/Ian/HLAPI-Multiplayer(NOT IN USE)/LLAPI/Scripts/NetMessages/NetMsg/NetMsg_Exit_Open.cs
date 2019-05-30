using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //exit open message
    [System.Serializable]
    public class NetMsg_Exit_Open : NetMsg_NetworkObject
    {
        public NetMsg_Exit_Open()
        {
            OP = NetOP.NETWORK_OBJECT;
        }

        public bool IsOpen { get; set; }
    }
}
