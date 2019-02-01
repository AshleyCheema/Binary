/*
 * 
 * Store the percentage of light object is in
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    [SerializeField]
    private float lightMeter;

    private SceneLight sceneLight;
    private bool updateLightMeter = false;

    // Update is called once per frame
    void Update()
    {
        UpdateLightMeter();
    }

    private void UpdateLightMeter()
    {
        if(updateLightMeter)
        {
            Debug.Log("Updating light meter");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LightSource")
        {
            sceneLight = other.gameObject.transform.parent.gameObject.GetComponent<SceneLight>();
            updateLightMeter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LightSource")
        {
            sceneLight = null;
        }
    }
}
