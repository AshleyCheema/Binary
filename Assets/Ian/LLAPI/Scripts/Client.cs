using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

namespace LLAPI
{
    public enum Team
    {
        Unassigned,
        Spy,
        Merc
    }

    public class Client : MonoBehaviour
    {
        private static Client instance;
        public static Client Instance
        { get { return instance; } }

        #region Server/Client settings
        private const int MAX_CONNECTIONS = 3;

        private int port = 7777;

        private int hostId;

        private int reliableChannel;
        public int ReliableChannel
        { get { return reliableChannel; } }

        private int unreliableChannel;
        public int UnreliableChannel
        { get { return UnreliableChannel; } }

        private int stateUpdateChannel;
        public int StateUpdateChannel
        { get { return stateUpdateChannel; } }

        private int clientId;

        private int connectionId;

        private int serverConnectionId;
        public int ServerConnectionId
        { get { return serverConnectionId; } }

        private float connectionTime;
        private bool isConnected = false;
        private bool isStarted = false;
        private byte error;

        private string clientName;

        #endregion

        //This is the local object for this client whihc is shown in the lobby
        [SerializeField]
        private Player localPlayer;
        public Player LocalPlayer
        { get { return localPlayer; } set { localPlayer = value; } }

        #region Game
        private Status currentStatus = Status.Lobby;

        [SerializeField]
        private SpawnableObjects spawnableObjects;

        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Dictionary<int, Player> Players
        { get { return players; } }

        private Dictionary<int, Network_Object> networkObjects = new Dictionary<int, Network_Object>();

        #endregion

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

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (currentStatus == Status.Lobby)
            {
                UpdateInLobby();
            }
            else if (currentStatus == Status.Game)
            {
                UpdateInGame();
            }
        }

