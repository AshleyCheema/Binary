﻿/*
 * Author: Ian Hudson
 * Description: Player controller to controll any player. This is used for Networking 
 * This includes basic movements features.
 * Created: 06/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_Net : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    [SyncVar]
    private GameObject playerObject;
    public GameObject PlayerObject
    { get { return playerObject; } set { playerObject = value; } }

    [SerializeField]
    Player player;

    Vector3 velocity;

    /// <summary>
    /// Position we think is most correct for this player.
    /// If we are the authority, then this will be transform.position
    /// </summary>
    Vector3 guessPosition;

    /// <summary>
    /// Objects latency's to the server
    /// </summary>
    float ourLatency;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasAuthority)
        {
            GetComponentInChildren<Camera>().enabled = false;
            return;
        }

        CmdUpdateVelocity(velocity, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority == false)
        {
            guessPosition = guessPosition + (velocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, guessPosition, Time.deltaTime * 10);

            GetComponentInChildren<Camera>().enabled = false;

            return;
        }

        GetComponentInChildren<Camera>().enabled = true;

        transform.Translate(velocity * Time.deltaTime);

        transform.Translate(InputManager.Joystick(player) * movementSpeed * Time.deltaTime);

        if(InputManager.Joystick(player) != Vector3.zero)
        {
            // The player is asking the change it's direction/speed (velocity)
            velocity = InputManager.Joystick(player) * movementSpeed * Time.deltaTime;

            CmdUpdateVelocity(velocity, transform.position);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (hasAuthority)
    //    {
    //        Debug.LogWarning("Entered trigger");
    //        PlayerObject_Net.SetAuthority(other.gameObject, PlayerObject);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(hasAuthority)
    //    {
    //        Debug.LogWarning("Exited trigger");
    //        PlayerObject_Net.RevokeAuthority(other.gameObject, PlayerObject);
    //    }
    //}

    /// <summary>
    /// Server. Update velocity on server side
    /// </summary>
    /// <param name="a_velocity"></param>
    /// <param name="a_position"></param>
    [Command]
    private void CmdUpdateVelocity(Vector3 a_velocity, Vector3 a_position)
    {
        velocity = a_velocity;
        transform.position = a_position;

        RpcUpdateVelocity(velocity, transform.position);
    }

    /// <summary>
    /// Client. Update velocity on all clients
    /// </summary>
    /// <param name="a_velocity"></param>
    /// <param name="a_position"></param>
    [ClientRpc]
    private void RpcUpdateVelocity(Vector3 a_velocity, Vector3 a_position)
    {
        if (hasAuthority)
        {
            return;
        }
            velocity = a_velocity;
            guessPosition = a_position + (velocity * (0));
            //transform.position = a_position;

            //If we know our latency we could try this:
            // transform.position = p + (v * (ourLatency + theirLatency))
    }

    /// <summary>
    /// Client. Set all PlayerObjects on clients
    /// </summary>
    /// <param name="a_go"></param>
    [Command]
    public void CmdSetPlayerObject(GameObject a_go)
    {
        playerObject = a_go;
    }

    public bool ObjectHasAuthority()
    {
        return hasAuthority;
    }

    public NetworkConnection SetAuthoirty(NetworkIdentity a_net)
    {
        a_net.AssignClientAuthority(PlayerObject.GetComponent<PlayerController_Net>().connectionToClient);

        return PlayerObject.GetComponent<PlayerController_Net>().connectionToClient;
    }
}
