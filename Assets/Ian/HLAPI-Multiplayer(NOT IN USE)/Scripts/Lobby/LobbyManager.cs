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
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkLobbyManager
{

    private static LobbyManager instance;
    public static LobbyManager Instance
    { get { return instance; } }

    [SerializeField]
    private LobbyTopPanel topPanel;
    public LobbyTopPanel TopPanel
    { get { return topPanel; } }

    [SerializeField]
    private RectTransform mainMenuPanel;

    [SerializeField]
    private RectTransform lobbyPanel;

    private RectTransform currentPanel;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private TextMeshProUGUI statusInfo;
    [SerializeField]
    private TextMeshProUGUI hostInfo;

    protected ulong currentMatchID;

    protected LobbyHook lobbyHooks;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        lobbyHooks = GetComponent<LobbyHook>();

        currentPanel = mainMenuPanel;

        backButton.gameObject.SetActive(false);
        GetComponent<Canvas>().enabled = true;

        DontDestroyOnLoad(gameObject);

        SetServerInfo("Offline", "None");
    }

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        if(SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            if(topPanel.IsInGame)
            {
                ChangeTo(lobbyPanel);
                if(conn.playerControllers[0].unetView.isClient)
                {
                    backDelegate = StopHostClbk;
                }
                else
                {
                    backDelegate = StopClientClbk;
                }
            }
            else
            {
                ChangeTo(mainMenuPanel);
            }
            topPanel.ToggleVisibility(true);
            topPanel.IsInGame = false;
        }
        else
        {
            ChangeTo(null);

            Destroy(GameObject.Find("MainMenu(CLone)"));

            topPanel.ToggleVisibility(false);
            topPanel.IsInGame = true;
        }
    }

    public void ChangeTo(RectTransform a_newPanel)
    {
        if(currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }

        if(a_newPanel != null)
        {
            a_newPanel.gameObject.SetActive(true);
        }

        currentPanel = a_newPanel;

        if(currentPanel != mainMenuPanel)
        {
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
            SetServerInfo("Offline", "None");
        }
    }

    public void SetServerInfo(string a_status, string a_host)
    {
        statusInfo.text = a_status;
        hostInfo.text = a_host;
    }

    public delegate void BackButtonDelegate();
    public BackButtonDelegate backDelegate;
    public void BackButton()
    {
        backDelegate();
        topPanel.IsInGame = false;
    }

    //---------------------------------------------------------------------//
    //CallBacks

    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    public void RemvoePlayer(LobbyPlayer a_player)
    {
        a_player.RemovePlayer();
    }

    public void SimpleClk()
    {
        ChangeTo(mainMenuPanel);
    }

    public void StopHostClbk()
    {
        StopHost();

        ChangeTo(mainMenuPanel);
    }

    public void StopClientClbk()
    {
        StopClient();

        ChangeTo(mainMenuPanel);
    }

    //---------------------------------------------------------------------//
    //Server

    /// <summary>
    /// 
    /// </summary>
    public override void OnStartHost()
    {
        base.OnStartHost();

        ChangeTo(lobbyPanel);
        backDelegate = StopHostClbk;
        SetServerInfo("Hosting", networkAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    /// <returns></returns>
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();

        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];

            if(p != null)
            {
            }
        }
        return obj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        if(lobbyHooks)
        {
            lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
        }
        return true;
    }

    //---------------------------------------------------------------------//
    //Client

    /// <summary>
    /// Client has connected to host
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        if(!NetworkServer.active)
        {
            ChangeTo(lobbyPanel);
            backDelegate = StopClientClbk;
            SetServerInfo("Client", networkAddress);
        }
    }

    /// <summary>
    /// Client has disconeted from host
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// Error with client connection to host
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="errorCode"></param>
    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        ChangeTo(mainMenuPanel);
    }
}
