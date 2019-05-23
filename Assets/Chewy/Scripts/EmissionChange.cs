using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionChange : MonoBehaviour
{
    private Renderer rend;
    public int emissionColour;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void ColourChange(Color color)
    {
       rend.material.SetColor("_EmissionColor", color * emissionColour);
    }
}
