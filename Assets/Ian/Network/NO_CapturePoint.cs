using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NO_CapturePoint : MonoBehaviour
{
    public int ID;

    [SerializeField]
    private bool isBeingCaptured = false;
    public bool IsBeingCaptured
    { get { return isBeingCaptured; } set { isBeingCaptured = value; } }
    [SerializeField]
    private float captureAmount = 5.0f;
    [SerializeField]
    private float capturePercentage = 0.0f;
    [SerializeField]
    private TextMesh tm;

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
            tm.text = capturePercentage.ToString();
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
                isBeingCaptured = false;

                GetComponent<MeshRenderer>().material.color = Color.white;

                Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();
                ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                ccp.IsBeingCaptured = false;
                ccp.ID = ID;

                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
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