using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_SpawnObject : NetMsg
    {
        public NetMsg_SpawnObject()
        {
            OP = NetOP.SPAWNOBJECT;

            ObjectsToSpawn = new List<SpawnableObject>();
        }
        public List<SpawnableObject> ObjectsToSpawn { get; set; }
    }

    [System.Serializable]
    public struct SpawnableObject
    {
        public int ConnectionID { get; set; }
        public int ObjectID { get; set; }

        public float XPos { get; set; }
        public float YPos { get; set; }
        public float ZPos { get; set; }

        public float XRot { get; set; }
        public float YRot { get; set; }
        public float ZRot { get; set; }

    }
}