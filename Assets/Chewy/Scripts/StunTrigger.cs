using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Merc")
        {
            //other.transform.Translate(InputManager.Joystick(Player.PlayerTwo))
            Debug.Log("Get Flashed");
        }
    }

}
