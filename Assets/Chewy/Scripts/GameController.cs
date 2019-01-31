using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    private NavMeshAgent xNavMeshAgent;
    private Vector3 v3TargetPosition;
    private RaycastHit hit;
    private RaycastHit hitRight;
    private Ray ray;
    private bool _isClimbing = false;
    private bool enemySelected = false;
    public GameObject drone;
    public bool isDrone = false;
    public bool isTerminal = false;
    private GameObject throwable;
    private bool isThrowing;
 
    // Use this for initialization
    void Start ()
    {
        xNavMeshAgent = GetComponent<NavMeshAgent>();
        xNavMeshAgent.updateRotation = false;
        drone = GameObject.Find("Drone");
        throwable = GameObject.Find("Throwable");
        throwable.SetActive(false);
        drone.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //transform.position = (Camera.main.WorldToScreenPoint(hit.collider.transform.position));
                //transform.position = hit.point + new Vector3(0, 0.5f, 0);
                //v3TargetPosition = hit.point;
                xNavMeshAgent.isStopped = false;
                LookAt();
                xNavMeshAgent.SetDestination(hit.point);
            }
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            if (!isDrone)
            {
                drone.transform.position = gameObject.transform.position + new Vector3(2, 0, 0);
                drone.SetActive(true);
                isDrone = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!isThrowing)
            {
                throwable.SetActive(true);
                isThrowing = true;
            }
            else
            {
                isThrowing = false;
                throwable.SetActive(false);
            }
        }

        if(isThrowing)
        {
            if(Physics.Raycast(ray, out hit))
            {
                float radius = 10;
                Vector3 centerPosition = transform.position;
                float distance = Vector3.Distance(hit.point, centerPosition);

                if(distance > radius)
                {
                    Vector3 fromOrigin = hit.point - centerPosition;
                    fromOrigin *= radius / distance;
                    hit.point = centerPosition + fromOrigin;
                }

                throwable.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

                if(Input.GetMouseButtonDown(1))
                {
                    isThrowing = false;
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if(Physics.Raycast(ray, out hitRight))
            {
                if (hitRight.collider.tag == "Ladder")
                {
                    //v3TargetPosition = hitRight.point;
                    xNavMeshAgent.SetDestination(new Vector3(hitRight.point.x, 0f, hitRight.point.z));
                    Vector3 forward = transform.forward;
                    Vector3 otherForward = hitRight.collider.transform.forward;
                    if (Vector3.Dot(forward, otherForward) >= 0.98f)
                    {

                    }
                }

                if(hitRight.collider.tag == "Enemy")
                {
                    xNavMeshAgent.SetDestination(hitRight.point);
                    enemySelected = true;

                    if(gameObject.transform.position == hit.point)
                    {
                        enemySelected = false;
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

    void LookAt()
    {
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 playerToMouse = hit.point - transform.position;
    
            playerToMouse.y = 0f;
    
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
    
            transform.rotation = newRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "WallClimbTrigger")
        //{
        //    //transform.Translate(0, 1, 0);
        //    //_isClimbing = true;
        //    //xNavMeshAgent.enabled = false;
        //}

        if(other.gameObject.tag == "Terminal")
        {
            isTerminal = true;
        }
        else
        {
            isTerminal = false;
        }

        if(enemySelected == true)
        {
            if(other.gameObject.tag == "Enemy")
            {
                xNavMeshAgent.isStopped = true;
                Debug.Log("Kill");
                other.gameObject.GetComponent<EnemyAi>().alive = false;
                other.gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
        }
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
