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

    [SerializeField]
    private GameObject mesh;

    private bool allowMiniGame;
    private bool exitOpen;
    public bool ExitOpen
    { get { return exitOpen; } set { exitOpen = value; } }

    private void Update()
    {
        if (isOpen && allowMiniGame)
        {
            if (Input.GetButtonDown("Hacking"))
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
            Msg_ClientExitAval msg = new Msg_ClientExitAval();
            msg.ConectID = (byte)ClientManager.Instance?.LocalPlayer.connectionId;
            msg.ExitID = (byte)ID;
            ClientManager.Instance.client.Send(MSGTYPE.CLIENT_EXITED_LEVEL, msg);

            //exit level do something 
            //disable spy
            ClientManager.Instance?.LocalPlayer.gameAvatar.SetActive(false);

            mesh.GetComponentInChildren<cakeslice.Outline>().color = 1;
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
                    Msg_ClientExitAval msg = new Msg_ClientExitAval();
                    msg.ExitID = 101;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_EXITED_LEVEL, msg);

                    //exit level do something 
                    //disable spy
                    ClientManager.Instance?.LocalPlayer.gameAvatar.SetActive(false);
                }
            }

            if (other.gameObject == ClientManager.Instance?.LocalPlayer.gameAvatar &&
                other.tag == "Spy")
            {
                if (ClientManager.Instance != null)
                {
                    allowMiniGame = true;

                    other.transform.GetChild(0).gameObject.GetComponent<SpyController>().cooldownScript.canHack = true;
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
                    other.transform.GetChild(0).gameObject.GetComponent<SpyController>().cooldownScript.canHack = false;
                }
            }
        }
    }

}
