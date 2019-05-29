using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LLAPI;
using UnityEngine.SceneManagement;

public class CS_LobbyMainMenu : Singleton<CS_LobbyMainMenu>
{
    [SerializeField]
    private TMP_InputField ipInput = null;

    [SerializeField]
    RectTransform mainPanel = null;

    [SerializeField]
    RectTransform lobbyPanel = null;
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

    public void Back()
    {
        if(HostManager.Instance != null)
        {
            HostManager.Instance.StopServer();
            HostManager.Instance.gameObject.SetActive(false);
        }
        else if(ClientManager.Instance != null)
        {
            ClientManager.Instance.StopClient();
            ClientManager.Instance.gameObject.SetActive(false);
        }


        CS_LobbyManager.Instance.ChangeTo(mainPanel);
    }
}
