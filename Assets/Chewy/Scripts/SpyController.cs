using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpyState
{
    Normal,
    Hurt,
    Dead
}

public class SpyController : PlayerController
{
    public bool isHacking;
    public GameObject stun;
    private Trigger bulletTrigger;
    public CooldownScript cooldownScript;
    private SpyState currentState;
    public SpyState CurrentState
    { get { return currentState; } set { currentState = value; } }

    public bool stunDrop;
    public bool isSpawned = false;

    //animation last state
    private int animLastState = -1;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (stun == null)
        {
            stun = GameObject.Find("StunG");
        }
        currentState = SpyState.Normal;
    }

    // Update is called once per frame
    public override void Update()
    {
        //Sets the animation to reflect to the Spy's current state
        animator.SetInteger("currentState", (int)currentState);

        //Networking for the Spy animations
        if(animLastState != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
        {
            animLastState = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

            //send animation message
            Debug.Log("Animation state changed");

            Msg_ClientAnimChange cac = new Msg_ClientAnimChange();
            cac.hash = animLastState;
            cac.connectId = (byte)ClientManager.Instance?.LocalPlayer.connectionId;
            cac.direction = (byte)(animator.GetFloat("InputX+") + 2);
            ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_ANIM_CHANGE, cac);
        }

        if (currentState == SpyState.Dead)
        {
            return;
        }
        base.Update();

        //The spy cannot use their flashbang while hacking
        if (Input.GetButton("Flashbang") && !isHacking)
        {
            if (stun.GetComponent<StunAbility>().IsActive == false)
            {
                stun.GetComponent<StunAbility>().IsActive = true;
                stunDrop = true;
            }
        }

        //If hacking play audio
        if (isHacking == true)
        {

            //if (!audioSource.isPlaying)
            //{
            //    hacking.SetSourceProperties(audioSource);
            //    audioSource.Play();
            //}
            GetComponent<AudioEvents>().PlayHackingSound();
        }
        else
        {
            GetComponent<AudioEvents>().StopHackingSound();
            //audioSource.Stop();
        }
    }

    //The function is called when the player is shot by the Merc
    //If the shot once go into a hurt state otherwise the Spy is now dead
    public void Shot()
    {
        if (currentState == SpyState.Normal)
        {
            //Change animation
            //Maybe drip blood?
            Debug.Log("Hurt State");
            currentState = SpyState.Hurt;
        }
        else if (currentState == SpyState.Hurt)
        {
            //bulletTrigger.hasShot = false;
            currentState = SpyState.Dead;
            Debug.Log("Dead");

            //Send message to host
            //spy is daed
            Msg_ClientState cs = new Msg_ClientState();
            cs.connectId = (int)ClientManager.Instance?.LocalPlayer.connectionId;
            cs.state = currentState;
            ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_STATE, cs);
        }
    }

    //private void Step()
    //{
    //    if (audioSource != null && !audioSource.isPlaying)
    //    {
    //        walking.SetSourceProperties(audioSource);
    //        audioSource.Play();
    //        PlayerStats.Instance.Steps += 1;
    //    }
    //}
    
    //private void Run()
    //{
    //    if (audioSource != null && !audioSource.isPlaying)
    //    {
    //        run.SetSourceProperties(audioSource);
    //        audioSource.Play();
    //        PlayerStats.Instance.Steps += 1;
    //    }
    //}
}
