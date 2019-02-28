/*
 * Author: Ian Hudson
 * Description: 
 * Created: 14/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class LobbyPlayer : NetworkLobbyPlayer
{
    [SerializeField]
    private TextMeshProUGUI nameInput;

    [SyncVar(hook = "OnMyName")]
    string playerName = "";

    [SerializeField]
    private Button waitingPlayer;
    [SerializeField]
    private Button readyPlayer;

    [SerializeField]
    private GameObject localIcon;

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        LobbyPlayerList.Instance.AddPlayer(this);

        if(isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }

        OnMyName(playerName);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        SetupLocalPlayer();
    }

    private void SetupOtherPlayer()
    {
        //nameInput.

        //readyPlayer.transform.GetChild(0).GetComponent<Text>().text = "...";
        readyPlayer.interactable = false;

        OnClientReady(false);
    }

    private void SetupLocalPlayer()
    {
        localIcon.SetActive(true);

        readyPlayer.transform.GetChild(0).GetComponent<Text>().text = "Join";
        readyPlayer.interactable = true;

        readyPlayer.onClick.RemoveAllListeners();
        readyPlayer.onClick.AddListener(OnReadyClicked);

        if(playerName == "")
        {
            CmdNameChanged("Player_Defulat");
        }
    }

    public override void OnClientReady(bool readyState)
    {
        if(readyState)
        {
            //Text text = transform.GetChild(0).GetComponent<Text>();
            //text.text = "Ready";
            readyPlayer.interactable = false;
        }
        else
        {
            //Text text = transform.GetChild(0).GetComponent<Text>();
            //text.text = isLocalPlayer ? "Join" : "...";
            readyPlayer.interactable = isLocalPlayer;
        }
    }

    public void OnMyName(string a_newName)
    {
        playerName = a_newName;
        nameInput.text = playerName;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnReadyClicked()
    {
        SendReadyToBeginMessage();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a_string"></param>
    public void OnNameChanged(string a_string)
    {
        CmdNameChanged(a_string);
    }

    [Command]
    public void CmdNameChanged(string a_name)
    {
        playerName = a_name;
    }

    private void OnDestroy()
    {
        LobbyPlayerList.Instance.RemovePlayer(this);
    }
}
