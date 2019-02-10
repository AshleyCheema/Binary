/*
 * Author: Ian Hudson
 * Description: 
 * Created: 06/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject_Net : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerObjectPrefab;

    [SerializeField]
    private string playerName;

    // Start is called before the first frame update
    void Start()
    {
        //Check if this is the local player
        if(!isLocalPlayer)
        {
            return;
        }

        CmdSpawnMyPlayer();
        CmdChangePlayerName("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    [Command]
    private void CmdSpawnMyPlayer()
    {
        GameObject go = Instantiate(playerObjectPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a_name"></param>
    [Command]
    void CmdChangePlayerName(string a_name)
    {
        Debug.Log("CmdChangePlayerName " + a_name);
        playerName = a_name;

        RpcChangePlayerName(playerName);
    }

    [ClientRpc]
    void RpcChangePlayerName(string a_name)
    {
        playerName = a_name;
    }
}
