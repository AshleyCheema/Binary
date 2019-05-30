using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientCaptureStats : MessageBase
{
    public int ID;
    public int CapturePercentage;
}
