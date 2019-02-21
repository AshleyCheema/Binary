using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The hacking terminal checks if the mouse is hovered over the object which then highlights
/// If the player is close enough to the terminal gameobject then they can hack it an access stuff...
/// </summary>
public class Terminal : MonoBehaviour
{
    public Camera[] camera;
    private KeyCode[] cameraKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    private Renderer renderer;
    private bool cameraActive;
    [SerializeField]
    private MercControls mercController;
    private int currentCamera;
    //[SerializeField]
    //private GameObject hackingCanvas;
    //[SerializeField]
    //private GameObject hackOutput;

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Check if the camera is enabled
        if (cameraActive)
        {
            for (int i = 0; i < cameraKeys.Length; i++)
            {
                if(Input.GetKeyDown(cameraKeys[i]))
                {
                    for (int j = 0; j < camera.Length; j++)
                    {
                        camera[j].enabled = (i == j) ? true : false;
                        currentCamera = j;
                    }
                }
            }

            //if so and the escape is pressed then turn off camera and re-enable player controls
            if (cameraActive && Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (Camera cam in camera)
                {
                    cam.enabled = false;
                }
                mercController.enabled = true;
                cameraActive = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        if(Input.GetKeyDown(KeyCode.E))
        {
            camera[currentCamera].enabled = true;
            mercController.enabled = false;
            cameraActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    //public void Enable()
    //{
    //    if (hackOutput.activeInHierarchy == true)
    //    {
    //        Debug.Log("Terminal is enabled");
    //        hackingCanvas.SetActive(true);
    //        mercController.enabled = false;
    //    }
    //
    //}

    //public void Hack()
    //{
    //    bool result = hackingCanvas.GetComponent<HackingPuzzleManager>().StartPuzzle();
    //
    //    hackingCanvas.SetActive(false);
    //    mercController.enabled = true;
    //
    //    if(result)
    //    {
    //        Debug.Log("Hack Complete");
    //        hackOutput.SetActive(false);
    //    }
    //    else
    //    {
    //        Debug.Log("Hack Failed");
    //    }
    //}


}
