using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientTrigger : MessageBase
{
    public int ConnectionID;
    public bool Trigger;
    public TriggerType Type;
}
