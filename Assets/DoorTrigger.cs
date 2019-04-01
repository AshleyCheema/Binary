using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spy" || other.gameObject.tag == "Merc")
        {
            GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Spy" || other.gameObject.tag == "Merc")
        {
            GetComponent<Animator>().SetTrigger("CloseDoor");
        }
    }
}
