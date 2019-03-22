using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientTeamChange : MessageBase
{
    public int ConnectionID;
    public LLAPI.Team Team;
}
