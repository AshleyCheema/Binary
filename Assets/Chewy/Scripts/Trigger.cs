using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (stunAbility.stunActive)
                {
                    isStunned = true;
                    Debug.Log("Flashed");
                }
            }
        }
    }
}
