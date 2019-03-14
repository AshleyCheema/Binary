using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class TrackerAbility : Cooldown
{
    public Abilities tracker;
    private GameObject trackingDevice;
    private Trigger trackerTrigger;

    private bool trackerActive;
    public bool trackerDown;

    private Collider deviceCollider;
    private Vector3 trackerPos;
    protected Client client;
    //private bool isThrowing;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        isCooldown = tracker.isCooldown;
        cooldown = tracker.abilityDuration;

        trackerActive = false;
        trackingDevice = GameObject.Find("Tracker");

        trackerTrigger = trackingDevice.GetComponent<Trigger>();
        trackingDevice.SetActive(false);
        deviceCollider = trackingDevice.GetComponent<Collider>();
        deviceCollider.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (trackerTrigger != null && trackerTrigger.isDetected)
        {
            //Tracker Position
            //Insert arrow pointing towards tracker position
            //Debug.Log("Detected");
            //ab_Tracker.TrackerTriggered = trackerTrigger.isDetected;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isCooldown && !trackerActive) //&& cooldown == tracker.abilityDuration)
            {
                trackingDevice.SetActive(true);
                trackerActive = true;
                //isCooldown = true;
            }
            else if(!isCooldown && trackerActive)
            {
                //isCooldown = false;
                trackerActive = false;
                trackingDevice.SetActive(false);
            }
        }

        if(trackerDown)
        {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0)
            {
                trackerDown = false;
                trackerActive = false;
                trackingDevice.SetActive(false);
                cooldown = tracker.abilityDuration;

                NetMsg_AB_Tracker ab_Tracker = new NetMsg_AB_Tracker();
                if (client != null)
                {
                    ab_Tracker.ConnectionID = client.ServerConnectionId;
                    ab_Tracker.TrackerPositionX = trackerPos.x;
                    ab_Tracker.TrackerPositionY = trackerPos.y;
                    ab_Tracker.TrackerPositionZ = trackerPos.z;
                    ab_Tracker.TrackerTriggered = false;
                    ab_Tracker.TrackerObjectIndex = 3;

                    client.Send(ab_Tracker);
                }
                //deviceCollider.enabled = false;
            }
        }

        if (!isCooldown && trackerActive)
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
                    isCooldown = true;
                    trackerDown = true;

                    #region NetMsg_Tracker

                    NetMsg_AB_Tracker ab_Tracker = new NetMsg_AB_Tracker();
                    if (client != null)
                    {
                        ab_Tracker.ConnectionID = client.ServerConnectionId;
                        ab_Tracker.TrackerPositionX = trackerPos.x;
                        ab_Tracker.TrackerPositionY = trackerPos.y;
                        ab_Tracker.TrackerPositionZ = trackerPos.z;
                        ab_Tracker.TrackerTriggered = true;
                        ab_Tracker.TrackerObjectIndex = 3;

                        client.Send(ab_Tracker);
                    }
                    #endregion
                }
            }
        }
    }
}
