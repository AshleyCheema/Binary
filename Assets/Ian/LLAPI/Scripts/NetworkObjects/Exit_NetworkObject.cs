using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    public class Exit_NetworkObject : Network_Object
    {
        private bool isOpen = false;
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            set
            {
                if(isOpen == false && value == true)
                {
                    SendIsOpen();
                }
                isOpen = value;
            }
        }

        [SerializeField]
        private GameObject miniGame = null;

        private void SendIsOpen()
        {
            //send message tell this is open

        }

        public override void Recive(NetMsg a_netMsg)
        {
            NetMsg_Exit_Open exit = (NetMsg_Exit_Open)a_netMsg;

            isOpen = exit.IsOpen;

            //do something
            if(isOpen)
            {
                Debug.Log("Exits are open");
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Spy")
            {
                if (isOpen)
                {
                    miniGame.SetActive(true);
                }
            }
        }
    }
}
