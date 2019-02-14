using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerAbility : MonoBehaviour
{
    public Abilities tracker;
    private float cooldown;
    private bool isCooldown;
    private float a_Duration;
    private GameObject trackingDevice;
    private bool trackerDown;
    private Collider deviceCollider;
    private Vector3 trackerPos;
    //private bool isThrowing;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = tracker.cooldown;
        isCooldown = tracker.isCooldown;
        a_Duration = tracker.abilityDuration;
        trackingDevice = GameObject.Find("Tracker");
        trackingDevice.SetActive(false);
        deviceCollider = trackingDevice.GetComponent<Collider>();
        deviceCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingDevice.GetComponent<Trigger>().isDetected == false)
        {
            //Tracker Position
            Debug.Log("Detected");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isCooldown && a_Duration == tracker.abilityDuration)
            {
                trackingDevice.SetActive(true);
                isCooldown = true;
            }
            else if(!trackerDown)
            {
                isCooldown = false;
                trackingDevice.SetActive(false);
            }
        }

        if(trackerDown)
        {
            a_Duration -= Time.deltaTime;

            if(a_Duration <= 0)
            {
                trackerDown = false;
                trackingDevice.SetActive(false);
                a_Duration = tracker.abilityDuration;
                deviceCollider.enabled = false;
            }
        }

        if (isCooldown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float radius = 10;
                Vector3 centerPosition = transform.position;
                float distance = Vector3.Distance(hit.point, centerPosition);

                if (distance > radius)
                {
                    Vector3 fromOrigin = hit.point - centerPosition;
                    fromOrigin *= radius / distance;
                    hit.point = centerPosition + fromOrigin;
                }

                trackingDevice.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    trackerPos = trackingDevice.transform.position;
                    deviceCollider.enabled = true;
                    isCooldown = false;
                    trackerDown = true;
                }
            }
        }
    }
}
