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
    public bool IsSpawned = false;

    private void Start()
    {
        stunAbility = GetComponent<StunAbility>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsSpawned)
        {
            if (other.gameObject.tag == "Spy")
            {
                if (triggerType == TriggerType.Tracker)
                {
                    isDetected = true;
                    Debug.Log("Spy detected: " + gameObject.transform.position);

                    //tell merc that the tracker has been set off
                    //spy tracked
                }
                else if (triggerType == TriggerType.Bullet)
                {
                    hasShot = true;
                    //Send message to client which has been hit
                    //first get the client which has been affected
                    foreach (var key in ClientManager.Instance?.Players.Keys)
                    {
                        //if true we have found the gameObejct hit
                        if (other.gameObject == ClientManager.Instance.Players[key].gameAvatar)
                        {
                            Msg_ClientTrigger ct = new Msg_ClientTrigger();
                            ct.ConnectionID = key;
                            ct.Trigger = true;
                            ct.Type = TriggerType.Bullet;
                            ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_TRIGGER, ct);

                            //Create the new message to send to the client who was shot
                            //NetMsg_AB_Trigger trigger = new NetMsg_AB_Trigger();
                            //trigger.ConnectionID = key;
                            //trigger.Trigger = true;
                            //trigger.Type = LLAPI.TriggerType.BULLET;
                            //
                            ////Send the message to the affected client
                            //Client.Instance.Send(trigger);
                        }
                    }
                    Debug.Log("Shot");
                }
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
            if (triggerType == TriggerType.Stun)
            {
                if (stunAbility.stunActive)
                {
                    isStunned = true;
                    Debug.Log("Flashed");
                }
            }
        }
        if(GameObject.FindGameObjectWithTag("Spy"))
        {

        }
    }
}
