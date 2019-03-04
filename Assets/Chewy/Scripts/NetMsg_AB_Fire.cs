using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_AB_Fire : NetMsg
    {
        public NetMsg_AB_Fire()
        {
            OP = NetOP.AB_FIRE;
        }

        public int ConnectionID { get; set; }
        public Vector3 BulletPosition { get; set; }
        public Vector3 Velocity { get; set; }
        public GameObject BulletObject { get; set; }
        public bool Trigger { get; set; }
    }

}
