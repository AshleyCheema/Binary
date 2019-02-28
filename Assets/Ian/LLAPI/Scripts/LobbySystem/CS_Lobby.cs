using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public class CS_Lobby : MonoBehaviour
{
    [SerializeField]
    GameObject mercLayout;

    [SerializeField]
    GameObject spyLayout;

    [SerializeField]
    GameObject unassignedLayout;

    public void AddMercPlayer(GameObject a_go)
    {
        a_go.transform.SetParent(mercLayout.transform);
    }

    public void AddSpyPlayer(GameObject a_go)
    {
        a_go.transform.SetParent(spyLayout.transform);
    }

    public void AddUnassignedPlayer(GameObject a_go)
    {
        a_go.transform.SetParent(unassignedLayout.transform);
    }

    public void MercTeamSelected()
    {
        CS_LobbyManager.Instance.Client.LocalPlayer.team = LLAPI.Team.Merc;
        CS_LobbyManager.Instance.SetPlayerTeam(CS_LobbyManager.Instance.Client.LocalPlayer);
        SendTeamChange(CS_LobbyManager.Instance.Client.LocalPlayer);
    }

    public void SpyTeamSelected()
    {
        CS_LobbyManager.Instance.Client.LocalPlayer.team = LLAPI.Team.Spy;
        CS_LobbyManager.Instance.SetPlayerTeam(CS_LobbyManager.Instance.Client.LocalPlayer);
        SendTeamChange(CS_LobbyManager.Instance.Client.LocalPlayer);
    }

    private void SendTeamChange(LLAPI.Player a_p)
    {
        NetMsg_TeamChangeLB lb = new NetMsg_TeamChangeLB();
        lb.ConnectionID = a_p.connectionId;
        lb.Team = a_p.team;

        CS_LobbyManager.Instance.Client.Send(lb);
    }
}
