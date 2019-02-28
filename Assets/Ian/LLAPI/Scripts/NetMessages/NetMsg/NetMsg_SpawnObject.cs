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

            ObjectsToSpawn = new List<int>();
            ObjectsConnectionIds = new List<int>();
        }

        public List<int> ObjectsToSpawn { get; set; }
        public List<int> ObjectsConnectionIds { get; set; }
    }
}