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
        public GameObject StunObject { get; set; }
        public ParticleSystem StunParticle { get; set; }
        public bool Stunned { get; set; }
    }
}
