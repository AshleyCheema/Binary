using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NO_Exit : MonoBehaviour
{
    public int ID;
    private bool isOpen = false;
    public bool IsOpen
    { get { return isOpen; } set { isOpen = value; } }

    [SerializeField]
    private GameObject miniGame;

    private void OnTriggerEnter(Collider other)
    {
        if(isOpen)
        {
            if(other.tag == "Spy")
            {
                if(ClientManager.Instance != null)
                {
                    miniGame.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            if (other.tag == "Spy")
            {
                if (ClientManager.Instance != null)
                {
                    miniGame.SetActive(false);
                }
            }
        }
    }

}
