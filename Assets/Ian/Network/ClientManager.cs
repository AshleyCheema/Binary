using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject gameOverUI;

    private long startLongTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ConnectToServer(string aIPAdress)
    {
        if(aIPAdress != "")
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
        if (SceneManager.GetActiveScene().name == "ClientGame")
        {
            //Spawn local avatar        
            mLocalPlayer.gameAvatar = SpawnPlayerObject(mLocalPlayer);
            Camera.main.GetComponent<CameraScript>().SetTarget(mLocalPlayer.gameAvatar.transform);
        }
        else if (SceneManager.GetActiveScene().name == "ClientLobby")
        {
            CS_LobbyManager.Instance.ChangeTo(CS_LobbyMainMenu.Instance.LobbyPanel);
            //destory the new network object
            DontDestroyOnLoad[] network = GameObject.FindObjectsOfType<DontDestroyOnLoad>();
            for (int i = 0; i < network.Length; i++)
            {
                if(!network[i].transform.GetChild(0).gameObject.activeInHierarchy && 
                    network[i].gameObject.name == "Network")
                {
                    Destroy(network[i].gameObject);
                }
            }

            //create all the players
            MiniModule_Lobby.Instance.OnLobbyPlayerAdd(mLocalPlayer, true);
            CS_Lobby.Instance.SetPlayerTeam(LocalPlayer);
            CS_Lobby.Instance.SendTeamChange(LocalPlayer);
        }
    }

    public void SetClientReady()
    {
        ClientScene.Ready(mLocalPlayer.conn);
        mLocalPlayer.isReady = true;
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

        GameObject go = Instantiate(
            MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[cso.ObjectID]);
        Players[cso.ConnectionID].gameAvatar = go;
        go.transform.position = cso.position;
        go.tag = Players[cso.ConnectionID].playerTeam == LLAPI.Team.Merc ? "Merc" : "Spy";

        MonoBehaviour[] allMonos = go.GetComponentsInChildren<MonoBehaviour>();

        go.GetComponentInChildren<FOWMask>().gameObject.SetActive(false);
        go.GetComponentInChildren<UIScript>().gameObject.SetActive(false);
        go.GetComponentInChildren<FOWAdaptiveRender>().gameObject.SetActive(false);
        go.GetComponent<MeshRenderer>().enabled = false;

        for (int i = 0; i < allMonos.Length; i++)
        {
            allMonos[i].enabled = false;
        }
    }

    public void OnReceivePlayerMoveChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientMove cm = aMsg.ReadMessage<Msg_ClientMove>();

        Players[cm.connectId].gameAvatar.transform.position = cm.position;
    }

    public void OnReceivePlayerRotationChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientRotation cm = aMsg.ReadMessage<Msg_ClientRotation>();

        if (Players[cm.connectId].gameAvatar != null)
        {
            Players[cm.connectId].gameAvatar.transform.rotation = cm.rot;
        }
    }

    public void OnReceivePlayerABFire(NetworkMessage aMsg)
    {
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
            GameObject go = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[cs.StunObjectIndex],
                Players[cs.ConnectionID].gameAvatar.transform.position, Quaternion.identity);
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
            tracker = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[ct.TrackerObjectIndex],
                ct.TrackerPosition, Quaternion.identity);

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
            if(ExitManager.Instance.Exits[i].ID == ce.ID)
            {
                ExitManager.Instance.Exits[i].IsOpen = ce.IsOpen;
                ExitManager.Instance.Exits[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }

    public void OnPlayerTrigger(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientTrigger ccp = aMsg.ReadMessage<Msg_ClientTrigger>();

        if(ccp.Type == TriggerType.Bullet)
        {
            LocalPlayer.gameAvatar.GetComponent<SpyController>().Shot();
        }
        else if(ccp.Type == TriggerType.Stun)
        {
            Debug.Log("I HAVE BEEN STUNNED. NO!!!!!!!!!!!!!!");
            //set isStunned to true
            if (mLocalPlayer.playerTeam == LLAPI.Team.Merc &&
                mLocalPlayer.gameAvatar.GetComponent<MercControls>())
            {
                mLocalPlayer.gameAvatar.GetComponent<MercControls>().IsStunned = true;
            }
        }
    }

    public void OnGameOver(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientGameOver cgo = aMsg.ReadMessage<Msg_ClientGameOver>();

        //enable the UI fade and show the game over screen 
        //ths is where all the state can be shown in need.
        if(gameOverUI)
        {
            gameOverUI.SetActive(true);
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
