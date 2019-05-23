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

    public Renderer[] objectsAround;
    private Coroutine c_lerpColor;
    private SpyController spyController;

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

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isBeingCaptured == false)
        {
            StartHacking();
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
        isBeingCaptured = true;
        GetComponent<MeshRenderer>().material.color = Color.red;

        Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();
        ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
        ccp.IsBeingCaptured = true;
        ccp.ID = ID;

        ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
        if (spyController != null)
        {
            spyController.cooldownScript.gameObject.SetActive(false);

            if (ClientManager.Instance.LocalPlayer.gameAvatar == spyController.transform.parent.gameObject)
            {

                c_lerpColor = StartCoroutine(LerpColor(Color.red, Color.green));
                miniGame.SetActive(true);
                miniGame.GetComponentInChildren<CapturePointMiniGame>().Show();
            }
        }
    }

    private IEnumerator LerpColor(Color start, Color end)
    {
        float lerpTimer = 0f;
        float inten = 25.0f;

        while (lerpTimer <= 1f)
        {
            lerpTimer = capturePercentage / 100;
            Debug.Log(lerpTimer);
            Color newColor = Color.Lerp(start, end, lerpTimer);

            newColor *= inten;
            for (int i = 0; i < objectsAround.Length - 1; i++)
            {
                objectsAround[i].material.SetColor("_EmissionColor", newColor);
            }

            yield return null;
        }

        end *= inten;
        for (int i = 0; i < objectsAround.Length - 1; i++)
        {
            objectsAround[i].material.SetColor("_EmissionColor", end);
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
                    Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();
                    ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                    ccp.IsBeingCaptured = isBeingCaptured;
                    ccp.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);

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