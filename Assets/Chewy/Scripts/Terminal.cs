using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The hacking terminal checks if the mouse is hovered over the object which then highlights
/// If the player is close enough to the terminal gameobject then they can hack it an access stuff...
/// </summary>
public class Terminal : MonoBehaviour
{
    public Camera camera;
    private Renderer renderer;
    private GameController gameController;
    private Material m;

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<Renderer>();
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
        m = this.renderer.material;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //Check if the camera is enabled
        if (camera.enabled == true)
        {
            //if so and the escape is pressed then turn off camera and re-enable player controls
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameController.enabled = true;
                camera.enabled = false;
            }
        }
    }

    //Hover mouse over gameobject, this will change it's colour.
    //If the player is close enough to the gameobject it is hackable
    private void OnMouseOver()
    {
        m.SetColor(Color.red);
        if (gameController.isTerminal == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                gameController.enabled = false;
                camera.enabled = true;
            }
        }
    }

    //Change back to original colour when no longer hovered over
    private void OnMouseExit()
    {
        m.SetColor(Color.white);
    }
}
