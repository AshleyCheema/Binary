using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
	
	private Camera playerCam;
	private GameObject[] ignoreObjs;
	
    void Start()
    {
		playerCam = gameObject.GetComponentInChildren<Camera>();
        ignoreObjs = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
		
		foreach(GameObject go in ignoreObjs){
			if(go != gameObject){
				Collider goCollider = go.GetComponent<Collider>();
				
				if(GeometryUtility.TestPlanesAABB(planes, goCollider.bounds)){
					go.GetComponent<Renderer>().enabled = true;
				}else{
					go.GetComponent<Renderer>().enabled = false;
				}
			}
		}
    }
}
