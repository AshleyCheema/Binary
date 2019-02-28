using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_PlayerMovement : NetMsg
    {
        public NetMsg_PlayerMovement()
        {
            OP = NetOP.PLAYERMOVEMENT;
        }

        public float xMove { set; get; }
        public float yMove { set; get; }
        public int connectId { set; get; }
    }
}
