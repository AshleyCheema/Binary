﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public enum TriggerType
{
    Tracker,
    Bullet,
    Stun
}

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private TriggerType triggerType = TriggerType.Tracker;
    public bool hasShot;
    public bool isDetected;
    public bool isStunned;
    private StunAbility stunAbility;
    private MercControls mercControls;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spy")
        {
            if (triggerType == TriggerType.Tracker)
            {
                isDetected = true;
                Debug.Log("SPY DETECTED");
            }
            else if(triggerType == TriggerType.Bullet)
            {
                hasShot = true;
                //Send message to client which has been hit
                //first get the client which has been affected
                foreach (var key in Client.Instance.Players.Keys)
                {
                    //if true we have found the gameObejct hit
                    if(other.gameObject == Client.Instance.Players[key].avater)
                    {
                        //Create the new message to send to the client who was shot
                        NetMsg_AB_Trigger trigger = new NetMsg_AB_Trigger();
                        trigger.ConnectionID = key;
                        trigger.Trigger = true;
                        trigger.Type = LLAPI.TriggerType.BULLET;

                        //Send the message to the affected client
                        Client.Instance.Send(trigger);
                    }
                }
                Debug.Log("Shot");
            }
            else if(triggerType == TriggerType.Tracker)
            {
                //tell merc that the tracker has been set off
                //spy tracked
                //send
            }
        }

        //if(parent != null)
        //{
        //    parent.Callback();
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Merc")
        {
            if(triggerType == TriggerType.Stun)
            {
                if (stunAbility != null && stunAbility.stunActive)
                {
                    isStunned = true;
                    Debug.Log("Flashed");
                }
            }
        }
    }
}
