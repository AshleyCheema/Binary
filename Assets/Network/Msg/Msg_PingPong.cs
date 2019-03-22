using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_PingPong : MessageBase
{
    public int connectId;
    public long timeStamp;
}
