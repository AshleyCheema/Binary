using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    //public Abilities parent;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Spy")
        {
            Debug.Log("SPY DETECTED");
        }

        //if(parent != null)
        //{
        //    parent.Callback();
        //}
    }
}
