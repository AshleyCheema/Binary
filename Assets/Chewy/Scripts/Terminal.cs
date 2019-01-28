using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Camera camera;
    private Renderer renderer;
    public GameController gameController;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<Renderer>();
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update ()
    { 
        if (camera.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameController.enabled = true;
                camera.enabled = false;
            }
        }
    }

    private void OnMouseOver()
    {
        renderer.material.color = Color.red;

        if(Input.GetMouseButtonDown(1))
        {
            gameController.enabled = false;
            camera.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
