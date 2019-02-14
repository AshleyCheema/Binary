using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    Tracker,
    Bullet
}

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private TriggerType triggerType = TriggerType.Tracker;
    public bool hasShot;
    public bool isDetected;

    private MercControls mercControls;

    private void Start()
    {
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

            if(triggerType  == TriggerType.Bullet)
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
}
