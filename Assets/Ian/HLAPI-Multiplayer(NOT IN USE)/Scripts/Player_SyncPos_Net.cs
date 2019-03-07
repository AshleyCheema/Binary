/*
 * Author: Ian Hudson
 * Description: Sync the players position using a SyncVar
 * Created: 06/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_SyncPos_Net : NetworkBehaviour
{
    //Position which will be synced
    [SyncVar]
    private Vector3 syncPos;

    //Object's transform
    [SerializeField]
    private Transform myTransform;

    //Amount which the position should be lerped by
    [SerializeField]
    private float lerpRate = 15;

    /// <summary>
    /// Update on a fixed time step
    /// </summary>
    private void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    /// <summary>
    /// Lerp the position
    /// </summary>
    private void LerpPosition()
    {
        if(!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    /// <summary>
    /// Provide the server with this object's position
    /// </summary>
    /// <param name="a_pos"></param>
    [Command]
    void CmdProvidePositionToServer(Vector3 a_pos)
    {
        syncPos = a_pos;
    }

    /// <summary>
    /// Tell the server our position
    /// </summary>
    [ClientCallback]
    private void TransmitPosition()
    {
        //if we have authority then tell the server our position
        if (hasAuthority)
        {
            CmdProvidePositionToServer(myTransform.position);
        }
    }
}
