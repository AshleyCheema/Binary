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
    private Camera playerCameraPrefab;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private string playerName;

    int playerId = 1;

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
        if (connectionToClient.isReady)
        {
            CmdSpawn();
        }
        else
        {
            StartCoroutine(WaitForRead());
        }
    }

    IEnumerator WaitForRead()
    {
        while (!connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }

        CmdSpawn();
    }

    [Command]
    void CmdSpawn()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Points Spy's");
        playerObject = Instantiate(playerObjectPrefab);

        playerObject.GetComponentInChildren<PlayerController_Net>().PlayerObject = this;
        NetworkServer.SpawnWithClientAuthority(playerObject, connectionToClient);

        string tag = "Spy" + playerId;
        RpcSetPLayerObject(playerObject, tag);

        //Tell the server where this playerObject is 
        playerObject.transform.position = spawnPoints[playerId].transform.position;
        Debug.Log("PLAYER SPAWNED AT " + playerObject.transform.position);
        //RpcChangePlayerPosition(spawnPoints[playerId].transform.position);

        playerId += 1;
    }

    [ClientRpc]
    private void RpcSetPLayerObject(GameObject a_go, string a_tag)
    {
        if (isLocalPlayer)
        {
            a_go.tag = a_tag;
        }
        //if(isLocalPlayer)
        //{
        //    a_go.GetComponent<PlayerController_Net>().PlayerObject = this;
        //}
    }

    [ClientRpc]
    private void RpcChangePlayerPosition(Vector3 a_position)
    {
        //tell all the clients where this playerObject is
        playerObject.transform.position = a_position;
        Debug.Log("RpcChangePlayerPosition " + playerObject.transform.position);
    }

    [ClientRpc]
    private void RpcChangePlayerCamera()
    {
        //playerCamera = Instantiate(playerCameraPrefab,
        //                    playerObject.transform.position + new Vector3(-1.5f, 3.55f, 0.74f),
        //                    Quaternion.Euler(30.0f, 40.0f, 0.0f));

        //playerCamera.GetComponent<CameraScript>().target = playerObject.transform;
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
