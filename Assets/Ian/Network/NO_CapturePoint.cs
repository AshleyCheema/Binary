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
    private float capturePercentage = 0.0f;
    [SerializeField]
    private TextMeshPro tm;

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
        if (tm != null)
        {
            tm.text = ((int)capturePercentage).ToString();
        }
        if (isBeingCaptured)
        {
            capturePercentage += captureAmount * Time.deltaTime;

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
                isBeingCaptured = true;

                GetComponent<MeshRenderer>().material.color = Color.red;

                Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();
                ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                ccp.IsBeingCaptured = true;
                ccp.ID = ID;

                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);

                if (ClientManager.Instance.LocalPlayer.gameAvatar == other.transform.parent.gameObject)
                {
                    miniGame.SetActive(true);
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
                Collider[] allColls = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
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

                    Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();
                    ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                    ccp.IsBeingCaptured = isBeingCaptured;
                    ccp.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);

                    if (ClientManager.Instance.LocalPlayer.gameAvatar == other.transform.parent.gameObject)
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