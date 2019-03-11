/*
 * Author: Ian Hudson
 * Description: Stun object. This sctipt is attached to the stun object. This will count down and keep
 * track of all the characters hit.
 * Created: 11/03/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public class StunObject : MonoBehaviour
{
    [SerializeField]
    private float timeRemaining = 5f;

    [SerializeField]
    private float countDownAmount = 1.0f;

    [SerializeField]
    private ParticleSystem particleSystem;

    private bool goneOff = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!goneOff && timeRemaining <= 0.0f)
        {
            //the stun has gone off
            goneOff = true;

            //Start particle system
            particleSystem.Play();

            //check who is affected

            Collider[] coll = null;
            Physics.OverlapSphereNonAlloc(transform.position, GetComponent<SphereCollider>().radius, coll);
            if (coll.Length > 0)
            {
                foreach (var pKey in Server.Instance.Players.Keys)
                {
                    for (int i = 0; i < coll.Length; i++)
                    {
                        if(Server.Instance.Players[pKey].avater == coll[i].gameObject)
                        {
                            //we have hit
                            //this is an effected gameobject

                            //Create a trigger network message
                            NetMsg_AB_Trigger trigger = new NetMsg_AB_Trigger();
                            trigger.ConnectionID = Server.Instance.Players[pKey].connectionId;
                            trigger.Trigger = true;
                            trigger.Type = LLAPI.TriggerType.STUN;

                            //Send message to client affected
                            Server.Instance.Send(trigger, Server.Instance.ReliableChannel, trigger.ConnectionID);
                        }
                    }
                }
            }
            //remove
        }
        else if(goneOff)
        {
            //when the particle system has stoped
            if(!particleSystem.IsAlive())
            {
                //clean up the particle system from the server/client
            }
        }
        else
        {
            //count down
            timeRemaining -= countDownAmount * Time.deltaTime;
        }
    }
}
