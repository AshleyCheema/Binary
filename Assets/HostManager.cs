using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Mover : NetworkMessage
{
    public Vector3 m_move;
}


public class HostManager : NetworkManager
{
    private void Start()
    {
        ConnectionConfig config;

        ConnectionConfig cc = new ConnectionConfig();

        StartServer(cc, 4);

        Mover mover = new Mover();

        mover.m_move = Vector3.zero;

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        
    }


}



public class LobbyMManger : MonoBehaviour
{
    HostManager manager = FindObjectOfType<HostManager>();

    private void Start()
    {
        //manager.client.re
    }
}