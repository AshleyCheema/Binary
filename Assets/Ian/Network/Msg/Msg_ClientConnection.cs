using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientConnection : MessageBase
{
    public int connectID;
    public string Name;
    public LLAPI.Team Team;
}
