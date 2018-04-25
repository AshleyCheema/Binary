using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public float movementValue = 0.5f;
    private NavMeshAgent xNavMeshAgent;
    Vector3 v3TargetPosition;
    RaycastHit hit;
    Ray ray;
    bool _isClimbing = false;

    // Use this for initialization
    void Start ()
    {
        xNavMeshAgent = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey("w"))
        {
            transform.Translate(0, 0, movementValue);
        }
        if(Input.GetKey("s"))
        {
            transform.Translate(0, 0, -movementValue);
        }
        if(Input.GetKey("a"))
        {
            transform.Translate(-movementValue, 0, 0);
        }
        if(Input.GetKey("d"))
        {
            transform.Translate(movementValue, 0, 0);
        }

        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                //transform.position = (Camera.main.WorldToScreenPoint(hit.collider.transform.position));
                //transform.position = hit.point + new Vector3(0, 0.5f, 0);

                xNavMeshAgent.SetDestination(hit.point);
            }
        }

        if (_isClimbing)
        {
            //transform.Translate(0, 1, 0);
            transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
            Debug.Log("Climbing");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WallClimbTrigger")
        {
            //transform.Translate(0, 1, 0);
            _isClimbing = true;
            xNavMeshAgent.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "WallClimbTrigger")
        {
            if (_isClimbing)
            {
                //transform.Translate(0, 1, 0);
               // transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
                //Debug.Log("Climbing");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isClimbing = false;
        StartCoroutine(EndClimb());
    }
   
    IEnumerator EndClimb()
    {
        float timer = 2f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            transform.position += (transform.forward * 1) * Time.deltaTime;

            yield return null;
        }
        xNavMeshAgent.enabled = true;

        yield return null;
    }
}
