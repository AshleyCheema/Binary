/*
 * Author: Ian Hudson
 * Description: 
 * Created: 06/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncPos_Net : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    private Transform myTransform;

    [SerializeField]
    private float lerpRate = 15;

    private void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    private void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 a_pos)
    {
        syncPos = a_pos;
    }

    [ClientCallback]
    private void TransmitPosition()
    {
        if (hasAuthority)
        {
            CmdProvidePositionToServer(myTransform.position);
        }
    }
}
