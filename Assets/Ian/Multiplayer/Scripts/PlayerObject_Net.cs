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

    // Start is called before the first frame update
    void Start()
    {
        //Check if this is the local player
        if(!isLocalPlayer)
        {
            return;
        }

        CmdSpawnMyPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    private void CmdSpawnMyPlayer()
    {
        GameObject go = Instantiate(playerObjectPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
