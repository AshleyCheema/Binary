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
            return;
        }
        transform.Translate(InputManager.Joystick(player) * movementSpeed * Time.deltaTime);
    }
}
