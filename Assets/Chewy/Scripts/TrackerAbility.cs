using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class TrackerAbility : Cooldown
{
    public Abilities tracker;
    private GameObject trackingDevice;
    private Trigger trackerTrigger;
    private GameObject arrowPointer;
    private RectTransform arrowRect;
    private bool trackerActive;
    public bool trackerDown;
    private Camera camera;
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
        arrowPointer = gameObject.transform.GetChild(2).GetChild(0).gameObject;
        arrowRect = arrowPointer.GetComponent<RectTransform>();
        trackerActive = false;
        trackingDevice = GameObject.Find("Tracker");
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        trackerTrigger = trackingDevice.GetComponent<Trigger>();
        trackingDevice.SetActive(false);
        deviceCollider = trackingDevice.GetComponent<Collider>();
        deviceCollider.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (trackerTrigger != null && trackerTrigger.isDetected)
        //{
        //    ArrowPointer();
        //}
        //else
        //{
        //    arrowPointer.SetActive(false);
        //}

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

    //void ArrowPointer()
    //{
    //
    //    arrowPointer.SetActive(true);
    //
    //    Vector3 dir = Camera.main.WorldToViewportPoint(transform.position) - Camera.main.WorldToViewportPoint(trackerPos);
    //    dir.Normalize();
    //    float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    //    
    //    angle += 180f;
    //    arrowRect.transform.eulerAngles = new Vector3(0, 0, angle);
    //
    //    Debug.Log(dir);
    //    Debug.Log(angle);
    //    //float hideDistance = 1f;
    //
    //    //if (direction.magnitude < hideDistance)
    //    //{
    //    //    arrowPointer.SetActive(false);
    //    //    trackerTrigger.isDetected = false;
    //    //}
    //    //else
    //    //{
    //    //    arrowPointer.SetActive(true);
    //    //
    //    //
    //    //    //float a = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
    //    //    //a += 180;
    //    //    //arrowPointer.transform.localEulerAngles = new Vector3(0, 0, a);
    //    //
    //    //    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    //    //arrowPointer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    //}
    //}
}
