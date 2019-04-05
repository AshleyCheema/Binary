/*
 * Author: Ian Hudson
 * Description: Fix the rotation so it does not change 
 * Created: 07/03/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWMask : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;

    [SerializeField]
    bool isCone = false;

    // Start is called before the first frame update
    void Start()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isCone)
        {
            position = transform.parent.transform.position + new Vector3(4.0f, 0.0f, 0.0f);
            transform.position = position;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            position = transform.parent.transform.position + new Vector3(4.0f, 0.0f, 0.0f);
            transform.position = position;
            transform.forward = transform.parent.transform.forward;
        }
    }
}
