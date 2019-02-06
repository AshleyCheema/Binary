using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is the for the drones movement
/// </summary>
public class Drone : MonoBehaviour
{
    private GameController gameController;

    // Use this for initialization
    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("Player").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update ()
    {
        //Check whether the drone has been activated
        if(gameController.isDrone)
        {
            //turn off player controller so the player cannot move when the drone is active
            gameController.enabled = false;

            //When key H is pressed turn off drone and re-enable player controls
            if(Input.GetKeyDown(KeyCode.H))
            {
                gameObject.SetActive(false);
                gameController.enabled = true;
                gameController.isDrone = false;
            }
        }

        //Movement keys
	    if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.right * 0.05f);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.right * -0.05f);
        }
        if(Input.GetKey(KeyCode.A))
        {
            //transform.Rotate(Vector3.down * 1);
            transform.Translate(Vector3.forward * 0.05f);

        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.Rotate(Vector3.up * 1);
            transform.Translate(Vector3.forward * -0.05f);

        }
    }

    //Check whether the drone is close enough to an enemy to be stunned
    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Renderer>().material.SetColor(Color.black);
                other.gameObject.GetComponent<EnemyAi>().StopCoro();
                Debug.Log("Phaser Set to Stun");
            }
        }
    }
}
