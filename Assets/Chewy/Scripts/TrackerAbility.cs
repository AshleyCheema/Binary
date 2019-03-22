using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class TrackerAbility : Cooldown
{
    public Abilities tracker;
    private GameObject trackingDevice;
    private Trigger trackerTrigger;
    private GameObject arrowPos;
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
        arrowPos = gameObject.transform.GetChild(2).GetChild(4).GetChild(3).gameObject;
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
            ArrowPointer();
        }
        else
        {
            arrowPos.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isCooldown && !trackerActive) //&& cooldown == tracker.abilityDuration)
            {
                trackerTrigger.isDetected = false;
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
                trackerTrigger.isDetected = false;
                trackerDown = false;
                trackerActive = false;
                trackingDevice.SetActive(false);
                cooldown = tracker.abilityDuration;

                Msg_Client_AB_Tracker ab_Tracker = new Msg_Client_AB_Tracker();
                if (ClientManager.Instance != null)
                {
                    ab_Tracker.ConnectionID = ClientManager.Instance.LocalPlayer.connectionId;
                    ab_Tracker.TrackerPosition = trackerPos;
                    ab_Tracker.TrackerTriggered = false;
                    ab_Tracker.TrackerObjectIndex = 3;

                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_TRACKER, ab_Tracker);
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

                    Msg_Client_AB_Tracker ab_Tracker = new Msg_Client_AB_Tracker();
                    if (ClientManager.Instance != null)
                    {
                        ab_Tracker.ConnectionID = ClientManager.Instance.LocalPlayer.connectionId;
                        ab_Tracker.TrackerPosition = trackerPos;
                        ab_Tracker.TrackerTriggered = true;
                        ab_Tracker.TrackerObjectIndex = 3;

                        ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_TRACKER, ab_Tracker);
                    }
                    #endregion
                }
            }
        }
    }

    void ArrowPointer()
    {
       //Transform playerTransform;

       Vector3 direction = transform.InverseTransformPoint(trackerPos);


       float hideDistance = 1f;
       
       //Vector3 direction = trackerPos - arrowPos.transform.position;
       
       if(direction.magnitude < hideDistance)
       {
           arrowPos.SetActive(false);
           trackerTrigger.isDetected = false;
       }
       else
       {
            arrowPos.SetActive(true);

            float a = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            a += 180;
            arrowPos.transform.localEulerAngles = new Vector3(0, 0, a);

            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //arrowPos.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
