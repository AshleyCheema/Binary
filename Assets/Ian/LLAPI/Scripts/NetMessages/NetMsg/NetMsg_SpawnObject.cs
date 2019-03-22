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
    public class SpawnableObject
    {
        public int ConnectionID;
        public int ObjectID;

        public Vector3 position;
    }
}