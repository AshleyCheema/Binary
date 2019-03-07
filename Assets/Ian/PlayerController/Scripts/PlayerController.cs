/*
 * Author: Ian Hudson
 * Description: Player controller to controll any player. 
 * This includes basic movements features.
 * Created: 04/02/2019
 * Edited By: Ian + Ash
 */

using LLAPI;
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

    [SerializeField]
    protected Rigidbody rb;

    protected Client client;

    private Vector3 velocity;

    // Start is called before the first frame update
    public virtual void Start()
    {
        client = FindObjectOfType<Client>();
        rb = GetComponent<Rigidbody>();
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

        velocity = InputManager.Joystick(player);
        //transform.Translate(InputManager.Joystick(player) * currentSpeed * Time.deltaTime);


        UpdateDirection();

        //Check if the velocity is not zero
        if (velocity != Vector3.zero)
        {
            //Update server setting for this object
            NetMsg_PlayerMovement playerMovement = new NetMsg_PlayerMovement();
            playerMovement.connectId = client.ServerConnectionId;
            playerMovement.xMove = rb.position.x;
            playerMovement.yMove = rb.position.y;
            playerMovement.zMove = rb.position.z;

            client.Send(playerMovement, client.StateUpdateChannel);
        }
    }

    private void FixedUpdate()
    {
        //Update the position on the rigidbody
        //this keeps our position in line with collisions/physics
        if (rb != null)
        {
            rb.MovePosition(rb.position + velocity * currentSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Update the directio that the character is facing,
    /// This locks the character not to rotate on the x plane
    /// </summary>
    public void UpdateDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
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