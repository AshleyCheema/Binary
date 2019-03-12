using System.Collections;
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

    private void Start()
    {
        stunAbility = gameObject.GetComponent<StunAbility>();
        mercControls = gameObject.GetComponent<MercControls>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spy")
        {
            if (triggerType == TriggerType.Tracker)
            {
                isDetected = true;
                Debug.Log("SPY DETECTED");
            }

            if(triggerType == TriggerType.Bullet)
            {
                hasShot = true;
                //Send message to client which has been hit
                //first get the client which has been affected
                foreach (var key in Server.Instance.Players.Keys)
                {
                    //if true we have found the gameObejct hit
                    if(other.gameObject == Server.Instance.Players[key].avater)
                    {
                        //Create the new message to send to the client who was shot
                        NetMsg_AB_Trigger trigger = new NetMsg_AB_Trigger();
                        trigger.ConnectionID = key;
                        trigger.Trigger = true;
                        trigger.Type = LLAPI.TriggerType.BULLET;

                        //Send the message to the affected client
                        Server.Instance.Send(trigger, Server.Instance.ReliableChannel, key);
                    }
                }
                Debug.Log("Shot");
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
