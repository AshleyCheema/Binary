using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    private NavMeshAgent xNavMeshAgent;
    Vector3 v3TargetPosition;
    RaycastHit hit;
    RaycastHit hitRight;
    Ray ray;
    bool _isClimbing = false;

    // Use this for initialization
    void Start ()
    {
        xNavMeshAgent = GetComponent<NavMeshAgent>();
        xNavMeshAgent.updateRotation = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 playerToMouse = hit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            transform.rotation = newRotation;
        }

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //transform.position = (Camera.main.WorldToScreenPoint(hit.collider.transform.position));
                //transform.position = hit.point + new Vector3(0, 0.5f, 0);
                v3TargetPosition = hit.point;

                xNavMeshAgent.SetDestination(hit.point);
            }
        }


        if(Input.GetMouseButtonDown(1))
        {
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if(Physics.Raycast(ray, out hitRight))
            {
               if(hitRight.collider.tag == "Ladder")
                {
                   v3TargetPosition = hitRight.point;
                   xNavMeshAgent.SetDestination(new Vector3(hitRight.point.x, 0f, hitRight.point.z));
                   Vector3 forward = transform.forward;
                   Vector3 otherForward = hitRight.collider.transform.forward;
                   if(Vector3.Dot(forward, otherForward) >= 0.98f)
                   {
                        
                   }
        
                }
            }
        }

        //if(Vector3.Distance(transform.position, v3TargetPosition) <= 1f)
        //{
        //    //xNavMeshAgent.isStopped = true;
        //}

        //if (_isClimbing)
        //{
        //    Debug.Log(hit.point);
        //    if (hit.point.y > 0.5)
        //    {
        //        //transform.Translate(0, 1, 0);
        //        transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
        //        Debug.Log("Climbing");
        //    }
        //    else if (hit.point.y < 0.1)
        //    {
        //        transform.position += new Vector3(0, -1f * Time.deltaTime, 0);
        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "WallClimbTrigger")
        //{
        //    //transform.Translate(0, 1, 0);
        //    //_isClimbing = true;
        //    //xNavMeshAgent.enabled = false;
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.gameObject.tag == "WallClimbTrigger")
        //{
        //    if (_isClimbing)
        //    {
        //        //transform.Translate(0, 1, 0);
        //       // transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
        //        //Debug.Log("Climbing");
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //_isClimbing = false;
        //StartCoroutine(EndClimb());
    }
 
}
