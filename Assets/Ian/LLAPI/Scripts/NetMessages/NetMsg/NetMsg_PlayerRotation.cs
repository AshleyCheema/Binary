using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_PlayerRotation : NetMsg
    {
        public NetMsg_PlayerRotation()
        {
            OP = NetOP.ROTATION;
        }

        public int ConnectionId { get; set; }

        public float XRot { get; set; }
        public float YRot { get; set; }
        public float ZRot { get; set; }
    }
}
