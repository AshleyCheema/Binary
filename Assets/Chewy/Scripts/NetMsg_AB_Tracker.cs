using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_AB_Tracker : NetMsg
    {
        public NetMsg_AB_Tracker()
        {
            OP = NetOP.AB_TRACKER;
        }

        public int ConnectionID { get; set; }
        public Vector3 TrackerPosition { get; set; }
        public GameObject TrackerObject { get; set; }
        public bool TrackerTriggered { get; set; }
    }
}

