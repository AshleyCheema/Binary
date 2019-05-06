using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAnimationController : MonoBehaviour
{
    Animator animatorController;

    Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if the current poisiton in not equal to lastPosition
        //then the position has changed
        //if(transform.position != lastPosition)
        //{
        //    Vector3 dir = 
        //}
        //float direction
        Debug.Log(GetComponent<Rigidbody>().velocity);
    }
}
