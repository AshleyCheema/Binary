/*
 * Author: Ashley Cheema
 * Description: Player controller to controll any player. 
 * This includes basic movements features.
 * Created: 04/02/2019
 * Edited By:
 */
using LLAPI;
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
    [SerializeField]
    private Trigger triggerScript;
    //Audio
    private AudioSource source;
    public AudioSO walkingSound;
    public AudioSO fireSound;
    public AudioSO burstRunSound;

    public Abilities sprint;
    private NetMsg_AB_Fire ab_Fire = new NetMsg_AB_Fire();

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        source = gameObject.GetComponent<AudioSource>();
        cooldown = sprint.cooldown;
        canSprint = sprint.isCooldown;
        speedDuration = sprint.abilityDuration;
        bullet = GameObject.Find("Bullet");
        if (bullet != null)
        {
            bullet.SetActive(false);
        }
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

        //ab_Fire.Trigger = triggerScript.hasShot;

        if (triggerScript != null)
        {
            if (triggerScript.isStunned)
            {
                currentSpeed = reloadSpeed;
                reloadSpeed -= Time.deltaTime;
                if (reloadSpeed <= 0)
                {
                    currentSpeed = normalSpeed;
                    triggerScript.isStunned = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !noShoot)
        {
            //Sound/Animation?
            if (bullet != null)
            {
                bullet.SetActive(true);
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * 100;
            }
            //walkingSound.SetSourceProperties(source);
            noShoot = true;
            if (fireSound != null)
            {
                fireSound.SetSourceProperties(source);
            }

            #region NetMsg_Fire
            ab_Fire.ConnectionID = client.ServerConnectionId;
            ab_Fire.BulletPosition = bullet.transform.position;
            ab_Fire.Velocity = bullet.GetComponent<Rigidbody>().velocity;
            ab_Fire.BulletObjectIndex = 2;
            client.Send(ab_Fire);
            #endregion

        }
        if(noShoot)
        {
            shotCooldown -= Time.deltaTime;
            //Reload Animation
            currentSpeed = reloadSpeed;
            if (shotCooldown <= 0)
            {
                if (bullet != null)
                {
                    bullet.SetActive(false);
                }
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
            #region NetMsg_Sprint
            NetMsg_AB_Sprint ab_Sprint = new NetMsg_AB_Sprint();
            ab_Sprint.ConnectionID = client.ServerConnectionId;
            ab_Sprint.SprintValue = runningSpeed;
            client.Send(ab_Sprint);
            #endregion
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