        private void UpdateInLobby()
        {
            if (isConnected)
            {
                int recHostId;
                int recConnectionId;
                int recChannelId;

                byte[] recBuffer = new byte[1024];
                int dataSize;

                NetworkEventType recData = NetworkEventType.Nothing;

                do
                {
                    recData = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, recBuffer.Length, out dataSize, out error);

                    if (recData == NetworkEventType.Nothing)
                    {
                        break;
                    }

                    byte[] workingBuffer = new byte[dataSize];

                    if (dataSize > 0)
                    {
                        Buffer.BlockCopy(recBuffer, 0, workingBuffer, 0, dataSize);
                    }

                    switch (recData)
                    {
                        case NetworkEventType.ConnectEvent:
                            Debug.Log("We have connected to server");
                            break;

                        case NetworkEventType.DisconnectEvent:
                            Debug.Log("We have disconnected to server");
                            break;

                        case NetworkEventType.DataEvent:
                            ReciveData(recHostId, recConnectionId, recChannelId, recBuffer);
                            break;
                    }
                } while (recData != NetworkEventType.Nothing);
            }
        }

        private void UpdateInGame()
        {
            if (!isConnected)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                NetMsg_StringMessage stringmsg = new NetMsg_StringMessage();
                stringmsg.Message = "This is a test message from " + connectionId + " Client";

                Send(stringmsg, reliableChannel);
            }

            int recHostId;
            int recConnectionId;
            int recChannelId;

            byte[] recBuffer = new byte[1024];
            int dataSize;

            NetworkEventType recData = NetworkEventType.Nothing;

            do
            {
                recData = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, recBuffer.Length, out dataSize, out error);


                if (recData == NetworkEventType.Nothing)
                {
                    break;
                }

                byte[] workingBuffer = new byte[dataSize];

                if (dataSize > 0)
                {
                    Buffer.BlockCopy(recBuffer, 0, workingBuffer, 0, dataSize);
                }

                switch (recData)
                {
                    case NetworkEventType.ConnectEvent:
                        Debug.Log("We have connected to server");
                        break;

                    case NetworkEventType.DisconnectEvent:
                        Debug.Log("We have disconnected to server");
                        break;

                    case NetworkEventType.DataEvent:
                        ReciveData(recHostId, recConnectionId, recChannelId, workingBuffer);
                        break;

                    default:
                    case NetworkEventType.BroadcastEvent:
                        //Debug.Log("Unexpected network event type");
                        break;
                }
            } while (recData != NetworkEventType.Nothing);
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect(string a_ip)
        {
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();

            reliableChannel = cc.AddChannel(QosType.Reliable);
            unreliableChannel = cc.AddChannel(QosType.Unreliable);
            stateUpdateChannel = cc.AddChannel(QosType.StateUpdate);

            HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);

            hostId = NetworkTransport.AddHost(topo, 0);
            connectionId = NetworkTransport.Connect(hostId, a_ip, port, 0, out error);

            //players.Add(connectionId, localPlayer.gameObject);

            connectionTime = Time.time;
            isConnected = true;
        }

        /// <summary>
        /// Become the server/Host
        /// </summary>
        public void ServerHost()
        {

        }

        /// <summary>
        /// Disconect from the server
        /// </summary>
        public void Disconnect()
        {
            NetworkTransport.Disconnect(hostId, connectionId, out error);
        }

        private void ReciveData(int a_hostId, int a_connectionId, int a_channelId, byte[] a_recBuffer)
        {
            Stream serializedMessage = new MemoryStream(a_recBuffer);
            BinaryFormatter formatter = new BinaryFormatter();
            NetMsg message = (NetMsg)formatter.Deserialize(serializedMessage);

            if(currentStatus == Status.Lobby)
            {
                OnDataLobby(a_hostId, a_connectionId, a_channelId, message);
            }
            else if (currentStatus == Status.Game)
            {
                OnData(a_hostId, a_connectionId, a_channelId, message);
            }
        }

        #region OnData

        //This function is called when data is sent
        void OnDataLobby(int a_hostId, int a_connectionId, int a_channelId, NetMsg a_netmsg)
        {
            switch (a_netmsg.OP)
            {
                case NetOP.CONNECTION:
                    //Output the deserialized message as well as the connection information to the console
                    Debug.Log("OnData(hostId = " + a_hostId + ", connectionId = "
                        + a_connectionId + ", channelId = " + a_channelId + ", data = " + a_netmsg.OP);
                    break;

                case NetOP.DISCONNECTION:
                    NetMsg_ClientDisconnection dis = (NetMsg_ClientDisconnection)a_netmsg;
                    Destroy(players[dis.ConnectionID].lobbyAvater);
                    players.Remove(dis.ConnectionID);
                    break;

                case NetOP.SENDCONNECTIONID:
                    NetMsg_SendServerConnectionID id = (NetMsg_SendServerConnectionID)a_netmsg;
                    serverConnectionId = id.ConnectionId;

                    localPlayer = new Player();
                    localPlayer.connectionId = serverConnectionId;
                    localPlayer.lobbyAvater = Instantiate(spawnableObjects.ObjectsToSpawn[1], CS_LobbyManager.Instance.transform);

                    CS_LobbyManager.Instance.AddLobbyPlayer(localPlayer);
                    break;

                case NetOP.SPAWN_PLAYER_LB:
                    NetMsg_SpawnPlayerLB sp = (NetMsg_SpawnPlayerLB)a_netmsg;

                    Player p = new Player();
                    p.connectionId = sp.ConnectionID;
                    p.playerName = sp.PlayerName;
                    p.team = sp.Team;

                    p.lobbyAvater = Instantiate(spawnableObjects.ObjectsToSpawn[1], CS_LobbyManager.Instance.transform);
                    p.lobbyAvater.transform.localScale = new Vector3(1, 1, 1);

                    players.Add(p.connectionId, p);

                    CS_LobbyManager.Instance.SetShell(p);
                    CS_LobbyManager.Instance.AddLobbyPlayer(p, true);
                    CS_LobbyManager.Instance.SetPlayerName(p);
                    CS_LobbyManager.Instance.SetPlayerTeam(p);

                    break;

                case NetOP.NAME_CHANGE_LB:
                    NetMsg_NameChangeLB nameLB = (NetMsg_NameChangeLB)a_netmsg;
                    players[nameLB.ConnectionID].playerName = nameLB.NewName;
                    CS_LobbyManager.Instance.SetPlayerName(players[nameLB.ConnectionID]);
                    break;

                case NetOP.TEAM_CHANGE_LB:
                    NetMsg_TeamChangeLB teamLB = (NetMsg_TeamChangeLB)a_netmsg;
                    players[teamLB.ConnectionID].team = teamLB.Team;
                    CS_LobbyManager.Instance.SetPlayerTeam(players[teamLB.ConnectionID]);
                    break;

                case NetOP.CLIENT_LOAD_SCENE_LB:
                    NetMsg_ClientLoadSceneLB sceneLoadLB = (NetMsg_ClientLoadSceneLB)a_netmsg;
                    SceneManager.sceneLoaded += SceneLoaded;
                    SceneManager.LoadScene(sceneLoadLB.SceneToLoad);

                    break;
            }

        }

        //This function is called when data is sent
        void OnData(int a_hostId, int a_connectionId, int a_channelId, NetMsg a_netmsg)
        {
            switch (a_netmsg.OP)
            {
                case NetOP.SPAWNOBJECT:
                    NetMsg_SpawnObject spawnObject = (NetMsg_SpawnObject)a_netmsg;

                    int index = 0;
                    foreach (var key in spawnObject.ObjectsToSpawn)
                    {
                        if (key.ConnectionID == serverConnectionId)
                        {
                            localPlayer.avater = Instantiate(spawnableObjects.ObjectsToSpawn[key.ObjectID], new Vector3(spawnObject.ObjectsToSpawn[index].XPos,
                                                                                                                        spawnObject.ObjectsToSpawn[index].YPos, 
                                                                                                                        spawnObject.ObjectsToSpawn[index].ZPos), 
                                                                                                                        Quaternion.Euler(spawnObject.ObjectsToSpawn[index].XRot,
                                                                                                                        spawnObject.ObjectsToSpawn[index].YRot,
                                                                                                                        spawnObject.ObjectsToSpawn[index].ZRot));
                            //localPlayer.avater.GetComponent<PlayerController>().client = this;
                            localPlayer.team = (key.ObjectID == 0) ? Team.Merc : Team.Spy;
                            localPlayer.avater.tag = (localPlayer.team == Team.Merc) ? "Merc" : "Spy";
                            localPlayer.avaterObjects = new List<GameObject>();

                            Camera.main.GetComponent<CameraScript>().SetTarget(localPlayer.avater.transform);
                            //Camera.main.transform.position = localPlayer.avater.transform.position; //+ new Vector3(-15, 33, -15);
                            //Camera.main.transform.rotation = Quaternion.Euler(56, 45, 0);
                        }
                        else
                        {
                            players[key.ConnectionID].avater =
                             Instantiate(spawnableObjects.ObjectsToSpawn[key.ObjectID], new Vector3(spawnObject.ObjectsToSpawn[index].XPos,
                                                                                                    spawnObject.ObjectsToSpawn[index].YPos,
                                                                                                    spawnObject.ObjectsToSpawn[index].ZPos), 
                                                                                                    Quaternion.Euler(spawnObject.ObjectsToSpawn[index].XRot,
                                                                                                    spawnObject.ObjectsToSpawn[index].YRot,
                                                                                                    spawnObject.ObjectsToSpawn[index].ZRot));
                            players[key.ConnectionID].team = (key.ObjectID == 0) ? Team.Merc : Team.Spy;
                            players[key.ConnectionID].avater.tag = (players[key.ConnectionID].team == Team.Merc) ? "Merc" : "Spy";
                            players[key.ConnectionID].avaterObjects = new List<GameObject>();

                            if (players[key.ConnectionID].team == Team.Merc)
                            {
                                //disable the controls for this player as it is not the local player
                                players[key.ConnectionID].avater.GetComponent<MercControls>().enabled = false;
                                players[key.ConnectionID].avater.GetComponent<TrackerAbility>().enabled = false;
                            }
                            else
                            {
                                //disable the controls for this player as it is not the local player
                                players[key.ConnectionID].avater.GetComponent<SpyController>().enabled = false;
                            }
                            //disable the fog of war for this player
                            players[key.ConnectionID].avater.GetComponentInChildren<FOWMask>().gameObject.SetActive(false);

                        }
                        index += 1;
                    }
                    break;

                case NetOP.PLAYERMOVEMENT:
                    NetMsg_PlayerMovement pm = (NetMsg_PlayerMovement)a_netmsg;
                    //pm.connectId is the id from the server on which client is moving.
                    //serverConnectionId is the id which this client is on the server 
                    //If both are true then this client is moving
                    if (pm.connectId == serverConnectionId)
                    {
                        //localPlayer.transform.position = new Vector3(pm.xMove, 0.5f, pm.yMove);
                        localPlayer.avater.GetComponent<Rigidbody>().MovePosition(new Vector3(pm.xMove, pm.yMove, pm.zMove));
                    }
                    else
                    {
                        //if pm.connectId is not serverConnectionId. Then we have not moved
                        //another client has moved there object on the server. Move that 
                        //object on this local client
                        //players[pm.connectId].transform.position = new Vector3(pm.xMove, 0.5f, pm.yMove);
                        players[pm.connectId].avater.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(players[pm.connectId].avater.GetComponent<Rigidbody>().position, new Vector3(pm.xMove, pm.yMove, pm.zMove), Time.deltaTime * 10));
                    }
                    break;

                case NetOP.ROTATION:
                    NetMsg_PlayerRotation playerRotation = (NetMsg_PlayerRotation)a_netmsg;
                    if (playerRotation.ConnectionId == serverConnectionId)
                    {
                        //IT'S ME
                    }
                    else
                    {
                        players[playerRotation.ConnectionId].avater.transform.rotation = Quaternion.Euler(playerRotation.XRot, playerRotation.YRot, playerRotation.ZRot);
                    }

                    break;

                //case NetOP.CP_CAPTURE:
                //    GameObject go = GameObject.Find("CapturePoint");
                //   break;

                case NetOP.NETWORK_OBJECT:
                    NetMsg_NetworkObject netObj = (NetMsg_NetworkObject)a_netmsg;
                    networkObjects[netObj.ID].Recive(netObj);
                    break;

                case NetOP.AB_SPRINT:

                    break;

                case NetOP.AB_FIRE:
                    //Stun object has been droped by another player
                    NetMsg_AB_Fire ab_fire = (NetMsg_AB_Fire)a_netmsg;

                    players[ab_fire.ConnectionID].avaterObjects.Add(Instantiate(spawnableObjects.ObjectsToSpawn[ab_fire.BulletObjectIndex], new Vector3(ab_fire.BulletPositionX, ab_fire.BulletPositionY, ab_fire.BulletPositionZ), Quaternion.identity));
                    players[ab_fire.ConnectionID].avaterObjects[players[ab_fire.ConnectionID].avaterObjects.Count - 1].GetComponent<Rigidbody>().velocity = new Vector3(ab_fire.VelocityX, ab_fire.VelocityY, ab_fire.VelocityZ);

                    players[ab_fire.ConnectionID].avaterObjects[players[ab_fire.ConnectionID].avaterObjects.Count - 1].GetComponent<Trigger>().IsSpawned = true;
                    players[ab_fire.ConnectionID].avaterObjects[players[ab_fire.ConnectionID].avaterObjects.Count - 1].gameObject.tag = "Destroy";

                    break;

                case NetOP.AB_STUN:
                    //Stun object has been droped by another player
                    NetMsg_AB_Stun ab_stun = (NetMsg_AB_Stun)a_netmsg;

                    players[ab_stun.ConnectionID].avaterObjects.Add(Instantiate(spawnableObjects.ObjectsToSpawn[ab_stun.StunObjectIndex], players[ab_stun.ConnectionID].avater.transform.position, Quaternion.identity));
                    players[ab_stun.ConnectionID].avaterObjects[players[ab_stun.ConnectionID].avaterObjects.Count - 1].GetComponent<StunAbility>().isSpawned = true;
                    players[ab_stun.ConnectionID].avaterObjects[players[ab_stun.ConnectionID].avaterObjects.Count - 1].GetComponent<StunAbility>().stunDropped = true;

                    players[ab_stun.ConnectionID].avaterObjects[players[ab_stun.ConnectionID].avaterObjects.Count - 1].GetComponent<StunAbility>().SetShell();
                    players[ab_stun.ConnectionID].avaterObjects[players[ab_stun.ConnectionID].avaterObjects.Count - 1].GetComponent<Trigger>().IsSpawned = true;

                    break;

                case NetOP.AB_TRACKER:
                    //A tracker has been placed down in the scene
                    NetMsg_AB_Tracker ab_tracker = (NetMsg_AB_Tracker)a_netmsg;

                    //Check if the player does not have a tracker within the scene already
                    GameObject tracker = null;
                    for (int i = 0; i < players[ab_tracker.ConnectionID].avaterObjects.Count; i++)
                    {
                        if (players[ab_tracker.ConnectionID].avaterObjects[i] != null &&
                            players[ab_tracker.ConnectionID].avaterObjects[i].name == "Merc_Tracker")
                        {
                            tracker = players[ab_tracker.ConnectionID].avaterObjects[i];
                            break;
                        }
                    }

                    //If there is no tracker for this player then add one. Otherwise edit the 
                    //old tracker
                    if (tracker == null)
                    {
                        players[ab_tracker.ConnectionID].avaterObjects.Add(Instantiate(spawnableObjects.ObjectsToSpawn[ab_tracker.TrackerObjectIndex], new Vector3(ab_tracker.TrackerPositionX, ab_tracker.TrackerPositionY, ab_tracker.TrackerPositionZ), Quaternion.identity));
                        players[ab_tracker.ConnectionID].avaterObjects[players[ab_tracker.ConnectionID].avaterObjects.Count - 1].gameObject.name = "Merc_Tracker";
                        players[ab_tracker.ConnectionID].avaterObjects[players[ab_tracker.ConnectionID].avaterObjects.Count - 1].GetComponent<Trigger>().IsSpawned = true;

                    }
                    else
                    {
                        tracker.transform.position = new Vector3(ab_tracker.TrackerPositionX, ab_tracker.TrackerPositionY, ab_tracker.TrackerPositionZ);

                        if (ab_tracker.TrackerTriggered)
                        {
                            if (tracker != null)
                            {
                                tracker.SetActive(true);
                            }
                        }
                        else
                        {
                            if (tracker != null)
                            {
                                tracker.SetActive(false);
                            }
                        }
                    }

                    break;

                //if a trigger message is recived
                case NetOP.AB_TRIGGER:
                    NetMsg_AB_Trigger ab_trigger = (NetMsg_AB_Trigger)a_netmsg;

                    //if the type is bullet
                    if(ab_trigger.Type == TriggerType.BULLET)
                    {
                        Debug.Log("I AM SHOT. HELP!!!!!!!!!!!!!!!!");
                    }
                    else if(ab_trigger.Type == TriggerType.STUN)
                    {
                        Debug.Log("I HAVE BEEN STUNNED. NO!!!!!!!!!!!!!!");
                        //set isStunned to true
                        if(localPlayer.avater.GetComponent<MercControls>())
                        {
                            localPlayer.avater.GetComponent<MercControls>().IsStunned = true;
                        }
                    }

                    break;
            }

        }

        #endregion

        /// <summary>
        /// Call when a scene has been loaded
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;

            NetMsg_ClientConfirmSceneLoadLB confirm = new NetMsg_ClientConfirmSceneLoadLB();
            confirm.ConnectionID = serverConnectionId;
            confirm.SceneLoaded = true;

            currentStatus = Status.Game;

            Send(confirm);

            Network_Object[] netObjs = GameObject.FindObjectsOfType<Network_Object>();
            for (int i = 0; i < netObjs.Length; i++)
            {
                networkObjects.Add(netObjs[i].ID, netObjs[i]);
            }
        }

        public void Send(NetMsg a_msg, int a_channel = -1)
        {
            byte[] buffer = new byte[1024];

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(buffer);
            bf.Serialize(memStream, a_msg);

            NetworkTransport.Send(hostId, connectionId, a_channel == -1 ? reliableChannel : a_channel, buffer, (int)memStream.Position, out error);
        }
    }
}