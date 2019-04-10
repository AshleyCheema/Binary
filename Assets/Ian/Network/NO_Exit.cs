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

    [SerializeField]
    private HackingPuzzleManager hackingGame;

    private bool allowMiniGame;
    private bool exitOpen;

    private void Update()
    {
        if(isOpen && allowMiniGame)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                miniGame.SetActive(!miniGame.activeInHierarchy);
            }
        }
    }

    public void Hack()
    {
        bool resualt = hackingGame.StartPuzzle();

        if(resualt)
        {
            //open exit game object
            exitOpen = true;
            miniGame.SetActive(false);

            //tell every one that the exit is open
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isOpen)
        {
            if(exitOpen)
            {
                //leave level completed
                MiniModule_GameOver.Instance.SpyExitedLevel(ClientManager.Instance.LocalPlayer.connectionId);
            }

            if(other.gameObject == ClientManager.Instance?.LocalPlayer.gameAvatar &&
                other.tag == "Spy")
            {
                if(ClientManager.Instance != null)
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
