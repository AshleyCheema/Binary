﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_SendServerConnectionID : NetMsg
    {
        public NetMsg_SendServerConnectionID()
        {
            OP = NetOP.SENDCONNECTIONID;
        }

        public int ConnectionId { set; get; }
    }
}
