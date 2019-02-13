/*
 * Author: Ian Hudson
 * Description: Player controller to controll any player. 
 * This includes basic movements features.
 * Created: 04/02/2019
 * Edited By: Ian + Ash
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float normalSpeed = 5f;
    private float crouchingSpeed = 2.5f;
    private float runningSpeed = 6f;

    [SerializeField]
    Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Crouching"))
        {
            transform.Translate(InputManager.Joystick(player) * crouchingSpeed * Time.deltaTime);
            Debug.Log("Left Control Held Down");
        }

        else if (Input.GetButton("Sprint"))
        {
            transform.Translate(InputManager.Joystick(player) * runningSpeed * Time.deltaTime);
            Debug.Log("Left Shift Held Down");
        }
        else
        {
            transform.Translate(InputManager.Joystick(player) * normalSpeed * Time.deltaTime);
        }

        UpdateDirection();
    }

    /// <summary>
    /// Update the directio that the character is facing,
    /// This locks the character not to rotate on the x plane
    /// </summary>
    private void UpdateDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 newDirection = transform.position - hit.point;
            newDirection.y = 0;
            transform.forward = newDirection;
            transform.right = Vector3.Cross(transform.forward, transform.up);
        }
    }
}