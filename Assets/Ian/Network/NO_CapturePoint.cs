using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NO_CapturePoint : MonoBehaviour
{
    public int ID;

    [SerializeField]
    private bool isBeingCaptured = false;
    public bool IsBeingCaptured
    { get { return isBeingCaptured; } set { isBeingCaptured = value; } }
    [SerializeField]
    private float captureAmount = 0.5f;
    [SerializeField]
    public float capturePercentage = 0.0f;
    [SerializeField]
    private TextMeshProUGUI tm;

    public GameObject[] objectsAround;
    private Coroutine c_lerpColor;
    private SpyController spyController;
    private Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();

    [SerializeField]
    private GameObject miniGame;

    //has this capture point been captured
    public bool IsCaptured
    {
        get
        {
            if ((int)captureAmount == 100 || capturePercentage > 99.9f)
            {
                return true;
            }
            return false;
        }
    }

    private void Start()
    {
        ccp.ID = -1;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !IsCaptured)
        {
            StartHacking();

            if (ccp.ID == -1)
            {
                ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                ccp.IsBeingCaptured = true;
                ccp.ID = ID;
                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
            }
        }


        if (tm != null)
        {
            tm.text = ((int)capturePercentage).ToString() + "%";
        }
        if (isBeingCaptured)
        {
            capturePercentage += captureAmount * Time.deltaTime;

            if (capturePercentage < 100.0f)
            {
                PlayerStats.Instance.CaptureedAmount += captureAmount * Time.deltaTime;
            }

            if (capturePercentage > 100.0f)
            {
                capturePercentage = 100.0f;

                //if we are the host/server
                if (HostManager.Instance != null)
                {
                    ExitManager.Instance.CapturePointCaptured();
                    //check for the exit manager
                    //if (isServer)
                    //{
                    //    ExitManager.Insntane.CapturePointCaptured();
                    //}
                }
            }
        }
    }

    public void IncreaseCaptureAmount(bool aSend = true)
    {
        captureAmount += 0.5f;
        if (captureAmount > 3.0f)
        {
            captureAmount = 3.0f;
        }

        if (aSend)
        {
            //send message to server and clients 
            Msg_ClientCapturePointIncrease ccpi = new Msg_ClientCapturePointIncrease();
            ccpi.CapturePointAmount = captureAmount;
            ccpi.NOIndex = ID;
            ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT_INCREASE, ccpi);
        }
    }

    public void ResetCaptureAmount()
    {
        captureAmount = 0.5f;
    }

    /// <summary>
    /// Enter trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (ClientManager.Instance != null)
        {
            if (other.tag == "Spy")
            {
                spyController = other.gameObject.GetComponentInChildren<SpyController>();
                spyController.cooldownScript.canHack = true;

                //ccp = new Msg_ClientCapaturePoint();
                //ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                //ccp.IsBeingCaptured = true;
                //ccp.ID = ID;
                //ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);

                //if (spyController.hackingKeyPressed == true)
                //{
                //
                //    //NetMsg_CP_Capture capture = new NetMsg_CP_Capture();
                //    //capture.ID = ID;
                //    //capture.IsBeingCaptured = isBeingCaptured;
                //    //capture.Percentage = (int)capturePercentage;
                //    //
                //    //Server.Instance.Send(capture, Server.Instance.ReliableChannel);
                //}
            }
        }
    }

    private void StartHacking()
    {
        //if(ccp != null)
        //{
        //    isBeingCaptured = ccp.IsBeingCaptured;
        //}

        GetComponent<MeshRenderer>().material.color = Color.red;

        if (spyController != null)
        {
            spyController.cooldownScript.gameObject.SetActive(false);

           if (ClientManager.Instance.LocalPlayer.gameAvatar == spyController.transform.parent.gameObject)
           {
                if(!isBeingCaptured)
                {
                    c_lerpColor = StartCoroutine(LerpColor(Color.red, Color.green));
                    miniGame.SetActive(true);
                    miniGame.GetComponentInChildren<CapturePointMiniGame>().Show();
                }
            }
        }

        isBeingCaptured = true;//ccp.IsBeingCaptured;
    }

    private IEnumerator LerpColor(Color start, Color end)
    {
        float lerpTimer = 0f;

        while (lerpTimer <= 1f)
        {
            lerpTimer = capturePercentage / 100;

            Color newColor = Color.Lerp(start, end, lerpTimer);

            for (int i = 0; i < objectsAround.Length; i++)
            {
                objectsAround[i].GetComponent<EmissionChange>().ColourChange(newColor);
            }

            yield return null;
        }

        for (int i = 0; i < objectsAround.Length; i++)
        {
            objectsAround[i].GetComponent<EmissionChange>().ColourChange(end);

            if(objectsAround[i].GetComponent<cakeslice.Outline>() != null)
            {
                objectsAround[i].GetComponent<cakeslice.Outline>().color = 1;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Exit trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (ClientManager.Instance != null)
        {
            if (other.tag == "Spy")
            {
                Collider[] allColls = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius - 0.5f);
                bool reset = true;
                for (int i = 0; i < allColls.Length; i++)
                {
                    if (allColls[i].gameObject.tag == "Spy")
                    {
                        reset = false;
                        break;
                    }
                }

                if (reset)
                {
                    captureAmount = 0.5f;
                    GetComponent<MeshRenderer>().material.color = Color.white;
                    isBeingCaptured = false;
                    spyController.cooldownScript.canHack = false;
                    spyController.cooldownScript.gameObject.SetActive(true);
                    if (c_lerpColor != null)
                    {
                        StopCoroutine(c_lerpColor);
                        c_lerpColor = null;
                    }
                   
                    ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                    ccp.IsBeingCaptured = isBeingCaptured;
                    ccp.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
                    ccp.ID = -1;

                    if (ClientManager.Instance.LocalPlayer.gameAvatar == other.gameObject)
                    {
                        miniGame.SetActive(false);
                    }
                    //NetMsg_CP_Capture capture = new NetMsg_CP_Capture();
                    //capture.ID = ID;
                    //capture.IsBeingCaptured = isBeingCaptured;
                    //capture.Percentage = (int)capturePercentage;
                    //
                    //Server.Instance.Send(capture, Server.Instance.ReliableChannel);
                }
            }
        }
    }
}