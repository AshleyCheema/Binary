using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class NO_Exit : MonoBehaviour
{
    public int ID;
    private bool isOpen = false;
    public bool IsOpen
    { get { return isOpen; } set { isOpen = value; } }

    [SerializeField]
    private GameObject miniGame;

    [SerializeField]
    private HackingPuzzleManager hackingGame;

    private bool allowMiniGame;
    private bool exitOpen;

    private void Update()
    {
        if (isOpen && allowMiniGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                miniGame.SetActive(!miniGame.activeInHierarchy);
            }
        }
    }

    public void Hack()
    {
        bool resualt = hackingGame.StartPuzzle();

        if (resualt)
        {
            //open exit game object
            exitOpen = true;
            miniGame.SetActive(false);

            //tell every one that the exit is open
            EmptyMessage msg = new EmptyMessage();
            ClientManager.Instance.client.Send(MSGTYPE.CLIENT_EXITED_LEVEL, msg);

            //exit level do something 

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
        {
            if (exitOpen)
            {
                if (other.gameObject == ClientManager.Instance?.LocalPlayer.gameAvatar &&
                    other.tag == "Spy")
                {
                    //leave level completed
                    //tell every one that the exit is open
                    //tell every one that the exit is open
                    EmptyMessage msg = new EmptyMessage();
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_EXITED_LEVEL, msg);

                    //exit level do something 
                }
            }

            if (other.gameObject == ClientManager.Instance?.LocalPlayer.gameAvatar &&
                other.tag == "Spy")
            {
                if (ClientManager.Instance != null)
                {
                    allowMiniGame = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            if (other.gameObject == ClientManager.Instance?.LocalPlayer.gameAvatar &&
                other.tag == "Spy")
            {
                if (ClientManager.Instance != null)
                {
                    allowMiniGame = false;
                }
            }
        }
    }

}
