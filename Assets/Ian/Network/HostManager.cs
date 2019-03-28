using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MSGTYPE
{
    public const short ADD_NEW_LOBBY_PLAYER = 100;
    public const short LOBBY_TEAM_CHANGE = 101;
    public const short LOBBY_NAME_CHANGE = 102;
    public const short CLIENT_SPAWN_OBJECT = 103;
    public const short CLIENT_MOVE = 104;
    public const short CLIENT_ROTATION = 105;
    public const short CLIENT_AB_FIRE = 106;
    public const short CLIENT_AB_STUN = 107;
    public const short CLIENT_AB_TRACKER = 108;
    public const short CLIENT_CAPTURE_POINT = 109;
    public const short CLIENT_EXIT = 110;
    public const short CLIENT_AB_TRIGGER = 111;
    public const short CLIENT_EXITED_LEVEL = 112;
    public const short PING_PONG = 250;
}

public class HostManager : NetworkManager
{
    private static HostManager instance;
    public static HostManager Instance
    { get { return instance; } }

    private Dictionary<int, LocalPlayer> players = new Dictionary<int, LocalPlayer>();
    public Dictionary<int, LocalPlayer> Players
    { get { return players; } }

    private Dictionary<int, GameObject> capturePoints = new Dictionary<int, GameObject>();

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

