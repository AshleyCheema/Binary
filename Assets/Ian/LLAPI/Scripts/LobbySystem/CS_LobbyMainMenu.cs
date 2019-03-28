﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LLAPI;
using UnityEngine.SceneManagement;

public class CS_LobbyMainMenu : Singleton<CS_LobbyMainMenu>
{
    [SerializeField]
    private Button serverHost;
    [SerializeField]
    private TMP_InputField ipInput;
    [SerializeField]
    private Button connect;

    [SerializeField]
    RectTransform lobbyPanel;
    public RectTransform LobbyPanel
    { get { return lobbyPanel; } }

    private void Start()
    {
        // DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {

        CS_LobbyManager.Instance.Host.gameObject.SetActive(false);
        CS_LobbyManager.Instance.Client.gameObject.SetActive(true);

        string ip = ipInput.text;
        CS_LobbyManager.Instance.Client.ConnectToServer(ip);
        
        //CS_LobbyManager.Instance.Client.Connect(ip);
        CS_LobbyManager.Instance.ChangeTo(lobbyPanel);
    }

    public void ServerHost()
    {
        CS_LobbyManager.Instance.Host.gameObject.SetActive(true);
        CS_LobbyManager.Instance.Client.gameObject.SetActive(false);

        HostManager.Instance.StartNewServer();

        CS_LobbyManager.Instance.ChangeTo(lobbyPanel);
        //SceneManager.LoadScene(1);
    }
}
