using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientAnimChange : MessageBase
{
    public byte connectId;
    public int hash;
    public byte direction;
}
