using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_NetworkObject : NetMsg
    {
        public NetMsg_NetworkObject()
        {
            OP = NetOP.NETWORK_OBJECT;
        }

        public int ID { set; get; }
    }
}
