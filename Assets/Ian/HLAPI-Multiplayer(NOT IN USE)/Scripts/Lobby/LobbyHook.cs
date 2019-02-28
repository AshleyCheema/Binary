/*
 * Author: Ian Hudson
 * Description: 
 * Created: 14/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class LobbyHook : MonoBehaviour
{
    public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager a_manager, GameObject a_lobbyPlayer, GameObject a_gamePlayer) { }
}
