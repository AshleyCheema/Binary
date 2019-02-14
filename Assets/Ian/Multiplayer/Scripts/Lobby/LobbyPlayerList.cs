/*
 * Author: Ian Hudson
 * Description: 
 * Created: 14/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerList : MonoBehaviour
{
    private static LobbyPlayerList instance;
    public static LobbyPlayerList Instance
    { get { return instance; } }

    public RectTransform playerListContent;

    protected VerticalLayoutGroup layout;
    protected List<LobbyPlayer> players = new List<LobbyPlayer>();

    /// <summary>
    /// Onenable
    /// </summary>
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        layout = playerListContent.GetComponent<VerticalLayoutGroup>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if(layout)
        {
            layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }
    }

    /// <summary>
    /// Add a new player to the lobby
    /// </summary>
    /// <param name="a_player"></param>
    public void AddPlayer(LobbyPlayer a_player)
    {
        if(players.Contains(a_player))
        {
            return;
        }

        players.Add(a_player);

        a_player.transform.SetParent(playerListContent, false);
    }

    /// <summary>
    /// Remove a player from the lobby
    /// </summary>
    /// <param name="a_player"></param>
    public void RemovePlayer(LobbyPlayer a_player)
    {
        players.Remove(a_player);
    }
}
