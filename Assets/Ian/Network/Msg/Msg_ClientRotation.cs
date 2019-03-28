using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientRotation : MessageBase
{
    public int connectId;
    public Quaternion rot;
}
