using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //player name change message
    [System.Serializable]
    public class NetMsg_NameChangeLB : NetMsg
    {
        public NetMsg_NameChangeLB()
        {
            OP = NetOP.NAME_CHANGE_LB;
        }

        public int ConnectionID { get; set; }
        public string NewName { get; set; }
    }
}
