using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MiniModule_Lobby : Singleton<MiniModule_Lobby>
{
    public void OnLobbyPlayerAdd(LocalPlayer aPlayer, bool aIsLocal = false)
    {
        GameObject go = Instantiate(MiniModule_SpawableObjects.Instance.SpawnableObjects.ObjectsToSpawn[1], 
                                    CS_LobbyManager.Instance.transform);
        aPlayer.lobbyAvatar = go;
        CS_LobbyManager.Instance.AddLobbyPlayer(aPlayer, aIsLocal);
    }

    public void OnLobbyNameChange(LocalPlayer aPlayer)
    {

    }
}
