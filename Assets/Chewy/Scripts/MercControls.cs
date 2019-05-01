/*
 * Author: Ashley Cheema
 * Description: Player controller to controll any player. 
 * This includes basic movements features.
 * Created: 04/02/2019
 * Edited By: Ash + Ian
 */

using LLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercControls : PlayerController
{
    private float cooldown;
    private float speedDuration;
    public Image flashPanel;
    private Color panelColor = new Color(255, 255, 255);
    public bool canSprint;
    private bool buttonPressed;
    private TrackerAbility trackerAbility;
    public bool noShoot;
    private float shotCooldown = 1.5f;
    private float reloadSpeed = 2f;
    private GameObject bullet;
    public Trigger triggerScript;
    public GameObject firePosition;
    public GameObject laser;

    public bool isStunned = false;
    public bool IsStunned
    { get { return isStunned; } set { isStunned = value; } }
    private float stunCountDown = 5.0f;

    //Audio
    private AudioSource audioSource;
    public AudioSO walkingSound;
    public AudioSO fireSound;
    public AudioSO burstRunSound;
    public AudioSO /*Thats How You Get*/ tinnitus;

    public Abilities sprint;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        trackerAbility = GetComponent<TrackerAbility>();
        audioSource = gameObject.GetComponent<AudioSource>();
        flashPanel.canvasRenderer.SetAlpha(1.0f);
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

        //if (triggerScript != null)
        //{
        if (triggerScript == null)
        {
            triggerScript = GameObject.Find("StunG").GetComponent<Trigger>();
        }

        animator.SetBool("isBurst", buttonPressed);
        animator.SetBool("isBlinded", triggerScript.isStunned);
        if (triggerScript.isStunned)
        {
            currentSpeed = reloadSpeed;
            canSprint = false;
            tinnitus.SetSourceProperties(audioSource);
            audioSource.Play();
            stunCountDown -= Time.deltaTime;
            flashPanel.gameObject.SetActive(true);
            flashPanel.CrossFadeAlpha(0.0f, stunCountDown, false);
            if (stunCountDown <= 0)
            {
                stunCountDown = 5.0f;
                currentSpeed = normalSpeed;
                triggerScript.isStunned = false;
                canSprint = true;
                flashPanel.gameObject.SetActive(false);
            }
        }

        //}
        if (Input.GetKeyDown(KeyCode.Mouse0) && !noShoot && !trackerAbility.trackerActive && !buttonPressed)
        {
            //Sound?
            if (bullet != null)
            {
                bullet.transform.position = firePosition.transform.position;//transform.GetChild(0).transform.position;//new Vector3(transform.position.x, transform.position.y, transform.position.z);
               // bullet.transform.position += transform.forward * 2.5f;//transform.GetChild(0).transform.forward * 2.5f;
                bullet.transform.rotation = firePosition.transform.rotation;//transform.GetChild(0).transform.rotation;
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
                bullet.SetActive(true);
            }
            noShoot = true;

            if (fireSound != null)
            {
                fireSound.SetSourceProperties(audioSource);
                audioSource.Play();
            }

            #region NetMsg_Fire
            Msg_AB_ClientFire ab_Fire = new Msg_AB_ClientFire();
            if (ClientManager.Instance != null)
            {
                ab_Fire.ConnectId = ClientManager.Instance.LocalPlayer.connectionId;

                ab_Fire.BulletPosition = bullet.transform.position;

                ab_Fire.BulletVelocity = bullet.GetComponent<Rigidbody>().velocity;

                ab_Fire.BulletObjectIndex = 2;
                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_FIRE, ab_Fire);
            }
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
                shotCooldown = 1.5f;
                currentSpeed = normalSpeed;
                noShoot = false;
            }
        }


        //If sprint has been used up then increase cooldown until it's back to it's original time
        if(!canSprint && !buttonPressed)
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
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            buttonPressed = true;
            laser.SetActive(false);
           #region NetMsg_Sprint
           NetMsg_AB_Sprint ab_Sprint = new NetMsg_AB_Sprint();
           if (ClientManager.Instance != null)
           {
               //ab_Sprint.ConnectionID = client.ServerConnectionId;
               ab_Sprint.SprintValue = runningSpeed;
               ClientManager.Instance.Send(ab_Sprint);
           }
           #endregion
        }
        if (buttonPressed)
        {
            currentSpeed = runningSpeed;
            speedDuration -= 0.01f;

            if (speedDuration <= 0)
            {
                laser.SetActive(true);
                cooldown = speedDuration;
                canSprint = false;
                buttonPressed = false;
                currentSpeed = normalSpeed;
            }
        }
    }

    private void Step()
    {
        if (!audioSource.isPlaying)
        {
            walkingSound.SetSourceProperties(audioSource);
            audioSource.Play();
        }
    }

    private void Run()
    {
        if (!audioSource.isPlaying)
        {
            burstRunSound.SetSourceProperties(audioSource);
            audioSource.Play();
        }
    }
}
