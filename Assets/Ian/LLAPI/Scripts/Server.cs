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

        private bool isStarted = false;
        private byte error;

        private Status currentStatus = Status.Lobby;

        [SerializeField]
        private GameObject playerPrefab;
        private Dictionary<int, Player> players = new Dictionary<int, Player>();

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

            NetworkEventType type = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, recBuffer.Length, out dataSize, out error);

            switch (type)
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

            NetworkEventType type = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, recBuffer.Length, out dataSize, out error);

            switch (type)
            {
                //case NetworkEventType.ConnectEvent:
                //    OnConection(recConnectionId);
                //    break;

                case NetworkEventType.DisconnectEvent:
                    OnDisconnect(recConnectionId);
                    break;

                case NetworkEventType.DataEvent:
                    ReciveData(recHostId, recConnectionId, recChannelId, recBuffer);
                    break;

                default:
                case NetworkEventType.BroadcastEvent:
                    //Debug.Log("Unexpected network event type");
                    break;
            }
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
                    NetMsg_PlayerMovement movement = a_netmsg as NetMsg_PlayerMovement;
                    MoveObject(movement.xMove, movement.yMove, players[connectionId].avater, connectionId);
                    break;

                case NetOP.NETWORK_OBJECT:
                    NetMsg_NetworkObject netObj = (NetMsg_NetworkObject)a_netmsg;
                    networkObjects[netObj.ID].Recive(netObj);
                    break;
            }

        }

        #endregion

        private void OnConection(int a_connectionId)
        {
            Player p = new Player();
            p.connectionId = a_connectionId;
            p.playerName = "Default";
            p.team = Team.Unassigned;
            players.Add(a_connectionId, p);

            NetMsg_SendServerConnectionID id = new NetMsg_SendServerConnectionID();
            id.ConnectionId = a_connectionId;
            Send(id, reliableChannel, a_connectionId);

            NetMsgSpawnPlayerLB msg = new NetMsgSpawnPlayerLB();
            msg.ConnectionID = a_connectionId;
            msg.PlayerName = "Default";
            msg.Team = Team.Unassigned;

            Send(msg, reliableChannel, a_connectionId, false);

            foreach (var pKey in players.Keys)
            {
                NetMsgSpawnPlayerLB spawnLobbyP = new NetMsgSpawnPlayerLB();
                spawnLobbyP.ConnectionID = players[pKey].connectionId;
                spawnLobbyP.PlayerName = players[pKey].playerName;
                spawnLobbyP.Team = players[pKey].team;

                if(pKey != a_connectionId)
                {
                    Send(spawnLobbyP, reliableChannel, a_connectionId);
                }
            }
        }

        private void SpawnAllPlayers()
        {
            foreach (var pKey in players.Keys)
            {
                NetMsg_SpawnObject spawnNewPlayer = new NetMsg_SpawnObject();
                spawnNewPlayer.ObjectsToSpawn.Add(0);

                spawnNewPlayer.ObjectsConnectionIds.Add(pKey);

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
            }
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

        private void MoveObject(float x, float y, GameObject obj, int a_connectionId)
        {
            //if (!fixedUpdateMovement.ContainsKey(a_connectionId))
            //{
            //    fixedUpdateMovement.Add(a_connectionId, new Vector3(x, 0, y));
            //}
            //else
            //{
            //    fixedUpdateMovement[a_connectionId] = new Vector3(x, 0, y);
            //}

            players[a_connectionId].avater.GetComponent<Rigidbody>().MovePosition(new Vector3(x, 0.5f, y));

            //obj.transform.Translate((x * 5) * Time.deltaTime, 0, (y * 5) * Time.deltaTime);
            //
            NetMsg_PlayerMovement pm = new NetMsg_PlayerMovement();
            pm.xMove = obj.GetComponent<Rigidbody>().position.x;
            pm.yMove = obj.GetComponent<Rigidbody>().position.z;
            pm.connectId = a_connectionId;
            //
            //Send(pm, reliableChannel, a_connectionId);
            Send(pm, reliableChannel, a_connectionId, false);
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