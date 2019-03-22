using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public class ExitManager : Singleton<ExitManager>
{
    [SerializeField]
    private NO_CapturePoint[] capturePoints;

    [SerializeField]
    private NO_Exit[] exits;
    public NO_Exit[] Exits
    { get { return exits; } }

    bool ExitsOpen
    {
        get
        {
            int cpCaptured = 0;
            for (int i = 0; i < capturePoints.Length; i++)
            {
                if (capturePoints[i].IsCaptured)
                {
                    cpCaptured += 1;
                }
            }

            if (cpCaptured >= 2)
            {
                return true;
            }
            return false;
        }
    }

    public void CapturePointCaptured()
    {
        if(ExitsOpen)
        {
            //Open all exits are they are now open
            for (int i = 0; i < exits.Length; i++)
            {
                exits[i].IsOpen = true;
            }

            for (int i = 0; i < exits.Length; i++)
            {
                Msg_ClientExit ce = new Msg_ClientExit();
                ce.IsOpen = true;
                ce.ID = exits[i].ID;
                HostManager.Instance.SendAll(MSGTYPE.CLIENT_EXIT, ce);
            }

            //NetMsg_Exit_Open exitOpen = new NetMsg_Exit_Open();
            //exitOpen.IsOpen = true;
            //exitOpen.ID = 4;
            //Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);

            //exitOpen = new NetMsg_Exit_Open();
            //exitOpen.IsOpen = true;
            //exitOpen.ID = 5;
            //Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);

            //exitOpen = new NetMsg_Exit_Open();
            //exitOpen.IsOpen = true;
            //exitOpen.ID = 6;
            //Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);
        }
    }
}
