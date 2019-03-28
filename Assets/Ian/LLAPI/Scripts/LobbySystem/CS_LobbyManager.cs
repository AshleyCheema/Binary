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
    private ClientManager client;
    public ClientManager Client
    { get { return client; } set { client = value; } }

    [SerializeField]
    private HostManager host;
    public HostManager Host
    { get { return host; } set { host = value; } }

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

    public void SetShell(LocalPlayer aPlayer)
    {
        aPlayer.lobbyAvatar.GetComponentInChildren<TMP_InputField>().interactable = false;
        Button[] buttons = aPlayer.lobbyAvatar.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

    }

    public void AddLobbyPlayer(LocalPlayer aPlayer, bool isLocal = false)
    {
        SetPlayerTeam(aPlayer);
        aPlayer.lobbyAvatar.name = aPlayer.connectionId.ToString();

        if(!isLocal)
        {
            SetShell(aPlayer);
            return;
        }

        TMP_InputField input = aPlayer.lobbyAvatar.GetComponentInChildren<TMP_InputField>();
        Button readyButton = aPlayer.lobbyAvatar.GetComponentInChildren<Button>();

        //Add a listener to the name input
        //When text has been entered then call OnNameChange
        input.onEndEdit.AddListener(delegate { OnNameChange(input, aPlayer); });

        //Add a listener to the ready button.
        //When the ready button has been pressed then tell the server
        //this client is ready
        readyButton.onClick.AddListener(delegate 
        {
            ClientManager.Instance.SetClientReady();
            //NetMsg_IsReadyLB readyLB = new NetMsg_IsReadyLB();
            //readyLB.ConnectionID = a_p.connectionId;
            //readyLB.IsReady = true;
            //
            //client.Send(readyLB);
        });

    }

    private void OnNameChange(TMP_InputField a_i, LocalPlayer aPlayer)
    {
        Msg_ClientNameChange nc = new Msg_ClientNameChange();
        nc.connectionID = aPlayer.connectionId;
        nc.name = a_i.text;
        ClientManager.Instance.client.Send(MSGTYPE.LOBBY_NAME_CHANGE, nc);
        //NetMsg_NameChangeLB lb = new NetMsg_NameChangeLB();
        //lb.ConnectionID = a_p.connectionId;
        //lb.NewName = a_i.text;

        //Send the new data to the server to inform all other clients
        //client.Send(lb);

        //Set the player name on the local client
        SetName(a_i, aPlayer);
    }

    public void SetName(TMP_InputField a_i, LocalPlayer aPlayer)
    {
        aPlayer.playerName = a_i.name;
    }

    public void SetPlayerName(LocalPlayer aPlayer)
    {
        aPlayer.lobbyAvatar.GetComponentInChildren<TMP_InputField>().text = aPlayer.playerName;
    }

    public void SetPlayerTeam(LocalPlayer aPlayer)
    {
        if(aPlayer.playerTeam == Team.Merc)
        {
            cs_lobby.AddMercPlayer(aPlayer.lobbyAvatar);
            for(int i = 0; i < aPlayer.lobbyAvatar.transform.childCount; ++i)
            {
                if(aPlayer.lobbyAvatar.transform.GetChild(i).gameObject.tag == "playerColourIndication")
                {
                    aPlayer.lobbyAvatar.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(142,23,23,255);
                }
            }
        }
        else if(aPlayer.playerTeam == Team.Spy)
        {
            cs_lobby.AddSpyPlayer(aPlayer.lobbyAvatar);
            for (int i = 0; i < aPlayer.lobbyAvatar.transform.childCount; ++i)
            {
                if (aPlayer.lobbyAvatar.transform.GetChild(i).gameObject.tag == "playerColourIndication")
                {
                    aPlayer.lobbyAvatar.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(51, 134, 160, 255);
                }
            }
        }
        else
        {
            cs_lobby.AddUnassignedPlayer(aPlayer.lobbyAvatar);
        }
    }
}
