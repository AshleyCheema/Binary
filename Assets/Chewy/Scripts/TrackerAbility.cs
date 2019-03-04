using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class TrackerAbility : MonoBehaviour
{
    public Abilities tracker;
    private float cooldown;
    private bool isCooldown;
    private float a_Duration;
    private GameObject trackingDevice;
    private Trigger trackerTrigger;
    private bool trackerDown;
    private Collider deviceCollider;
    private Vector3 trackerPos;
    protected Client client;
    private NetMsg_AB_Tracker ab_Tracker = new NetMsg_AB_Tracker();
    //private bool isThrowing;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        cooldown = tracker.cooldown;
        isCooldown = tracker.isCooldown;
        a_Duration = tracker.abilityDuration;
        trackingDevice = GameObject.Find("Tracker");
        trackerTrigger = trackingDevice.GetComponent<Trigger>();
        trackingDevice.SetActive(false);
        deviceCollider = trackingDevice.GetComponent<Collider>();
        deviceCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackerTrigger != null && trackerTrigger.isDetected)
        {
            //Tracker Position
            //Insert arrow pointing towards tracker position
            Debug.Log("Detected");
            ab_Tracker.TrackerTriggered = trackerTrigger.isDetected;
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

                    #region NetMsg_Tracker
                    ab_Tracker.ConnectionID = client.ServerConnectionId;
                    ab_Tracker.TrackerPosition = trackerPos;
                    ab_Tracker.TrackerObject = trackingDevice;
                    client.Send(ab_Tracker);
                    #endregion
                }
            }
        }
    }
}
