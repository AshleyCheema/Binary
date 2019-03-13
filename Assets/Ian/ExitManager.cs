using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public class ExitManager : MonoBehaviour
{
    private static ExitManager insntane;
    public static ExitManager Insntane
    { get{return insntane;}}

    [SerializeField]
    private CP_NetworkObject[] capturePoints;

    [SerializeField]
    private Exit_NetworkObject[] exits;

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

    // Start is called before the first frame update
    void Start()
    {
        if(insntane == null)
        {
            insntane = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

            NetMsg_Exit_Open exitOpen = new NetMsg_Exit_Open();
            exitOpen.IsOpen = true;
            exitOpen.ID = 4;
            Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);

            exitOpen = new NetMsg_Exit_Open();
            exitOpen.IsOpen = true;
            exitOpen.ID = 5;
            //Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);

            exitOpen = new NetMsg_Exit_Open();
            exitOpen.IsOpen = true;
            exitOpen.ID = 6;
            //Server.Instance.Send(exitOpen, Server.Instance.ReliableChannel);
        }
    }
}
