using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientNameChange : MessageBase
{
    public int connectionID;
    public string name;
}
