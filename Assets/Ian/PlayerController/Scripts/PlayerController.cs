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
    public float normalSpeed = 5f;
    private float crouchingSpeed = 2.5f;
    public float runningSpeed = 6f;

    public float currentSpeed = 5f;

    [SerializeField]
    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetButton("Crouching"))
        {
            currentSpeed = crouchingSpeed;
        }

        else if (Input.GetButton("Sprint"))
        {
            currentSpeed = runningSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        transform.Translate(InputManager.Joystick(player) * currentSpeed * Time.deltaTime);

        UpdateDirection();
    }

    /// <summary>
    /// Update the directio that the character is facing,
    /// This locks the character not to rotate on the x plane
    /// </summary>
    public void UpdateDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Merc")
            {
                Vector3 newDirection = transform.position - hit.point;
                newDirection.y = 0.5f;
                transform.forward = newDirection;
                transform.right = Vector3.Cross(transform.forward, transform.up);

                //Vector3 playerToMouse = hit.point - transform.position;
                //
                //playerToMouse.y = 0f;
                //
                //Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                //
                //transform.rotation = newRotation;
            }
        }
    }
}