/*
 * Author: Ashley Cheema
 * Description: Player controller to controll any player. 
 * This includes basic movements features.
 * Created: 04/02/2019
 * Edited By:
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercControls : MonoBehaviour
{
    [SerializeField]
    private float normalSpeed = 5f;
    private float runningSpeed = 8f;
    public float cooldown;
    private float speedDuration;
    private bool canSprint;
    private bool buttonPressed;

    public Abilities sprint;

    [SerializeField]
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = sprint.cooldown;
        canSprint = sprint.isCooldown;
        speedDuration = sprint.abilityDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //If sprint has been used up then increase cooldown until it's back to it's original time
        if(!canSprint)
        {
            cooldown += Time.deltaTime;
            if (cooldown >= sprint.cooldown)
            {
                speedDuration = sprint.abilityDuration;
                cooldown = sprint.cooldown;
                canSprint = true;
            }
        }

        //When sprint is pressed the merc will be able to run for a short period of time
        if (Input.GetButton("Sprint") && canSprint)
        {
            buttonPressed = true;
        }
        if (buttonPressed)
        {
            transform.Translate(InputManager.Joystick(player) * runningSpeed * Time.deltaTime);
            speedDuration -= 0.01f;

            if (speedDuration <= 0)
            {
                cooldown = speedDuration;
                canSprint = false;
                buttonPressed = false;
            }
        }
        else
        {
            transform.Translate(InputManager.Joystick(player) * normalSpeed * Time.deltaTime);
        }
 
    }
}
