using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientMove : MessageBase
{
    public int connectId;
    public Vector3 position;
    public int Time;
}
