/*
 * Author: Ian Hudson
 * Description: 
 * Created: 14/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Net;

public class LobbyMainMenuPanel : MonoBehaviour
{
    //lobby Manager
    [SerializeField]
    private LobbyManager lobbyManager;

    //Lobby Panel
    [SerializeField]
    private RectTransform lobbyPanel;

    //ip input field
    [SerializeField]
    private TextMeshProUGUI ipInput;

    /// <summary>
    /// When script is enabled call this
    /// </summary>
    private void OnEnable()
    {
        lobbyManager.TopPanel.ToggleVisibility(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnClickHost()
    {
        lobbyManager.StartHost();
        Debug.Log(lobbyManager.isNetworkActive);
        Debug.Log(lobbyManager.networkPort);
        Debug.Log(lobbyManager.networkAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnClickJoin()
    {
        lobbyManager.ChangeTo(lobbyPanel);
        lobbyManager.networkAddress = "192.168.0.29";
        lobbyManager.StartClient();

        lobbyManager.backDelegate = lobbyManager.StopClientClbk;

        lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
    }
}
