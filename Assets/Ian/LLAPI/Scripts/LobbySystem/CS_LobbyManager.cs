using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LLAPI;
using UnityEngine.SceneManagement;
using System;

public class CS_LobbyManager : MonoBehaviour
{
    private static CS_LobbyManager instance;
    public static CS_LobbyManager Instance
    { get { return instance; } }

    [SerializeField]
    private Client client;
    public Client Client
    { get { return client; } }

    [SerializeField]
    private CS_Lobby cs_lobby;

    [SerializeField]
    private RectTransform currentPanel;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeTo(RectTransform a_newPanel)
    {
        if(currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }

        if (a_newPanel != null)
        {
            a_newPanel.gameObject.SetActive(true);
        }

        currentPanel = a_newPanel;
    }

    public void SetShell(LLAPI.Player a_p)
    {
        a_p.lobbyAvater.GetComponentInChildren<TMP_InputField>().interactable = false;
        Button[] buttons = a_p.lobbyAvater.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

    }

    public void AddLobbyPlayer(LLAPI.Player a_p, bool isLocal = false)
    {
        cs_lobby.AddUnassignedPlayer(a_p.lobbyAvater);
        a_p.lobbyAvater.name = a_p.connectionId.ToString();

        TMP_InputField input = a_p.lobbyAvater.GetComponentInChildren<TMP_InputField>();
        Button readyButton = a_p.lobbyAvater.GetComponentInChildren<Button>();

        //Add a listener to the name input
        //When text has been entered then call OnNameChange
        input.onEndEdit.AddListener(delegate { OnNameChange(input, a_p); });

        //Add a listener to the ready button.
        //When the ready button has been pressed then tell the server
        //this client is ready
        readyButton.onClick.AddListener(delegate 
        {
            NetMsg_IsReadyLB readyLB = new NetMsg_IsReadyLB();
            readyLB.ConnectionID = a_p.connectionId;
            readyLB.IsReady = true;

            client.Send(readyLB);
        });

    }

    private void OnNameChange(TMP_InputField a_i, LLAPI.Player a_p)
    {
        NetMsg_NameChangeLB lb = new NetMsg_NameChangeLB();
        lb.ConnectionID = a_p.connectionId;
        lb.NewName = a_i.text;

        //Send the new data to the server to inform all other clients
        client.Send(lb);

        //Set the player name on the local client
        a_p.playerName = lb.NewName;
    }

    public void SetPlayerName(LLAPI.Player a_p)
    {
        a_p.lobbyAvater.GetComponentInChildren<TMP_InputField>().text = a_p.playerName;
    }

    public void SetPlayerTeam(LLAPI.Player a_p)
    {
        if(a_p.team == Team.Merc)
        {
            cs_lobby.AddMercPlayer(a_p.lobbyAvater);
            for(int i = 0; i < a_p.lobbyAvater.transform.childCount; ++i)
            {
                if(a_p.lobbyAvater.transform.GetChild(i).gameObject.tag == "playerColourIndication")
                {
                    a_p.lobbyAvater.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(142,23,23,255);
                }
            }
        }
        else if(a_p.team == Team.Spy)
        {
            cs_lobby.AddSpyPlayer(a_p.lobbyAvater);
            for (int i = 0; i < a_p.lobbyAvater.transform.childCount; ++i)
            {
                if (a_p.lobbyAvater.transform.GetChild(i).gameObject.tag == "playerColourIndication")
                {
                    a_p.lobbyAvater.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(51, 134, 160, 255);
                }
            }
        }
        else
        {
            cs_lobby.AddUnassignedPlayer(a_p.lobbyAvater);
        }
    }
}
