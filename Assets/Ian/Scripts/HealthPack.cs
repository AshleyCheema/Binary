using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //check if the spy enters a health pack trigger
        if (other.tag == "Spy")
        {
            Debug.Log("Health pack picked up");
            //if this spy is hurt then change it's state to
            //normal
            if (other.GetComponent<SpyController>().CurrentState == SpyState.Hurt)
            {
                other.GetComponent<SpyController>().CurrentState = SpyState.Normal;
            }
        }
    }
}
