using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LLAPI
{
    public class Player
    {
        public int connectionId;
        public string playerName;
        public Team team;
        public GameObject lobbyAvater;
        public GameObject avater;
        public List<GameObject> avaterObjects;
        public bool IsReady;
        public bool LoadedGame;
        public float moveSpeed = 5;
    }

    public enum Status
    {
        Lobby,
        Game
    }

    public class Server : MonoBehaviour
    {
        private static Server instance;
        public static Server Instance
        { get { return instance; } }

        private const int MAX_CONNECTIONS = 3;

        private int port = 7777;

        private int hostId;

        private int reliableChannel;
        public int ReliableChannel
        { get { return reliableChannel; } }

        private int unreliableChannel;
        public int UnreliableChannel
        { get { return unreliableChannel; } }

        private int stateUpdateChannel;
        public int StateUpdateChannel
        { get { return stateUpdateChannel; } }

        private bool isStarted = false;
        private byte error;

        private Status currentStatus = Status.Lobby;

        [SerializeField]
        private SpawnableObjects spawnableObjects;
        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Dictionary<int, Player> Players
        { get { return players; } }

        private Dictionary<int, Network_Object> networkObjects = new Dictionary<int, Network_Object>();

        private Dictionary<int, Vector3> fixedUpdateMovement = new Dictionary<int, Vector3>();

        // Start is called before the first frame update
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

            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();

            reliableChannel = cc.AddChannel(QosType.Reliable);
            unreliableChannel = cc.AddChannel(QosType.Unreliable);
            stateUpdateChannel = cc.AddChannel(QosType.StateUpdate);

            HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);

            hostId = NetworkTransport.AddHost(topo, port, null);

            Network_Object[] netObjs = GameObject.FindObjectsOfType<Network_Object>();
            for (int i = 0; i < netObjs.Length; i++)
            {
                networkObjects.Add(netObjs[i].ID, netObjs[i]);
            }

            isStarted = true;
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


        private int syncCount = 0;
        private void FixedUpdate()
        {
            /*
            syncCount++;
            List<int> keyList = new List<int>(fixedUpdateMovement.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                Vector3 rawVelocity = fixedUpdateMovement[keyList[i]];
                Rigidbody rb = players[keyList[i]].gameObject.GetComponent<Rigidbody>();

                rb.MovePosition(rb.position + rawVelocity * 5 * Time.fixedDeltaTime);
            }

            if(syncCount == 1)
            {
                syncCount = 0;

                foreach (var key in players.Keys)
                {
                    NetMsg_PlayerMovement pm = new NetMsg_PlayerMovement();
                    pm.connectId = key;

                    Rigidbody rb = players[key].GetComponent<Rigidbody>();
                    pm.xMove = rb.position.x;
                    pm.yMove = rb.position.z;

                    //update clients
                    Send(pm, reliableChannel, key);
                    Send(pm, reliableChannel, key, false);
                }
            }
            */
        }

        private void UpdateInLobby()
        {
            if (!isStarted)
            {
                return;
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
                        OnConection(recConnectionId);
                        break;

                    case NetworkEventType.DataEvent:
                        ReciveData(recHostId, recConnectionId, recChannelId, recBuffer);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        OnDisconnect(recConnectionId);
                        break;
                }
            } while (recData != NetworkEventType.Nothing);
        }

        private void UpdateInGame()
        {
            if (!isStarted)
            {
                return;
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

                if(recData == NetworkEventType.Nothing)
                {
                    break;
                }

                byte[] workingBuffer = new byte[dataSize];

                if(dataSize > 0)
                {
                    Buffer.BlockCopy(recBuffer, 0, workingBuffer, 0, dataSize);
                }

                switch (recData)
                {
                    //case NetworkEventType.ConnectEvent:
                    //    OnConection(recConnectionId);
                    //    break;

                    case NetworkEventType.DisconnectEvent:
                        OnDisconnect(recConnectionId);
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

        #region OnData

        private void ReciveData(int a_hostId, int a_connectionId, int a_channelId, byte[] a_recBuffer)
        {
            Stream serializedMessage = new MemoryStream(a_recBuffer);
            BinaryFormatter formatter = new BinaryFormatter();
            NetMsg message = (NetMsg)formatter.Deserialize(serializedMessage);

            if (currentStatus == Status.Lobby)
            {
                OnDataLobby(hostId, a_connectionId, a_channelId, message);
            }
            else if (currentStatus == Status.Game)
            {
                OnData(hostId, a_connectionId, a_channelId, message);
            }
        }

        //This function is called when data is sent
        void OnDataLobby(int hostId, int connectionId, int channelId, NetMsg a_netmsg)
        {
            switch (a_netmsg.OP)
            {
                case NetOP.NAME_CHANGE_LB:
                    NetMsg_NameChangeLB nameLB = (NetMsg_NameChangeLB)a_netmsg;
                    players[nameLB.ConnectionID].playerName = nameLB.NewName;
                    Send(nameLB, reliableChannel, nameLB.ConnectionID, false);
                    break;

                case NetOP.TEAM_CHANGE_LB:
                    NetMsg_TeamChangeLB teamLB = (NetMsg_TeamChangeLB)a_netmsg;
                    players[teamLB.ConnectionID].team = teamLB.Team;
                    Send(teamLB, reliableChannel, teamLB.ConnectionID, false);
                    break;
                case NetOP.IS_READY_LB:
                    NetMsg_IsReadyLB readyLB = (NetMsg_IsReadyLB)a_netmsg;
                    players[readyLB.ConnectionID].IsReady = readyLB.IsReady;

                    CheckForGameStart();

                    break;

                case NetOP.CLIENT_CONFIRM_LOAD_SCENE_LB:
                    NetMsg_ClientConfirmSceneLoadLB conformLoadLB = (NetMsg_ClientConfirmSceneLoadLB)a_netmsg;
                    players[conformLoadLB.ConnectionID].LoadedGame = true;

                    CheckForAllSceneLoads();
                    break;
            }

        }

        //This function is called when data is sent
        void OnData(int hostId, int connectionId, int channelId, NetMsg a_netmsg)
        {
            switch (a_netmsg.OP)
            {
                case NetOP.CONNECTION:
                    break;

                case NetOP.PLAYERMOVEMENT:
                    NetMsg_PlayerMovement movement = (NetMsg_PlayerMovement)a_netmsg;
                    MoveObject(movement.xMove, movement.yMove, movement.zMove, players[connectionId].avater, connectionId);
                    break;

                case NetOP.ROTATION:
                    NetMsg_PlayerRotation rotation = (NetMsg_PlayerRotation)a_netmsg;
                    RotateObject(rotation, connectionId);
                    break;

                case NetOP.NETWORK_OBJECT:
                    NetMsg_NetworkObject netObj = (NetMsg_NetworkObject)a_netmsg;
                    networkObjects[netObj.ID].Recive(netObj);
                    break;

                case NetOP.AB_SPRINT:
                    NetMsg_AB_Sprint ab_sprint = (NetMsg_AB_Sprint)a_netmsg;
                    players[ab_sprint.ConnectionID].moveSpeed = ab_sprint.SprintValue;
                    break;

               case NetOP.AB_FIRE:
                   NetMsg_AB_Fire ab_fire = (NetMsg_AB_Fire)a_netmsg;
                    players[ab_fire.ConnectionID].avaterObjects.Add(Instantiate(spawnableObjects.ObjectsToSpawn[ab_fire.BulletObjectIndex], new Vector3(ab_fire.BulletPositionX, ab_fire.BulletPositionY, ab_fire.BulletPositionZ), Quaternion.identity));
                    players[ab_fire.ConnectionID].avaterObjects[players[ab_fire.ConnectionID].avaterObjects.Count  - 1].GetComponent<Rigidbody>().velocity = new Vector3(ab_fire.VelocityX, ab_fire.VelocityY, ab_fire.VelocityZ);

                    /*
                     * foreach(player)
                     *      check if there position is in the radius
                     *      
                     *      send new message affect flash bang to client 
                     * 
                     * 
                     */
                     break;
                case NetOP.AB_STUN:
                    NetMsg_AB_Stun ab_stun = (NetMsg_AB_Stun)a_netmsg;

                    //spawn stun
                    players[ab_stun.ConnectionID].avaterObjects.Add(Instantiate(spawnableObjects.ObjectsToSpawn[ab_stun.StunObjectIndex], players[ab_stun.ConnectionID].avater.transform.position, Quaternion.identity));
                    Send(ab_stun, reliableChannel, ab_stun.ConnectionID, false);
                    break;

                case NetOP.AB_TRACKER:
                    //A tracker has been placed down in the scene
                    NetMsg_AB_Tracker ab_tracker = (NetMsg_AB_Tracker)a_netmsg;

                    //Check if the player does not have a tracker within the scene already
                    GameObject tracker = null;
                    for (int i = 0; i < players[ab_tracker.ConnectionID].avaterObjects.Count; i++)
                    {
                        if(players[ab_tracker.ConnectionID].avaterObjects[i].name == "Merc_Tracker")
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
                    }
                    else
                    {
                        tracker.transform.position = new Vector3(ab_tracker.TrackerPositionX, ab_tracker.TrackerPositionY, ab_tracker.TrackerPositionZ);
                    }
                    break;
            }

        }

        #endregion

        /// <summary>
        /// Check if the game can start. This can only happen if all the current players 
        /// have readied up.
        /// </summary>
        private void CheckForGameStart()
        {
            bool isReady = true;

            foreach (var key in players.Keys)
            {
                if (!players[key].IsReady)
                {
                    isReady = false;
                    break;
                }
            }

            if (!isReady)
            {
                //Not all players have readied up
            }
            else
            {
                //StartGame
                ClientLoadScenes(1);
            }
        }

        /// <summary>
        /// Tell all the clients to load new scenes
        /// </summary>
        private void ClientLoadScenes(int a_scene)
        {
            foreach (var key in players.Keys)
            {
                NetMsg_ClientLoadSceneLB sceneLoadLB = new NetMsg_ClientLoadSceneLB();
                sceneLoadLB.ConnectionID = players[key].connectionId;
                sceneLoadLB.SceneToLoad = 2;
                Send(sceneLoadLB, reliableChannel, players[key].connectionId);
            }
        }

        /// <summary>
        /// Check that all the players have loaded their scenes
        /// </summary>
        private void CheckForAllSceneLoads()
        {
            bool allScenesLoad = true;

            foreach (var key in players.Keys)
            {
                if (!players[key].LoadedGame)
                {
                    allScenesLoad = false;
                    break;
                }
            }

            if(allScenesLoad)
            {
                //Spawn all the players and set the current status to game 
                //as we are now being the game
                SpawnAllPlayers();
                currentStatus = Status.Game;
            }
        }

        /// <summary>
        /// When a new client has connected to the server 
        /// call this to setup the new player and send all the 
        /// relevant data to all players on the server
        /// </summary>
        /// <param name="a_connectionId"></param>
        private void OnConection(int a_connectionId)
        {
            //Create a new player
            Player p = new Player();
            p.connectionId = a_connectionId;
            p.playerName = "Default";
            p.team = Team.Unassigned;
            p.IsReady = false;
            p.LoadedGame = false;
            p.avaterObjects = new List<GameObject>();
            //Add the new player to the players on the srever
            players.Add(a_connectionId, p);

            //Create a new message to send to the new player
            //Send the player the server's connection ID. THIS IS UNIQUE 
            NetMsg_SendServerConnectionID id = new NetMsg_SendServerConnectionID();
            id.ConnectionId = a_connectionId;
            Send(id, reliableChannel, a_connectionId);

            //Create a new message to send to all players on the server apart from
            //the new player
            NetMsg_SpawnPlayerLB msg = new NetMsg_SpawnPlayerLB();
            msg.ConnectionID = p.connectionId;
            msg.PlayerName = p.playerName;
            msg.Team = p.team;

            //Send the new player's information to all clients
            Send(msg, reliableChannel, a_connectionId, false);

            //Send all the old players information to the new player
            foreach (var pKey in players.Keys)
            {
                NetMsg_SpawnPlayerLB spawnLobbyP = new NetMsg_SpawnPlayerLB();
                spawnLobbyP.ConnectionID = players[pKey].connectionId;
                spawnLobbyP.PlayerName = players[pKey].playerName;
                spawnLobbyP.Team = players[pKey].team;

                if(pKey != a_connectionId)
                {
                    Send(spawnLobbyP, reliableChannel, a_connectionId);
                }
            }

            //All clients should now have synced the lobby information
        }

        /// <summary>
        /// Spawn all the players on the server and on all the clients
        /// </summary>
        private void SpawnAllPlayers()
        {
            NetMsg_SpawnObject spawnNewPlayer = new NetMsg_SpawnObject();
            foreach (var pKey in players.Keys)
            {
                Vector3 spawnPosition = Vector3.zero;

                BoxCollider spySpawnTrigger = GameObject.Find("Spawn_Spy_Area").GetComponent<BoxCollider>();
                BoxCollider mercSpawnTrigger = GameObject.Find("Spawn_Merc_Area").GetComponent<BoxCollider>();

                if (players[pKey].team == Team.Merc)
                {
                    spawnPosition = ExtensionFunctions.RandomPointInBounds(mercSpawnTrigger.bounds);
                }
                else if (players[pKey].team == Team.Spy)
                {
                    spawnPosition = ExtensionFunctions.RandomPointInBounds(spySpawnTrigger.bounds);
                }

                spawnNewPlayer.ObjectsToSpawn.Add(new SpawnableObject
                {
                    ConnectionID = pKey,
                    ObjectID = players[pKey].team == Team.Merc ? 0 : 5,
                    XPos = spawnPosition.x,
                    YPos = spawnPosition.y,
                    ZPos = spawnPosition.z,
                    XRot = 0,
                    YRot = 0,
                    ZRot = 0
                });

                //Spawn server avater
                players[pKey].avater = Instantiate(spawnableObjects.ObjectsToSpawn[(players[pKey].team == Team.Merc ? 0 : 5)],
                                        spawnPosition,
                                        Quaternion.identity);

                if (players[pKey].team == Team.Merc)
                {
                    players[pKey].avater.GetComponent<MercControls>().enabled = false;
                    players[pKey].avater.GetComponent<TrackerAbility>().enabled = false;
                }
                else
                {
                    players[pKey].avater.GetComponent<SpyController>().enabled = false;
                    players[pKey].avater.GetComponent<StunAbility>().enabled = false;
                }

                if (players[pKey].team == Team.Spy)
                {
                    players[pKey].avater.tag = "Spy";
                }
                else
                {
                    players[pKey].avater.tag = "Merc";
                }

                /*
                //loop though all our current players
                //if the keys are different then 
                //tell the old client to spawn the new client's player
                foreach (var key in players.Keys)
                {
                    if (key != pKey)
                    {
                        //spawn the new player on all the old players client's
                        Send(spawnNewPlayer, reliableChannel, key);
                    }
                }

                spawnNewPlayer = new NetMsg_SpawnObject();
                //loop though all our current players 
                //if the keys are different then add the old player to 
                //the list 
                foreach (var key in players.Keys)
                {
                    if (key != pKey)
                    {
                        spawnNewPlayer.ObjectsToSpawn.Add(0);
                        spawnNewPlayer.ObjectsConnectionIds.Add(key);
                    }
                }

                //spawn all the old players on the new player's client
                Send(spawnNewPlayer, reliableChannel, pKey);
                */
            }

            Send(spawnNewPlayer, reliableChannel);
            //Add all other players
            //foreach (var key in players.Keys)
            //{
            //    NetMsg_SpawnObject spawnNewPlayer = new NetMsg_SpawnObject();
            //
            //    spawnNewPlayer.ObjectsToSpawn.Add(new SpawnableObject
            //    {
            //        ConnectionID = key,
            //        ObjectID = 0,
            //        XPos = players[key].avater.GetComponent<Rigidbody>().position.x,
            //        YPos = players[key].avater.GetComponent<Rigidbody>().position.y,
            //        ZPos = players[key].avater.GetComponent<Rigidbody>().position.z,
            //        XRot = 0,
            //        YRot = 0,
            //        ZRot = 0
            //    });
            //    //Tell the client it self to spawn
            //}
        }

        private void OnDisconnect(int a_connectionId)
        {
            //destroy ther server object for that client
            if (players[a_connectionId].avater != null)
            {
                Destroy(players[a_connectionId].avater);
            }

            players.Remove(a_connectionId);

            NetMsg_ClientDisconnection dis = new NetMsg_ClientDisconnection();
            dis.ConnectionID = a_connectionId;

            Send(dis, reliableChannel, a_connectionId, false);
        }

        /// <summary>
        /// Move an object 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="obj"></param>
        /// <param name="a_connectionId"></param>
        private void MoveObject(float x, float y, float z, GameObject obj, int a_connectionId)
        {
            //if (!fixedUpdateMovement.ContainsKey(a_connectionId))
            //{
            //    fixedUpdateMovement.Add(a_connectionId, new Vector3(x, 0, y));
            //}
            //else
            //{
            //    fixedUpdateMovement[a_connectionId] = new Vector3(x, 0, y);
            //}

            players[a_connectionId].avater.GetComponent<Rigidbody>().MovePosition(new Vector3(x, y, z));

            //obj.transform.Translate((x * 5) * Time.deltaTime, 0, (y * 5) * Time.deltaTime);
            //
            NetMsg_PlayerMovement pm = new NetMsg_PlayerMovement();
            pm.xMove = obj.GetComponent<Rigidbody>().position.x;
            pm.yMove = obj.GetComponent<Rigidbody>().position.y;
            pm.zMove = obj.GetComponent<Rigidbody>().position.z;
            pm.connectId = a_connectionId;
            //
            //Send(pm, reliableChannel, a_connectionId);
            Send(pm, unreliableChannel, a_connectionId, false);
        }

        /// <summary>
        /// Rotate an object 
        /// </summary>
        /// <param name="a_msg"></param>
        /// <param name="a_connectionId"></param>
        private void RotateObject(NetMsg_PlayerRotation a_msg, int a_connectionId)
        {
            players[a_connectionId].avater.transform.rotation = Quaternion.Euler(a_msg.XRot, a_msg.YRot, a_msg.ZRot);

            NetMsg_PlayerRotation pm = new NetMsg_PlayerRotation();
            pm.ConnectionId = a_connectionId;
            pm.XRot = a_msg.XRot;
            pm.YRot = a_msg.YRot;
            pm.ZRot = a_msg.ZRot;

            Send(pm, unreliableChannel, a_connectionId, false);
        }

        public void Send(NetMsg a_msg, int a_channel)
        {
            List<int> keyList = new List<int>(players.Keys);
            Send(a_msg, a_channel, keyList);
        }

        /// <summary>
        /// Send a message to clients
        /// </summary>
        /// <param name="a_message"></param>
        /// <param name="a_channel"></param>
        /// <param name="a_connectionId"></param>
        public void Send(NetMsg a_msg, int a_channel, int a_connectionId, bool inclusive = true)
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
            Send(a_msg, a_channel, p);
        }

        /// <summary>
        /// Send a message to clients
        /// </summary>
        /// <param name="a_message"></param>
        /// <param name="a_channel"></param>
        /// <param name="a_clients"></param>
        public void Send(NetMsg a_msg, int a_channel, List<int> a_players)
        {
            byte[] buffer = new byte[1024];

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(buffer);
            bf.Serialize(memStream, a_msg);

            foreach (int p in a_players)
            {
                NetworkTransport.Send(hostId, p, a_channel, buffer, (int)memStream.Position, out error);
            }
        }
    }
}