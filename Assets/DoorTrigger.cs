using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
