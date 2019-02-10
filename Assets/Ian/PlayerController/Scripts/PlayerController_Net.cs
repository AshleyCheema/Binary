/*
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
        if (!isLocalPlayer)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority == false)
        {
            guessPosition = guessPosition + (velocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, guessPosition, Time.deltaTime * 10);

            return;
        }

        transform.Translate(velocity * Time.deltaTime);

        transform.Translate(InputManager.Joystick(player) * movementSpeed * Time.deltaTime);

        if(true)
        {
            // The player is asking the change it's direction/speed (velocity)
            velocity = InputManager.Joystick(player) * movementSpeed * Time.deltaTime;

            CmdUpdateVelocity(velocity, transform.position);
        }
    }

    [Command]
    void CmdUpdateVelocity(Vector3 a_velocity, Vector3 a_position)
    {
        velocity = a_velocity;
        transform.position = a_position;

        RpcUpdateVelocity(velocity, transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity(Vector3 a_velocity, Vector3 a_position)
    {
        if(hasAuthority)
        {
            return;
        }

        velocity = a_velocity;
        guessPosition = a_position + (velocity * (ourLatency));
        //transform.position = a_position;

        //If we know our latency we could try this:
        // transform.position = p + (v * (ourLatency + theirLatency))
    }
}
