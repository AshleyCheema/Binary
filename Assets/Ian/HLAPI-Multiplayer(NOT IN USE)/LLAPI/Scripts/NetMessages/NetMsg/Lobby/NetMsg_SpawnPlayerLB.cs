using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //spawn player message
    [System.Serializable]
    public class NetMsg_SpawnPlayerLB : NetMsg
    {
        public NetMsg_SpawnPlayerLB()
        {
            OP = NetOP.SPAWN_PLAYER_LB;
        }

        public int ConnectionID;
        public string PlayerName;
        public Team Team;
    }
}
