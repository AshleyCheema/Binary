using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_AB_Trigger : NetMsg
    {
        public NetMsg_AB_Trigger()
        {
            OP = NetOP.AB_TRIGGER;
        }

        public int ConnectionID { get; set; }
        public bool Trigger { get; set; }
    }
}