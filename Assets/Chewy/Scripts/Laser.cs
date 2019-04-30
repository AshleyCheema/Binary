using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    Gradient gradientRed = new Gradient();
    Gradient gradientGreen = new Gradient();
    public GameObject firePos;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        gradientRed.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 1.0f) }, 
                            new GradientAlphaKey[] { new GradientAlphaKey(0.09f, 0.0f), new GradientAlphaKey(0f, 1.0f) });

        gradientGreen.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 1.0f) },
                              new GradientAlphaKey[] { new GradientAlphaKey(0.09f, 0.0f), new GradientAlphaKey(0.0f, 1f) });
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, firePos.transform.position);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            if(hit.collider)
            {
                Vector3 pos = lineRenderer.GetPosition(0);
                pos += firePos.transform.forward * hit.distance;
                lineRenderer.SetPosition(1, pos);
                lineRenderer.colorGradient = gradientGreen;
        
            }
            if (hit.collider.tag == "Spy")
            {
                lineRenderer.colorGradient = gradientRed;
            }
        }
        else
        {
            Vector3 pos = lineRenderer.GetPosition(0);
            pos += firePos.transform.forward * 500;
            lineRenderer.SetPosition(1, pos);
        }
    }
}
