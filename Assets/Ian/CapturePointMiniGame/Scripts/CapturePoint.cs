/*
 * Author: Ian Hudson
 * Description: 
 * Created: 19/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    [SerializeField]
    private bool spyIsCapturing = false;

    [SerializeField]
    private float capturePercentage = 0.0f;

    [SerializeField]
    private float captureMulitiplier = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if(spyIsCapturing)
        {
            capturePercentage += (1.0f * captureMulitiplier) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spy")
        {
            spyIsCapturing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Spy")
        {
            spyIsCapturing = false;
            capturePercentage = Mathf.RoundToInt(capturePercentage);
        }
    }
}
