/*
 * Author: Ian Hudson
 * Description: This script is for the player's object when
 * first connecting to the server. (Connection object)
 * Created: 06/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject_Net : NetworkBehaviour
{
    //Sync the id of this player
    [SyncVar]
    public string id;

    //Player "Avater" object
    [SerializeField]
    private GameObject playerObjectPrefab;
    //Player camera prefab
    [SerializeField]
    private Camera playerCameraPrefab;

    //Sync the "Avater" of this player
    [SerializeField]
    [SyncVar]
    private GameObject playerObject;

    //Camera for this player
    [SerializeField]
    private Camera playerCamera;

    //Player name
    [SerializeField]
    private string playerName;

    //Player ID
    [SyncVar]
    int playerId = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Check if this is the local player
        if (!isLocalPlayer)
        {
            return;
        }

        CmdSpawnMyPlayer();
        CmdSetID("Player" + GetComponent<NetworkIdentity>().netId.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Server. Spawn a player
    /// </summary>
    [Command]
    private void CmdSpawnMyPlayer()
    {
        //If the connection to the server is ready.
        //Then spawn the player
        if (connectionToClient.isReady)
        {
            CmdSpawn();
        }
        else
        {
            //Connection to the server is not ready 
            //wait
            StartCoroutine(WaitForRead());
        }
    }

    /// <summary>
    /// Coroutine. Wait for the connection to be ready
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForRead()
    {
        while (!connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }

        CmdSpawn();
    }

    /// <summary>
    /// Server. Spawn the player controller prefab on the server
    /// </summary>
    [Command]
    void CmdSpawn()
    {
        //Find all the spawn points
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Points Spy's");
        //Instantiate the player object/avater
        GameObject tempPlayer = Instantiate(playerObjectPrefab);

        //tempPlayer.GetComponentInChildren<PlayerController_Net>().PlayerObject = this;
        NetworkServer.SpawnWithClientAuthority(tempPlayer, connectionToClient);

        //After spwning the playerControllers reference the controller to the object and 
        //object to controller
        CmdSetPlayerController(tempPlayer);
        playerObject.GetComponent<PlayerController_Net>().CmdSetPlayerObject(gameObject);

        //Tell the server where this playerObject is 
        tempPlayer.transform.position = spawnPoints[playerId].transform.position;
        Debug.Log("PLAYER SPAWNED AT " + tempPlayer.transform.position);
        //RpcChangePlayerPosition(spawnPoints[playerId].transform.position);

        playerId += 1;
    }

    /// <summary>
    /// Server. Set the new id
    /// </summary>
    /// <param name="a_newID"></param>
    [Command]
    void CmdSetID(string a_newID)
    {
        id = a_newID;
    }

    /// <summary>
    /// Server. Set the player controller on the server
    /// </summary>
    /// <param name="a_go"></param>
    [Command]
    void CmdSetPlayerController(GameObject a_go)
    {
        playerObject = a_go;
    }

    //[ClientRpc]
    //private void RpcChangePlayerPosition(Vector3 a_position)
    //{
    //    //tell all the clients where this playerObject is
    //    playerObject.transform.position = a_position;
    //    Debug.Log("RpcChangePlayerPosition " + playerObject.transform.position);
    //}

    /// <summary>
    /// Client. Spawn a camera and set it to a player controller
    /// </summary>
    [ClientRpc]
    private void RpcChangePlayerCamera()
    {
        //playerCamera = Instantiate(playerCameraPrefab,
        //                    playerObject.transform.position + new Vector3(-1.5f, 3.55f, 0.74f),
        //                    Quaternion.Euler(30.0f, 40.0f, 0.0f));

        //playerCamera.GetComponent<CameraScript>().target = playerObject.transform;
    }

    /// <summary>
    /// Server. Change a playerName on the server
    /// </summary>
    /// <param name="a_name"></param>
    [Command]
    void CmdChangePlayerName(string a_name)
    {
        Debug.Log("CmdChangePlayerName " + a_name);
        playerName = a_name;

        RpcChangePlayerName(playerName);
    }

    /// <summary>
    /// Client. Chagne the player name on all clients
    /// </summary>
    /// <param name="a_name"></param>
    [ClientRpc]
    void RpcChangePlayerName(string a_name)
    {
        playerName = a_name;
    }

    /// <summary>
    /// Set an objects authority
    /// </summary>
    /// <param name="a_toBeSet"></param>
    /// <param name="a_client"></param>
    public static void SetAuthority(GameObject a_toBeSet, GameObject a_client)
    {
        if (NetworkServer.active)
        {
            a_toBeSet.GetComponent<NetworkIdentity>().AssignClientAuthority(a_client.GetComponent<NetworkIdentity>().connectionToClient);
        }
    }

    /// <summary>
    /// Revoke the authority from an object
    /// </summary>
    /// <param name="a_toBeSet"></param>
    /// <param name="a_client"></param>
    public static void RevokeAuthority(GameObject a_toBeSet, GameObject a_client)
    {
        if (NetworkServer.active)
        {
            a_toBeSet.GetComponent<NetworkIdentity>().RemoveClientAuthority(a_client.GetComponent<NetworkIdentity>().connectionToClient);
        }
    }
}
