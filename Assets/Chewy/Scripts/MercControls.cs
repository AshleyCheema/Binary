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

public class MercControls : PlayerController
{

    private float cooldown;
    private float speedDuration;
    private bool canSprint;
    private bool buttonPressed;

    private bool noShoot;
    private float shotCooldown = 5f;
    public float reloadSpeed = 2f;
    private GameObject bullet;
    private Rigidbody rb;
    [SerializeField]
    private Trigger triggerScript;

    public Abilities sprint;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = sprint.cooldown;
        canSprint = sprint.isCooldown;
        speedDuration = sprint.abilityDuration;
        bullet = GameObject.Find("Bullet");
        rb = bullet.GetComponent<Rigidbody>();
        bullet.SetActive(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        //UpdateDirection();
        base.Update();
        //if(Input.GetKey(KeyCode.P))
        //{
        //    sprint.Trigger();
        //}

        if(triggerScript.isStunned)
        {
            currentSpeed = reloadSpeed;
            reloadSpeed -= Time.deltaTime;
            if(reloadSpeed <= 0)
            {
                currentSpeed = normalSpeed;
                triggerScript.isStunned = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !noShoot)
        {
            //Sound/Animation?
            bullet.SetActive(true);
            bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            rb.velocity = transform.forward * 100;
            noShoot = true;
        }
        if(noShoot)
        {
            shotCooldown -= Time.deltaTime;
            //Reload Animation
            currentSpeed = reloadSpeed;
            if (shotCooldown <= 0)
            {
                bullet.SetActive(false);
                shotCooldown = 5f;
                noShoot = false;
            }
        }


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
            currentSpeed = runningSpeed;
            speedDuration -= 0.01f;

            if (speedDuration <= 0)
            {
                cooldown = speedDuration;
                canSprint = false;
                buttonPressed = false;

                currentSpeed = normalSpeed;
            }
        }
    }
}
