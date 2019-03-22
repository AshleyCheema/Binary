using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Msg_AB_ClientFire : MessageBase
{
    public int ConnectId;

    public Vector3 BulletPosition;

    public Vector3 BulletVelocity;

    public int BulletObjectIndex;
}
