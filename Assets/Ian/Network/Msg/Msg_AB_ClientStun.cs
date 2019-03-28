using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_AB_ClientStun : MessageBase
{
    public int ConnectionID;
    public int StunObjectIndex;
    public bool Stunned;
    public int AffectedID;
}
