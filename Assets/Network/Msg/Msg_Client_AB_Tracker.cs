using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_Client_AB_Tracker : MessageBase
{
    public int ConnectionID;

    public Vector3 TrackerPosition;

    public int TrackerObjectIndex;
    public bool TrackerTriggered;
}
