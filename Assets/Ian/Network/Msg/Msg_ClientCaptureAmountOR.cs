using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientCaptureAmountOR : MessageBase
{
    public int ConnectID;
    //override the capture point amounts
    public float BaseCaptureAmount;
    public float MaxCaptureAmount;
}
