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
        animatorController = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if the current poisiton in not equal to lastPosition
        //then the position has changed
        if (transform.position != lastPosition)
        {
            Vector3 dir = lastPosition - transform.position;
            dir.Normalize();
            lastPosition = transform.position;
            Debug.Log(dir);

            if (Mathf.Sign(dir.z) == -1)
            {
                animatorController.SetFloat("Direction", 1.0f);
            }
            else if (Mathf.Sign(dir.z) == 1)
            {
                animatorController.SetFloat("Direction", -1.0f);
            }
        }
        else
        {
            animatorController.SetFloat("Direction", 0.0f);
        }
    }
}
