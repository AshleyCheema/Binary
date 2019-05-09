using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_IsReadyLB : NetMsg
    {
        public NetMsg_IsReadyLB()
        {
            OP = NetOP.IS_READY_LB;
        }

        public int ConnectionID { get; set; }
        public bool IsReady { get; set; }
    }
}
