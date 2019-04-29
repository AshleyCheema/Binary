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
    protected float normalSpeed = 5f;

    [SerializeField]
    protected float crouchingSpeed = 2.5f;

    [SerializeField]
    protected float runningSpeed = 6f;

    public bool isRunning;

    public float currentSpeed = 5f;

    public Animator animator;

    public bool isMoving;

    [SerializeField]
    public Player player;

    [SerializeField]
    protected Rigidbody rb;

    private Vector3 velocity;

    // Start is called before the first frame update
    public virtual void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Maybe add a client check if need

        if (player == Player.PlayerTwo)
        {
            if (Input.GetButton("Crouching"))
            {
                currentSpeed = crouchingSpeed;
            }

            else if (Input.GetButton("Sprint"))
            {
                isRunning = true;
                currentSpeed = runningSpeed;
            }
            else
            {
                isRunning = false;
                currentSpeed = normalSpeed;
            }
        }
        animator.SetBool("isRunning", isRunning);
        velocity = InputManager.MovementRelativeToCamera(InputManager.Joystick(player, animator));
        Quaternion oldRot = transform.rotation;

        UpdateDirection();

        //Check if the velocity is not zero
        if (velocity != Vector3.zero)
        {
            isMoving = true;
            animator.SetBool("isHacking", false);
            //Update server setting for this object
            Msg_ClientMove playerMovement = new Msg_ClientMove();
            if (ClientManager.Instance != null)
            {
                playerMovement.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                playerMovement.position = rb.position;
                ClientManager.Instance.client?.Send(MSGTYPE.CLIENT_MOVE, playerMovement);
            }
        }
        else
        {
            isMoving = false;
        }

        //Check if the rotation has changed
        if(oldRot != transform.rotation)
        {
            //Update server seting for this object
            Msg_ClientRotation playerRotation = new Msg_ClientRotation();
            if (ClientManager.Instance != null)
            {
                playerRotation.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                playerRotation.rot = transform.rotation;

                ClientManager.Instance.client?.Send(MSGTYPE.CLIENT_ROTATION, playerRotation);
            }
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
        RaycastHit[] hits = Physics.RaycastAll(ray,1000);

        for (int i = hits.Length - 1; i >= 0; --i)
        {
            if (hits[i].collider.gameObject.layer == 9)
            {
                Vector3 newDirection = transform.position - hits[i].point;
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
                break;
            }
        }
    }
}