using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
	
	public LayerMask layerMask;
	private Camera playerCam;
	private GameObject[] ignoreObjs;
	
    void Start()
    {
		//initialise variables
		playerCam = gameObject.GetComponentInChildren<Camera>();
        ignoreObjs = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
		
		//loop through each object with the player tag
		foreach(GameObject go in ignoreObjs){
			if(go != gameObject){ //ignore if the object is the client
				Collider goCollider = go.GetComponent<Collider>();
				
				//test if the collider is clipping the camera
				if(GeometryUtility.TestPlanesAABB(planes, goCollider.bounds)){ 
					
					//cast a ray that checks if a wall is in the way
					RaycastHit hit;
					if (Physics.Linecast(transform.position, go.transform.position, out hit, layerMask) && hit.transform.gameObject == go)
					{
						go.GetComponent<Renderer>().enabled = true;
					}else{
						go.GetComponent<Renderer>().enabled = false;
					}
				}else{
					go.GetComponent<Renderer>().enabled = false;
				}
			}
		}
    }
}