    public void StartNewServer()
    {
        StartServer();

        Debug.Log("Server stated");

        NetworkServer.RegisterHandler(MSGTYPE.LOBBY_TEAM_CHANGE, OnPlayerTeamChange);
        NetworkServer.RegisterHandler(MSGTYPE.LOBBY_NAME_CHANGE, OnPlayerNameChange);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_MOVE, OnPlayerMoveChange);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_ROTATION, OnPlayerRotationChange);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_ROTATION, OnPlayerRotationChange);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_AB_FIRE, OnPlayerABFire);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_AB_STUN, OnPlayerABStun);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_AB_TRACKER, OnPlayerABTracker);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_CAPTURE_POINT, OnPlayerCapturePoint);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_AB_TRIGGER, OnPlayerTrigger);
        NetworkServer.RegisterHandler(MSGTYPE.CLIENT_EXITED_LEVEL, OnSpyExitedLevel);

        //NetworkServer.RegisterHandler(MSGTYPE.PING_PONG, OnPingPong);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        Debug.Log("Client has connected to this server");

        LocalPlayer mLocalPlayer = new LocalPlayer();
        mLocalPlayer.connectionId = conn.connectionId;
        mLocalPlayer.gameObjects = new List<GameObject>();
        mLocalPlayer.conn = conn;

        Players.Add(conn.connectionId, mLocalPlayer);

        MiniModule_Lobby.Instance.OnLobbyPlayerAdd(mLocalPlayer);

        Msg_ClientConnection cc = new Msg_ClientConnection();
        cc.connectID = conn.connectionId;

        NetworkServer.SendToClient(conn.connectionId, MSGTYPE.ADD_NEW_LOBBY_PLAYER, cc);

        //tell all other clients about the new client
        Send(conn.connectionId, MSGTYPE.ADD_NEW_LOBBY_PLAYER, cc, false);

        foreach (var item in Players.Keys)
        {
            if(item != conn.connectionId)
            {
                cc.connectID = (int)item;
                Send(conn.connectionId, MSGTYPE.ADD_NEW_LOBBY_PLAYER, cc);
            }
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client has disconnected: " + conn.connectionId);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        Debug.Log("Client: " + conn.connectionId + " is ready.");
        Players[conn.connectionId].isReady = true;

        bool ready = true;

        foreach (var v in Players.Values)
        {
            if(!v.isReady)
            {
                ready = false;
                break;
            }
        }

        if(ready)
        {
            ServerChangeScene("ClientGame");
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "ClientGame")
        {

            NO_CapturePoint[] capturePoints = GameObject.FindObjectsOfType<NO_CapturePoint>();

            for (int i = 0; i < capturePoints.Length; i++)
            {
                this.capturePoints.Add(capturePoints[i].ID, capturePoints[i].gameObject);
            }

            Debug.Log("Add all player shells");
            Camera.main.transform.position = new Vector3(-16, 100, -13);
            Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
            Camera.main.GetComponent<Camera>().orthographicSize = 53;

            foreach (var pKey in Players.Keys)
            {
                foreach (var cKey in Players.Keys)
                {
                    if (cKey != pKey)
                    {
                        Msg_ClientSpawnObject cpo = new Msg_ClientSpawnObject();
                        cpo.ConnectionID = cKey;
                        cpo.ObjectID = Players[cKey].playerTeam == LLAPI.Team.Merc ? 0 : 5;

                        Vector3 spawnPos = Vector3.zero;

                        if (cpo.ObjectID == 0)
                        {
                            BoxCollider bc = GameObject.FindWithTag("Merc_Spawn_Area").GetComponent<BoxCollider>();
                            spawnPos = ExtensionFunctions.RandomPointInBounds(bc.bounds);
                        }
                        else
                        {
                            BoxCollider bc = GameObject.FindWithTag("Spy_Spawn_Area").GetComponent<BoxCollider>();
                            spawnPos = ExtensionFunctions.RandomPointInBounds(bc.bounds);
                        }

                        cpo.position = spawnPos;

                        Send(pKey, MSGTYPE.CLIENT_SPAWN_OBJECT, cpo);
                    }
                }

                GameObject go = Instantiate(
                    MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[Players[pKey].playerTeam == LLAPI.Team.Merc ? 0 : 5]);
                BoxCollider bcoll = GameObject.FindWithTag("Spy_Spawn_Area").GetComponent<BoxCollider>();
                Vector3 spawnPosition = ExtensionFunctions.RandomPointInBounds(bcoll.bounds);

                go.transform.position = spawnPosition;
                Players[pKey].gameAvatar = go;
                go.tag = Players[pKey].playerTeam == LLAPI.Team.Merc ? "Merc" : "Spy";

                MonoBehaviour[] allMonos = go.GetComponentsInChildren<MonoBehaviour>();

                for (int i = 0; i < allMonos.Length; i++)
                {
                    allMonos[i].enabled = false;
                }
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
    }

    public void OnPlayerTeamChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientTeamChange ctc = aMsg.ReadMessage<Msg_ClientTeamChange>();
        Players[ctc.ConnectionID].playerTeam = ctc.Team;
        CS_Lobby.Instance.SetPlayerTeam(Players[ctc.ConnectionID]);

        //tell other clients
        Send(ctc.ConnectionID, MSGTYPE.LOBBY_TEAM_CHANGE, ctc, false);
    }

    public void OnPlayerNameChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientNameChange cnc = aMsg.ReadMessage<Msg_ClientNameChange>();
        Players[cnc.connectionID].playerName = cnc.name;
        CS_LobbyManager.Instance.SetPlayerName(Players[cnc.connectionID]);

        Send(cnc.connectionID, MSGTYPE.LOBBY_NAME_CHANGE, cnc, false);
    }

    public void OnPlayerMoveChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientMove cm = aMsg.ReadMessage<Msg_ClientMove>();

        Players[cm.connectId].gameAvatar.transform.position = cm.position;

        Send(cm.connectId, MSGTYPE.CLIENT_MOVE, cm, false);
    }

    public void OnPlayerRotationChange(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientRotation cm = aMsg.ReadMessage<Msg_ClientRotation>();

        Players[cm.connectId].gameAvatar.transform.rotation = cm.rot;

        Send(cm.connectId, MSGTYPE.CLIENT_ROTATION, cm, false);
    }

    public void OnPlayerABFire(NetworkMessage aMsg)
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

        Send(cf.ConnectId, MSGTYPE.CLIENT_AB_FIRE, cf, false);
    }

    public void OnPlayerABStun(NetworkMessage aMsg)
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

        Send(cs.ConnectionID, MSGTYPE.CLIENT_AB_STUN, cs, false);
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

        if(ct.TrackerTriggered)
        {
            tracker.SetActive(true);
        }
        else
        {
            tracker.SetActive(false);
        }

        Send(ct.ConnectionID, MSGTYPE.CLIENT_AB_TRACKER, ct, false);
    }

    public void OnPlayerCapturePoint(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientCapaturePoint ccp = aMsg.ReadMessage<Msg_ClientCapaturePoint>();

        if (ccp.IsBeingCaptured)
        {
            capturePoints[ccp.ID].GetComponent<NO_CapturePoint>().IsBeingCaptured = true;
        }
        else
        {
            capturePoints[ccp.ID].GetComponent<NO_CapturePoint>().IsBeingCaptured = false;

            //Sync both capture points
            //Desync could happen
        }
    }

    public void OnPlayerTrigger(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_ClientTrigger ccp = aMsg.ReadMessage<Msg_ClientTrigger>();

        Send(ccp.ConnectionID, MSGTYPE.CLIENT_AB_TRIGGER, ccp);
    }


    public void OnSpyExitedLevel(NetworkMessage aMsg)
    {
        MiniModule_GameOver.Instance.SpyExitedLevel(aMsg.conn.connectionId);
    }

    public void OnPingPong(NetworkMessage aMsg)
    {
        aMsg.reader.SeekZero();
        Msg_PingPong pp = aMsg.ReadMessage<Msg_PingPong>();

        Debug.Log("Server got message: " + (DateTime.UtcNow.Millisecond - pp.timeStamp));

        pp.timeStamp = DateTime.UtcNow.Millisecond;
        NetworkServer.SendToClient(pp.connectId, MSGTYPE.PING_PONG, pp);
    }

    public void Send(int a_connectionId, short aType, MessageBase aMsg, bool inclusive = true)
    {
        List<int> p = new List<int>();
        foreach (var id in players.Keys)
        {
            if (inclusive)
            {
                if (id == a_connectionId)
                {
                    p.Add(id);
                }
            }
            else
            {
                if (id != a_connectionId)
                {
                    p.Add(id);
                }
            }
        }
        Send(aType, aMsg, p);
    }

    public void Send(short aType, MessageBase aMsg, List<int> aP)
    {
        foreach (var item in aP)
        {
            NetworkServer.SendToClient(item, aType, aMsg);
        }
    }

    public void SendAll(short aType, MessageBase aMsg)
    {
        NetworkServer.SendToAll(aType, aMsg);
    }
}
