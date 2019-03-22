using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientCapaturePoint : MessageBase
{
    public int connectId;
    public bool IsBeingCaptured;
    public int ID;
}
