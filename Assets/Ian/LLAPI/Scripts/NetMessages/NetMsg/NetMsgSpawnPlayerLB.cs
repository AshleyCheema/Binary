using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsgSpawnPlayerLB : NetMsg
    {
        public NetMsgSpawnPlayerLB()
        {
            OP = NetOP.SPAWN_PLAYER_LB;
        }

        public int ConnectionID;
        public string PlayerName;
        public Team Team;
    }
}
