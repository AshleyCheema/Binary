using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(target != null)
        { 
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }

    public void SetTarget(Transform a_tarTransform, Transform a_parent)
    {
        target = a_parent;
        transform.position = target.position;

        transform.position += new Vector3(-15f, 20, -15f);
        transform.rotation = Quaternion.Euler(40, 45, 0);

        offset = transform.position - target.position;
    }
}
