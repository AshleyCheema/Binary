using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    /// <summary>
    /// Singleton
    /// </summary>
    private static LobbyManager instance;
    public static LobbyManager Instance => instance;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float matchStartCountDown = 5.0f;

    [SerializeField]
    private LobbyTopPanel topPanel;
    public LobbyTopPanel TopPanel => topPanel;

    /// <summary>
    /// Ref the Rect transform of the mainMenu panel
    /// </summary>
    [SerializeField]
    private RectTransform mainMenuPanel;
    public RectTransform MainMenuPanel => mainMenuPanel;

    /// <summary>
    /// Ref the Rect transform of the lobby panel
    /// </summary>
    [SerializeField]
    private RectTransform lobbyPanel;
    public RectTransform LobbyPanel => lobbyPanel;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private LobbyInfoPanel lobbyInfoPanel;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private LobbyCountdownPanel countDownPanel;
    public LobbyCountdownPanel CountdownPanel => countDownPanel;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject addplayerButton;

    protected RectTransform currentPanel;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text statusInfo;
    [SerializeField]
    private Text hostInfo;

    /// <summary>
    /// Store the number of players connected to the lobby
    /// </summary>
    private int numberOfPlayersConnected = 0;

    private bool isMatchmaking = false;
    public bool IsMatchmaking
    { get { return isMatchmaking; } set { isMatchmaking = value; } }

    protected bool disconnectServer = false;

    protected ulong currentMatchID;

    protected LobbyHook lobbyHooks;

    /// <summary>
    /// 
    /// </summary>
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
        GetComponent<Canvas>().enabled = true;

        DontDestroyOnLoad(gameObject);

        SetServerInfo("Offline", "Node");
    }

    /// <summary>
    /// The scene has changed for the lobby
    /// </summary>
    /// <param name="conn"></param>
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        //if the scene at 0 is the same as the lobby scene
        if(SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            if(topPanel.IsInGame)
            {
                ChangeTo(lobbyPanel);
                if(isMatchmaking)
                {
                    if(conn.playerControllers[0].unetView.isServer)
                    {
                        backDelegate = StopHostCallback;
                    }
                    else
                    {
                        backDelegate = StopClientCallback;
                    }
                }
                else
                {
                    if (conn.playerControllers[0].unetView.isClient)
                    {
                        backDelegate = StopHostCallback;
                    }
                    else
                    {
                        backDelegate = StopClientCallback;
                    }
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

            Destroy(GameObject.Find("MainMenuUI(Clone)"));

            topPanel.IsInGame = true;
            topPanel.ToggleVisibility(false);
        }
    }

    /// <summary>
    /// Change the scene which is being displayed
    /// </summary>
    /// <param name="a_newPanel"></param>
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
            isMatchmaking = false;
        }
    }

    /// <summary>
    /// Display the IsConnecting screen
    /// </summary>
    public void DisplayIsConnecting()
    {
        var thisObject = this;
        lobbyInfoPanel.Display("Connecting...", "Cancel", () => { thisObject.backDelegate(); });
    }

    /// <summary>
    /// Set the server information
    /// </summary>
    /// <param name="a_status"></param>
    /// <param name="a_host"></param>
    public void SetServerInfo(string a_status, string a_host)
    {
        statusInfo.text = a_status;
        hostInfo.text = a_host;
    }

    public delegate void BackButtonDelegate();
    public BackButtonDelegate backDelegate;

    /// <summary>
    /// 
    /// </summary>
    public void GoBackButton()
    {
        backDelegate();
        topPanel.IsInGame = false;
    }

    /// <summary>
    /// Try and add a player to the server/loby
    /// </summary>
    public void AddLocalPlayer()
    {
        TryToAddPlayer();
    }

    /// <summary>
    /// Remove a player from the server/lobby
    /// </summary>
    /// <param name="a_player"></param>
    public void RemovePlayer(LobbyPlayer a_player)
    {
        a_player.RemovePlayer();
    }

    /// <summary>
    /// /
    /// </summary>
    public void SimpleBackCallback()
    {
        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// 
    /// </summary>
    public void StopHostCallback()
    {
        if(isMatchmaking)
        {
            matchMaker.DestroyMatch((NetworkID)currentMatchID, 0, OnDestroyMatch);
            disconnectServer = true;
        }
        else
        {
            StopHost();
        }
    }

    /// <summary>
    /// Stop the client callback
    /// </summary>
    public void StopClientCallback()
    {
        StopClient();

        if(isMatchmaking)
        {
            StopMatchMaker();
        }

        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// Stop the server callback
    /// </summary>
    public void StopServerCallback()
    {
        StopServer();
        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// Kick a player from the server
    /// </summary>
    /// <param name="conn"></param>
    public void KickPlauer(NetworkConnection conn)
    {
        conn.Disconnect();
    }


    /// <summary>
    /// Call when someone starts hosing a server
    /// </summary>
    public override void OnStartHost()
    {
        base.OnStartHost();

        ChangeTo(lobbyPanel);
        backDelegate = StopHostCallback;
        SetServerInfo("Hosting", networkAddress);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="success"></param>
    /// <param name="extendedInfo"></param>
    /// <param name="matchInfo"></param>
    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        currentMatchID = (System.UInt64)matchInfo.networkId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="success"></param>
    /// <param name="extendedInfo"></param>
    public override void OnDestroyMatch(bool success, string extendedInfo)
    {
        base.OnDestroyMatch(success, extendedInfo);
        if(disconnectServer)
        {
            StopMatchMaker();
            StopHost();
        }
    }

    public void OnPlayersNumberModified(int a_count)
    {
        //Increase the number of players which are connected
        numberOfPlayersConnected += a_count;

        //the current number if local players connected
        int localPlayerCount = 0;
        //loop though all the local players
        foreach (UnityEngine.Networking.PlayerController p in ClientScene.localPlayers)
        {
            //if the loaclPlayer is null or it's controller is -1 the add 0 to localPlayerCount
            //else add 1
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;
        }
        //Set the add player button to true if there is space in the lobby
        addplayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && numberOfPlayersConnected < maxPlayers);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    /// <returns></returns>
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        //Create a new lobbyPlayerPrefab
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject);

        LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
        newPlayer.ToggleJoinButton(numberOfPlayersConnected + 1 >= minPlayers);

        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];

            if(p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numberOfPlayersConnected + 1 >= minPlayers);
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
        for (int i = 0; i < lobbySlots.Length; i++)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];

            if(p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        for (int i = 0; i < lobbySlots.Length; i++)
        {
            LobbyPlayer p = (LobbyPlayer)lobbySlots[i];

            if (p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers >= minPlayers);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lobbyPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns></returns>
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        if(lobbyHooks)
        {
            lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnLobbyServerPlayersReady()
    {
        bool allPlayersReady = true;
        for (int i = 0; i < lobbySlots.Length; i++)
        {
            if(lobbySlots[i] != null)
            {
                allPlayersReady &= lobbySlots[i].readyToBegin;
            }
        }

        if(allPlayersReady)
        {
            StartCoroutine(ServerCountDownCoroutine());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ServerCountDownCoroutine()
    {
        float remainingTime = matchStartCountDown;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while(remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            int newFloorTime = Mathf.FloorToInt(remainingTime);

            if(newFloorTime != floorTime)
            {
                floorTime = newFloorTime;

                for (int i = 0; i < lobbySlots.Length; i++)
                {
                    if(lobbySlots[i] != null)
                    {
                        (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                    }
                }
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        lobbyInfoPanel.gameObject.SetActive(false);

        //conn.RegisterHandler(Msg)

        if(!NetworkServer.active)
        {
            ChangeTo(lobbyPanel);
            backDelegate = StopClientCallback;
            SetServerInfo("Client", networkAddress);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ChangeTo(mainMenuPanel);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        ChangeTo(mainMenuPanel);
        lobbyInfoPanel.Display("Client error: " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
    }
}

