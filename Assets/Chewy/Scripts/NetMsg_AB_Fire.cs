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

        public float BulletPositionX { get; set; }
        public float BulletPositionY { get; set; }
        public float BulletPositionZ { get; set; }

        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float VelocityZ { get; set; }

        public int BulletObjectIndex { get; set; }
        //public bool Trigger { get; set; }
    }

}
