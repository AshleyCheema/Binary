using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //player team change message
    [System.Serializable]
    public class NetMsg_TeamChangeLB : NetMsg
    {
        public NetMsg_TeamChangeLB()
        {
            OP = NetOP.TEAM_CHANGE_LB;
        }

        public int ConnectionID { get; set; }
        public Team Team { get; set; }
    }
}
