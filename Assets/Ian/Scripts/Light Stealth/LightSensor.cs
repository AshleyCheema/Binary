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
            float relDis = Vector3.Distance(sceneLight.transform.position - new Vector3(0, 5, 0), 
                                            new Vector3(transform.position.x, 0, transform.position.z));
            lightMeter = Map(relDis, 3.5f, 0, 0, 100);
            Debug.Log(lightMeter);
        }
    }

    private float Map(float a_v, float a_start1, float a_stop1, float a_start2, float a_stop2)
    {
        return ((a_v - a_start1) / (a_stop1 - a_start1)) * (a_stop2 - a_start2) + a_start2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LightSource")
        {
            sceneLight = other.gameObject.GetComponent<SceneLight>();
            updateLightMeter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LightSource")
        {
            sceneLight = null;
            updateLightMeter = false;
        }
    }
}
