using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_StringMessage : NetMsg
    {
        public NetMsg_StringMessage()
        {
            OP = NetOP.NAME;
        }

        public string Message { get; set; }
        public long Time { get; set; }
    }
}