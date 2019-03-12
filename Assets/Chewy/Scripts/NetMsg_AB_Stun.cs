using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_AB_Stun : NetMsg
    {
        public NetMsg_AB_Stun()
        {
            OP = NetOP.AB_STUN;
        }

        public int ConnectionID { get; set; }
        public int StunObjectIndex { get; set; }
        public bool Stunned { get; set; }
        public int AffectedID { get; set; }
    }
}
