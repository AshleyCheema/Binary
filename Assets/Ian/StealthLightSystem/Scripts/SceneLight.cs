/*
 * 
 * This script should be attached to all lights that need intacting with 
 * the light meter
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLight : MonoBehaviour
{
    private SphereCollider collider;
    public SphereCollider Collider
    { get { return collider; } }

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
    }
}
