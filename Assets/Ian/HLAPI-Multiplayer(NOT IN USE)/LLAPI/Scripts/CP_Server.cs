using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    public class CP_Server : Network_Object
    {
        [SerializeField]
        private bool isBeingCaptured = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Spy")
            {
                isBeingCaptured = true;

                GetComponent<MeshRenderer>().material.color = Color.red;

                NetMsg_CP_Capture capture = new NetMsg_CP_Capture();
                capture.ID = ID;
                capture.IsBeingCaptured = isBeingCaptured;

                Server.Instance.Send(capture, Server.Instance.ReliableChannel);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Spy")
            {
                isBeingCaptured = false;

                GetComponent<MeshRenderer>().material.color = Color.white;

                NetMsg_CP_Capture capture = new NetMsg_CP_Capture();
                capture.ID = ID;
                capture.IsBeingCaptured = isBeingCaptured;


                Server.Instance.Send(capture, Server.Instance.ReliableChannel);
            }
        }
    }
}