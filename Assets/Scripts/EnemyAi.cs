using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    ///Searching
    public float timer = 10f;
    private Quaternion patroling = Quaternion.Euler(0, 180, 0);
    private bool rotated = false;

    ///Patroling 
    public Transform pathHolder;
    //public float waitTime = 0.3f;
    public float speed = 5f;
    private Coroutine c_FollowPath;
    private WaitForSeconds followPathDuration = new WaitForSeconds(5);

    ///LookAt/Rotate
    public float turnSpeed = 90;
    private Coroutine c_Rotating;

    ///Detection
    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    private Transform player;
    private float viewAngle;
    private RaycastHit hit;
    private Color originalSpotColor;
    private bool isSpotted = false;
    private float waitTimer = 5;
    private float spottingTimer = 3;
    private Vector3 lastKnownPos;
    private Vector3 targetWaypoint;
    private Vector3[] waypoints;

    void Start()
    {
        viewAngle = spotlight.spotAngle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotColor = spotlight.color;
        

        waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        c_FollowPath = StartCoroutine(FollowPath(waypoints, 0));
    }

    void Update()
    {
        //Change the spotlight color even the player is detected
        if(CanSeePlayer())
        {
            isSpotted = true;
            spottingTimer -= Time.deltaTime;
            spotlight.color = Color.red;
            lastKnownPos = new Vector3(player.position.x, transform.position.y, player.position.z);

            if (spottingTimer <= 0)
            {
                ShootPlayer();
            }
        }
        else
        {
            spottingTimer = 3;
            spotlight.color = originalSpotColor;
            isSpotted = false;

            if(c_FollowPath == null)
            {
                GoToLastPosition();

                if(transform.position == lastKnownPos)
                {
                    waitTimer -= Time.deltaTime;

                    if(waitTimer <= 0)
                    {
                        DistanceCheck();
                        waitTimer = 5;
                    }
                }
            }

            //isChasing = false;
            //if (isChasing == false)
            //{
            //    chaseTimer -= Time.deltaTime;
            //
            //    if (c_FollowPath == null && !isChasing && chaseTimer <= 0)
            //    {
            //        c_FollowPath = StartCoroutine(FollowPath(waypoints, 3));
            //        chaseTimer = 5;
            //    }
            //}
            //chaseTimer = 5;  
        }

        //timer -= Time.deltaTime;
        //
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        //{
        //    Debug.Log("Hit");
        //}
        //
        ////Find more efficent way then timer
        ////Rotates back and forth like a generic guard
        //if (timer <= 5)
        //{
        //    //PatrolingRotate();
        //    rotated = true;
        //}
        //else
        //{
        //    //PatrolingRotate();
        //    rotated = false;
        //}
        //if (timer <= 0)
        //{
        //    timer = 10;
        //}

    }

    private void DistanceCheck()
    {
        int closestNode = -1;
        float closestNodeDis = float.MaxValue;

        for (int i = 0; i < waypoints.Length; i++)
        {
            //float lastDist = (transform.position - waypoints[i - 1]).magnitude;
            //Debug.Log(lastDist);

            float distance = (transform.position - waypoints[i]).magnitude;

            if (distance < closestNodeDis)
            {
                closestNode = i;
                closestNodeDis = distance;
            }
        }

        transform.LookAt(waypoints[closestNode]);
        c_FollowPath = StartCoroutine(FollowPath(waypoints, closestNode));
    }

    //This function is used to detect the player within the cone of vision of the spotlight
    bool CanSeePlayer()
     {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, dirToPlayer);

            if(angleBetween < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, player.position, out hit, viewMask))
                {
                    //lastKnownPos = new Vector3(player.position.x, transform.position.y, player.position.y); //new Vector3(hit.point.x, transform.position.y, hit.point.z);

                    if(c_FollowPath != null)
                    {
                        StopCoroutine(c_FollowPath);
                        c_FollowPath = null;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    //The function is used to make the AI patrol a certain route in the level
    //It works by taking in array of positions and then the AI moving to those position
    IEnumerator FollowPath(Vector3[] waypoints, int targetWaypointIndex)
    {
        //transform.position = waypoints[0];
        //targetWaypointIndex = 1;
        targetWaypoint = waypoints[targetWaypointIndex];

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                Debug.Log(targetWaypointIndex);
                //TIMER HERE
                yield return followPathDuration;
                //Start the LookAt function
                c_Rotating = StartCoroutine(LookAtPatrol(targetWaypoint));
                yield return c_Rotating;
            }
            yield return null;
        }
    }

    //This function makes the AI movement move fluid, so the AI now turns to look at the next waypoint 
    //instead of staying in the same facing position throughout
    IEnumerator LookAtPatrol(Vector3 lookTarget)
    {
        Vector3 directionLook = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(directionLook.z, directionLook.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    //Change to searching?
    private void PatrolingRotate()
    {
        if (rotated == false)
        {
            transform.rotation = patroling;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void ChasePlayer()
    {
        timer -= Time.deltaTime;

        transform.LookAt(player);
        if(timer <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            //isChasing = true;
        }
    }

    private void ShootPlayer()
    {

    }

    private void GoToLastPosition()
    {
        if (!isSpotted)
        {
            transform.LookAt(lastKnownPos);
            transform.position = Vector3.MoveTowards(transform.position, lastKnownPos, speed * Time.deltaTime);
        }
        else
        {

        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Debug.DrawRay(transform.position, transform.forward * viewDistance, Color.red);

    }
}