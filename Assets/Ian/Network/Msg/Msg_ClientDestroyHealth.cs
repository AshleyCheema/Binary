using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_ClientDestroyHealth : MessageBase
{
    public int ConnectID;
    public int ID;
}
