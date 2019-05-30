using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //sprint ability message
    [System.Serializable]
    public class NetMsg_AB_Sprint : NetMsg
    {
        public NetMsg_AB_Sprint()
        {
            OP = NetOP.AB_SPRINT;
        }

        public int ConnectionID { get; set; }
        public float SprintValue { get; set; }
    }
}
