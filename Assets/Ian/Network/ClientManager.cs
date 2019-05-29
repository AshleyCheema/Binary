using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class LocalPlayer
{
    public int connectionId;
    public string playerName;
    public LLAPI.Team playerTeam;
    public GameObject lobbyAvatar;
    public GameObject gameAvatar;
    public List<GameObject> gameObjects;
    public NetworkConnection conn;
    public bool isReady;
    public Vector3 latestPosition;
}

public class ClientManager : NetworkManager
{
    private static ClientManager instance;
    public static ClientManager Instance
    { get { return instance; } }

    private LocalPlayer mLocalPlayer;
    public LocalPlayer LocalPlayer
    { get { return mLocalPlayer; } }

    private Dictionary<int, LocalPlayer> players = new Dictionary<int, LocalPlayer>();
    public Dictionary<int, LocalPlayer> Players
    { get { return players; } }

    private Dictionary<int, GameObject> capturePoints = new Dictionary<int, GameObject>();

    [SerializeField]
    private GameObject gameOverUI;

    private long startLongTime;

    private string leveToLoad = "NewLevel";

    [SerializeField]
    private RuntimeAnimatorController spyShellAnim;
    private RuntimeAnimatorController spyShellAnim1;
    [SerializeField]
    private RuntimeAnimatorController mercShellAnim;

