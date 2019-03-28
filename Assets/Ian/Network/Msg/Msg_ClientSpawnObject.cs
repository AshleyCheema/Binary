using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientSpawnObject : MessageBase
{
    public int ConnectionID;
    public int ObjectID;

    public Vector3 position;
}
