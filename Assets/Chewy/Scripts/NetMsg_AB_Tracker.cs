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

        public float TrackerPositionX { get; set; }
        public float TrackerPositionY { get; set; }
        public float TrackerPositionZ { get; set; }

        public int TrackerObjectIndex { get; set; }
        public bool TrackerTriggered { get; set; }
    }
}