    [SerializeField]
    private float movementSmooth = 1.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (players.Count > 0)
        {
            foreach (LocalPlayer lp in players.Values)
            {
                if (lp.gameAvatar != null)
                {
                    lp.gameAvatar.transform.position = Vector3.Lerp(lp.gameAvatar.transform.position, lp.latestPosition, Time.deltaTime * movementSmooth);
                }
            }
        }
    }

    public void ConnectToServer(string aIPAdress)
    {
        if (aIPAdress != "")
        {
            networkAddress = aIPAdress;
        }
        StartClient();
        this.client.RegisterHandler(MSGTYPE.ADD_NEW_LOBBY_PLAYER, OnReceiveNewPlayerLobby);
        this.client.RegisterHandler(MSGTYPE.LOBBY_TEAM_CHANGE, OnReceivePlayerChangeTeam);
        this.client.RegisterHandler(MSGTYPE.LOBBY_NAME_CHANGE, OnReceivePlayerChangeName);
        this.client.RegisterHandler(MSGTYPE.CLIENT_SPAWN_OBJECT, OnReceiveSpawnOtherPlayer);
        this.client.RegisterHandler(MSGTYPE.CLIENT_MOVE, OnReceivePlayerMoveChange);
        this.client.RegisterHandler(MSGTYPE.CLIENT_ROTATION, OnReceivePlayerRotationChange);
        this.client.RegisterHandler(MSGTYPE.CLIENT_AB_FIRE, OnReceivePlayerABFire);
        this.client.RegisterHandler(MSGTYPE.CLIENT_AB_STUN, OnReceivePlayerABStun);
        this.client.RegisterHandler(MSGTYPE.CLIENT_AB_TRACKER, OnPlayerABTracker);
        this.client.RegisterHandler(MSGTYPE.CLIENT_EXIT, OnServerExitOpen);
        this.client.RegisterHandler(MSGTYPE.CLIENT_AB_TRIGGER, OnPlayerTrigger);
        this.client.RegisterHandler(MSGTYPE.CLIENT_GAME_OVER, OnGameOver);
        this.client.RegisterHandler(MSGTYPE.CLIENT_CAPTURE_POINT_INCREASE, OnSpyCaptureIncrease);
        this.client.RegisterHandler(MSGTYPE.CLIENT_CAPTURE_POINT, OnSpyStartCapture);
        this.client.RegisterHandler(MSGTYPE.CLIENT_FEEDBACK, OnReceivePlayerFeedback);
        this.client.RegisterHandler(MSGTYPE.CLIENT_ANIM_CHANGE, OnReceiveClientAnim);
        this.client.RegisterHandler(MSGTYPE.CLIENT_EXIT_AVAL, OnReceiveClientExtiAval);

        this.client.RegisterHandler(MSGTYPE.PING_PONG, OnPingPong);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("This client has connect to the server");

        mLocalPlayer = new LocalPlayer();
        mLocalPlayer.connectionId = -1;
        mLocalPlayer.playerTeam = LLAPI.Team.Unassigned;
        mLocalPlayer.conn = conn;

        MiniModule_Lobby.Instance.OnLobbyPlayerAdd(mLocalPlayer, true);

        //Msg_PingPong pp = new Msg_PingPong();
        //pp.connectId = mLocalPlayer.connectionId;
        //pp.timeStamp = DateTime.UtcNow.Millisecond;
        //startLongTime = pp.timeStamp;
        //this.client.Send(MSGTYPE.PING_PONG, pp);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == leveToLoad)
        {
            gameOverUI.SetActive(false);
            //Spawn local avatar        
            mLocalPlayer.gameAvatar = SpawnPlayerObject(mLocalPlayer);
            Camera.main.GetComponent<CameraScript>().SetTarget(mLocalPlayer.gameAvatar.transform.GetChild(0), 
                                                               mLocalPlayer.gameAvatar.transform);

            NO_CapturePoint[] capturePoints = GameObject.FindObjectsOfType<NO_CapturePoint>();

            for (int i = 0; i < capturePoints.Length; i++)
            {
                if (this.capturePoints.ContainsKey(capturePoints[i].ID) == false)
                {
                    this.capturePoints.Add(capturePoints[i].ID, capturePoints[i].gameObject);
                }
                else if (this.capturePoints[capturePoints[i].ID] == null)
                {
                    this.capturePoints[capturePoints[i].ID] = capturePoints[i].gameObject;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "ClientLobby")
        {
            CS_LobbyManager.Instance.Client = this;
            CS_LobbyManager.Instance.ChangeTo(CS_LobbyMainMenu.Instance.LobbyPanel);
            //destory the new network object
            DontDestroyOnLoad[] network = GameObject.FindObjectsOfType<DontDestroyOnLoad>();
            for (int i = 0; i < network.Length; i++)
            {
                if (!network[i].transform.GetChild(0).gameObject.activeInHierarchy &&
                    network[i].gameObject.name == "Network")
                {
                    Destroy(network[i].gameObject);
                }
            }

            //create all the players
            MiniModule_Lobby.Instance.OnLobbyPlayerAdd(mLocalPlayer, true);
            CS_Lobby.Instance.SetPlayerTeam(LocalPlayer);

            foreach (var v in Players.Values)
            {
                MiniModule_Lobby.Instance.OnLobbyPlayerAdd(v);
                CS_Lobby.Instance.SetPlayerTeam(v);

                foreach (var item in v.lobbyAvatar.GetComponentsInChildren<TextMeshProUGUI>())
                {
                    if (item.text == "Enter name...")
                    {
                        item.text = "";
                        break;
                    }
                }
            }

            mLocalPlayer.playerName = "";
        }
    }

    public void SetClientReady()
    {
        if(mLocalPlayer.playerName == "" || mLocalPlayer.playerName == null)
        {
            return;
        }

        //ClientScene.Ready(mLocalPlayer.conn);
        mLocalPlayer.isReady = true;

        Msg_ClientReady cr = new Msg_ClientReady();
        cr.connectId = LocalPlayer.connectionId;
        client.Send(MSGTYPE.CLIENT_READY, cr);
    }

    private GameObject SpawnPlayerObject(LocalPlayer aPlayer)
    {
        Vector3 spawnPosition = Vector3.zero;
        GameObject go;
        if (mLocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            BoxCollider bc = GameObject.FindWithTag("Merc_Spawn_Area").GetComponent<BoxCollider>();
            spawnPosition = ExtensionFunctions.RandomPointInBounds(bc.bounds);
        }
        else if (mLocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            BoxCollider bc = GameObject.FindWithTag("Spy_Spawn_Area").GetComponent<BoxCollider>();
            spawnPosition = ExtensionFunctions.RandomPointInBounds(bc.bounds);
        }

        go = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[(aPlayer.playerTeam == LLAPI.Team.Merc) ? 0 : 5],
            spawnPosition, Quaternion.identity);
        go.tag = (aPlayer.playerTeam == LLAPI.Team.Merc) ? "Merc" : "Spy";

        return go;
    }

    private void SpawnOtherPlayerObject(LocalPlayer aPlayer)
    {
        if (Players[aPlayer.connectionId].gameAvatar == null)
        {
            GameObject go = Instantiate(
                MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[aPlayer.playerTeam == LLAPI.Team.Merc ? 0 : 5]);
            Players[aPlayer.connectionId].gameAvatar = go;
            go.tag = Players[aPlayer.connectionId].playerTeam == LLAPI.Team.Merc ? "Merc" : "Spy";
            
            MonoBehaviour[] allMonos = go.GetComponentsInChildren<MonoBehaviour>();

            go.GetComponentInChildren<FOWMask>().gameObject.SetActive(false);
            go.GetComponentInChildren<UIScript>().gameObject.SetActive(false);
            //go.GetComponentInChildren<FOWAdaptiveRender>().gameObject.SetActive(false);
            go.GetComponentInChildren<AudioListener>().enabled = false;
            go.AddComponent<ShellAnimationController>();
            if (go.tag == "Merc")
            {
                //go.transform.GetChild(0).GetChild(7).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                //go.transform.GetChild(0).GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }

            for (int i = 0; i < allMonos.Length; i++)
            {
                allMonos[i].enabled = false;
            }
            go.GetComponentInChildren<AudioEvents>().enabled = true;
        }
    }

    public void OnReceiveNewPlayerLobby(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientConnection cc = aMsg.ReadMessage<Msg_ClientConnection>();

        if (cc.connectID != mLocalPlayer.connectionId && mLocalPlayer.connectionId != -1)
        {
            LocalPlayer localPlayer = new LocalPlayer();
            localPlayer.connectionId = (int)cc.connectID;
            localPlayer.playerTeam = LLAPI.Team.Unassigned;
            localPlayer.gameObjects = new List<GameObject>();
            Players.Add(localPlayer.connectionId, localPlayer);


            MiniModule_Lobby.Instance.OnLobbyPlayerAdd(localPlayer);

            foreach (var item in localPlayer.lobbyAvatar.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (item.text == "Enter name...")
                {
                    item.text = "";
                    break;
                }
            }
        }
        else
        {
            mLocalPlayer.connectionId = (int)cc.connectID;
        }
    }

    public void OnReceivePlayerChangeTeam(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientTeamChange ctc = aMsg.ReadMessage<Msg_ClientTeamChange>();
        if (ctc.ConnectionID != mLocalPlayer.connectionId)
        {
            Players[ctc.ConnectionID].playerTeam = ctc.Team;

            CS_Lobby.Instance.SetPlayerTeam(Players[ctc.ConnectionID]);
        }
    }

    public void OnReceivePlayerChangeName(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientNameChange cnc = aMsg.ReadMessage<Msg_ClientNameChange>();
        if (cnc.connectionID != mLocalPlayer.connectionId)
        {
            Players[cnc.connectionID].playerName = cnc.name;

            CS_LobbyManager.Instance.SetPlayerName(Players[cnc.connectionID]);
        }
    }

    public void OnReceiveSpawnOtherPlayer(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientSpawnObject cso = aMsg.ReadMessage<Msg_ClientSpawnObject>();
        if (Players[cso.ConnectionID].gameAvatar == null)
        {
            GameObject go = Instantiate(
                MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[cso.ObjectID]);
            Players[cso.ConnectionID].gameAvatar = go;
            go.transform.position = cso.position != new Vector3(0, 0, 0) ? cso.position : new Vector3(0, 25, 0);
            go.tag = Players[cso.ConnectionID].playerTeam == LLAPI.Team.Merc ? "Merc" : "Spy";

            MonoBehaviour[] allMonos = go.GetComponentsInChildren<MonoBehaviour>();

            go.GetComponentInChildren<FOWMask>().gameObject.SetActive(false);
            go.GetComponentInChildren<UIScript>().gameObject.SetActive(false);
            //go.GetComponentInChildren<FOWAdaptiveRender>().gameObject.SetActive(false);
            go.GetComponentInChildren<AudioListener>().enabled = false;
            go.AddComponent<ShellAnimationController>();

            if (go.tag == "Merc")
            {
                //go.transform.GetChild(0).GetChild(7).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
            else
            {
                //go.transform.GetChild(0).GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }

            for (int i = 0; i < allMonos.Length; i++)
            {
                allMonos[i].enabled = false;
            }
            go.GetComponentInChildren<AudioEvents>().enabled = true;

        }
    }

    public void OnReceivePlayerMoveChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientMove cm = aMsg.ReadMessage<Msg_ClientMove>();

        //int mil = DateTime.UtcNow.Millisecond;
        //Debug.Log(mil - cm.Time);

        if (Players[cm.connectId].gameAvatar != null)
        {
            //Players[cm.connectId].gameAvatar.transform.position = Vector3.Lerp(Players[cm.connectId].gameAvatar.transform.position,
            //                                                                   cm.position, 0.5f);
            Players[cm.connectId].latestPosition = cm.position;
        }
        else
        {
            //create the game object
            SpawnOtherPlayerObject(Players[cm.connectId]);
            Players[cm.connectId].gameAvatar.transform.position = Vector3.Lerp(Players[cm.connectId].gameAvatar.transform.position,
                                                                            cm.position, 0.5f);
        }
    }

    public void OnReceivePlayerRotationChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientRotation cm = aMsg.ReadMessage<Msg_ClientRotation>();

        if (Players[cm.connectId].gameAvatar != null)
        {
            Players[cm.connectId].gameAvatar.transform.rotation = cm.rot;
        }
        else
        {
            //create the game object
            SpawnOtherPlayerObject(Players[cm.connectId]);
            Players[cm.connectId].gameAvatar.transform.rotation = cm.rot;
        }
    }

    public void OnReceivePlayerABFire(NetworkMessage aMsg)
    {
        return;
        aMsg.reader.SeekZero();
        Msg_AB_ClientFire cf = aMsg.ReadMessage<Msg_AB_ClientFire>();

        GameObject bullet = null;
        for (int i = 0; i < players[cf.ConnectId].gameObjects.Count; i++)
        {
            if (players[cf.ConnectId].gameObjects[i] == null)
            {
                players[cf.ConnectId].gameObjects.RemoveAt(i);
            }
            else if (players[cf.ConnectId].gameObjects[i].name == "Merc_Bullet")
            {
                bullet = players[cf.ConnectId].gameObjects[i];
                break;
            }
        }

        if (bullet == null)
        {
            GameObject go = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[cf.BulletObjectIndex],
                cf.BulletPosition, Quaternion.identity);
            go.GetComponent<Rigidbody>().velocity = cf.BulletVelocity;
            go.GetComponent<Trigger>().IsSpawned = true;
            go.name = "Merc_Bullet";
            Players[cf.ConnectId].gameObjects.Add(go);
        }
        else
        {
            bullet.transform.position = cf.BulletPosition;
            bullet.GetComponent<Rigidbody>().velocity = cf.BulletVelocity;
        }
    }

    public void OnReceivePlayerABStun(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_AB_ClientStun cs = aMsg.ReadMessage<Msg_AB_ClientStun>();

        GameObject stun = null;
        for (int i = 0; i < players[cs.ConnectionID].gameObjects.Count; i++)
        {
            if (players[cs.ConnectionID].gameObjects[i] == null)
            {
                players[cs.ConnectionID].gameObjects.RemoveAt(i);
            }
            else if (players[cs.ConnectionID].gameObjects[i].name == "Merc_Stun")
            {
                stun = players[cs.ConnectionID].gameObjects[i];
                break;
            }
        }

        if (stun == null)
        {
            GameObject go = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[cs.StunObjectIndex]);
            go.transform.position = Players[cs.ConnectionID].gameAvatar.transform.position;
            go.GetComponent<StunAbility>().isSpawned = true;
            go.GetComponent<StunAbility>().stunDropped = true;
            go.GetComponent<StunAbility>().SetShell();
            go.GetComponent<Trigger>().IsSpawned = true;
            go.GetComponent<Rigidbody>().isKinematic = true;
            go.GetComponent<Rigidbody>().useGravity = false;
            go.GetComponent<MeshRenderer>().enabled = true;

            go.name = "Merc_Stun";

            Players[cs.ConnectionID].gameObjects.Add(go);
        }
        else
        {
            stun.transform.position = Players[cs.ConnectionID].gameAvatar.transform.position;
            //stun.GetComponent<StunAbility>().Play();
        }
    }

    public void OnPlayerABTracker(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_Client_AB_Tracker ct = aMsg.ReadMessage<Msg_Client_AB_Tracker>();

        GameObject tracker = null;
        for (int i = 0; i < players[ct.ConnectionID].gameObjects.Count; i++)
        {
            if (players[ct.ConnectionID].gameObjects[i] == null)
            {
                players[ct.ConnectionID].gameObjects.RemoveAt(i);
            }
            else if (players[ct.ConnectionID].gameObjects[i].name == "Merc_Tracker")
            {
                tracker = players[ct.ConnectionID].gameObjects[i];
                break;
            }
        }

        if (tracker == null)
        {
            tracker = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[ct.TrackerObjectIndex]);
            tracker.transform.position = ct.TrackerPosition;
            tracker.GetComponent<Trigger>().IsSpawned = true;
            tracker.GetComponent<Trigger>().enabled = false;
            tracker.name = "Merc_Tracker";

            Players[ct.ConnectionID].gameObjects.Add(tracker);
        }
        else
        {
            tracker.transform.position = ct.TrackerPosition;
        }

        if (ct.TrackerTriggered)
        {
            tracker.SetActive(true);
        }
        else
        {
            tracker.SetActive(false);
        }
    }

    public void OnServerExitOpen(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientExit ce = aMsg.ReadMessage<Msg_ClientExit>();

        for (int i = 0; i < ExitManager.Instance.Exits.Length; i++)
        {
            if (ExitManager.Instance.Exits[i].ID == ce.ID)
            {
                ExitManager.Instance.Exits[i].IsOpen = ce.IsOpen;
            }
        }
    }

    public void OnPlayerTrigger(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientTrigger ccp = aMsg.ReadMessage<Msg_ClientTrigger>();

        if (ccp.Type == TriggerType.Bullet)
        {
            LocalPlayer.gameAvatar.transform.GetChild(0).GetComponent<SpyController>().Shot();
        }
        else if (ccp.Type == TriggerType.Stun)
        {
            Debug.Log("I HAVE BEEN STUNNED. NO!!!!!!!!!!!!!!");
            //set isStunned to true
            if (mLocalPlayer.playerTeam == LLAPI.Team.Merc &&
                mLocalPlayer.gameAvatar.transform.GetChild(0).gameObject.GetComponent<MercControls>())
            {
                mLocalPlayer.gameAvatar.transform.GetChild(0).gameObject.GetComponent<MercControls>().IsStunned = true;
            }
        }
    }

    public void OnGameOver(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientGameOver cgo = aMsg.ReadMessage<Msg_ClientGameOver>();

        if (mLocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            PlayerStats.Instance.HasWon = !cgo.spiesWon;
        }
        else
        {
            PlayerStats.Instance.HasWon = cgo.spiesWon;
        }

        Debug.Log("Player Name: " + PlayerStats.Instance.PlayerName);
        Debug.Log("Player Team: " + PlayerStats.Instance.PlayerTeam);
        Debug.Log("Has Won: " + PlayerStats.Instance.HasWon);
        Debug.Log("Steps: " + PlayerStats.Instance.Steps);
        Debug.Log("Shots: " + PlayerStats.Instance.ShotsFired);
        Debug.Log("Has Abililites: " + PlayerStats.Instance.AbililitesUsed);
        Debug.Log("Capture Amount: " + PlayerStats.Instance.CaptureedAmount);

        //set data to upload to database
        DB_UpdatePlayer updateDB = FindObjectOfType<DB_UpdatePlayer>();
        updateDB.playerName = PlayerStats.Instance.PlayerName;
        updateDB.matchResult = PlayerStats.Instance.HasWon == true ? "win" : "lose";
        updateDB.playerClass = PlayerStats.Instance.PlayerTeam == LLAPI.Team.Merc ? "merc" : "spy";
        updateDB.stepsTaken = PlayerStats.Instance.Steps;
        updateDB.shotsFired = PlayerStats.Instance.ShotsFired;
        updateDB.ablitiesUsed = PlayerStats.Instance.AbililitesUsed;
        updateDB.pointsCaptured = (int)PlayerStats.Instance.CaptureedAmount;
       // updateDB.CallUpdatePlayerStats();

        //enable the UI fade and show the game over screen 
        //ths is where all the state can be shown in need.
        if (gameOverUI)
        {
            gameOverUI.SetActive(true);

            string result = (PlayerStats.Instance.HasWon == true) ? "Won" : "Lost";
            if (LocalPlayer.playerTeam == LLAPI.Team.Merc)
            {
                gameOverUI.transform.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Name - " + PlayerStats.Instance.PlayerName;
                gameOverUI.transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Team - " + PlayerStats.Instance.PlayerTeam;
                gameOverUI.transform.GetChild(2).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Result - " + result;
                gameOverUI.transform.GetChild(2).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Steps Taken - " + PlayerStats.Instance.Steps.ToString();
                gameOverUI.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Shots Fired - " + PlayerStats.Instance.ShotsFired.ToString();
                gameOverUI.transform.GetChild(2).transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Abilities Used - " + PlayerStats.Instance.AbililitesUsed.ToString();
            }
            else
            {
                gameOverUI.transform.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Name - " + PlayerStats.Instance.PlayerName;
                gameOverUI.transform.GetChild(2).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Team - " + PlayerStats.Instance.PlayerTeam;
                gameOverUI.transform.GetChild(2).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Result - " + result;
                gameOverUI.transform.GetChild(2).transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Steps Taken - " + PlayerStats.Instance.Steps.ToString();
                gameOverUI.transform.GetChild(2).transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Abilities Used - " + PlayerStats.Instance.AbililitesUsed.ToString();
                gameOverUI.transform.GetChild(2).transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Data Stolen - " + PlayerStats.Instance.CaptureedAmount.ToString();
            }
        }
    }

    public void OnSpyCaptureIncrease(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientCapturePointIncrease ccpi = aMsg.ReadMessage<Msg_ClientCapturePointIncrease>();
        capturePoints[ccpi.NOIndex].GetComponent<NO_CapturePoint>().IncreaseCaptureAmount(false);
    }

    public void OnSpyStartCapture(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientCapaturePoint ccp = aMsg.ReadMessage<Msg_ClientCapaturePoint>();

        capturePoints[ccp.ID].GetComponent<NO_CapturePoint>().IsBeingCaptured = ccp.IsBeingCaptured;
    }

    public void OnReceivePlayerFeedback(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientMercFeedback cmf = aMsg.ReadMessage<Msg_ClientMercFeedback>();

        //should be merc player only
        //do something as we have been notified of somethig
        LocalPlayer.gameAvatar?.transform.GetChild(0).gameObject.GetComponent<TrackerAbility>().SetFeedback(cmf.Location);
    }

    public void OnReceiveClientAnim(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientAnimChange cac = aMsg.ReadMessage<Msg_ClientAnimChange>();
        
        if (Players[cac.connectId].gameAvatar != null)
        {
            if (Players[cac.connectId].gameAvatar.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController != spyShellAnim ||
                Players[cac.connectId].gameAvatar.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController != mercShellAnim)
            {
                if (Players[cac.connectId].playerTeam == LLAPI.Team.Merc)
                {
                    Players[cac.connectId].gameAvatar.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController = mercShellAnim;
                }
                else
                {
                    Players[cac.connectId].gameAvatar.transform.GetChild(0).gameObject.GetComponent<Animator>().runtimeAnimatorController = spyShellAnim;
                }
            }
            Players[cac.connectId].gameAvatar.transform.GetChild(0).gameObject.GetComponent<Animator>().Play(cac.hash);
        }
    }

    public void OnReceiveClientExtiAval(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientExitAval cea = aMsg.ReadMessage<Msg_ClientExitAval>();

        NO_Exit[] exits = FindObjectsOfType<NO_Exit>();
        for (int i = 0; i < exits.Length; i++)
        {
            if(exits[i].ID == cea.ExitID)
            {
                exits[i].ExitOpen = true;
            }
        }
    }

    public void OnPingPong(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_PingPong pp = aMsg.ReadMessage<Msg_PingPong>();

        Debug.Log("Client got message from server: " + (DateTime.UtcNow.Millisecond - pp.timeStamp));

        Debug.Log("Total time was: " + (startLongTime - DateTime.UtcNow.Millisecond));
    }

    public void Send(LLAPI.NetMsg amesh, int a_channel = -1)
    {

    }
}
