using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientState : MessageBase
{
    public int connectId;
    public SpyState state;
}
