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

    //bool VectorWithinRange(Vector3 aVector, Vector3 aRangeVector, float aRange)
    //{
    //    if(Mathf.Abs(aVector.x - aRangeVector.x) < aRange &&
    //       Mathf.Abs(aVector.x - aRangeVector.x) < aRange)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        //if the current poisiton in not equal to lastPosition
        //then the position has changed
        float x = Mathf.Abs(transform.position.x - lastPosition.x);
        float z = Mathf.Abs(transform.position.x - lastPosition.x);
        if (x > 0.01f && z > 0.01f)
        {
            Vector3 dir = lastPosition - transform.position;
            dir.Normalize();
            lastPosition = transform.position;
            //Debug.Log(dir);

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
