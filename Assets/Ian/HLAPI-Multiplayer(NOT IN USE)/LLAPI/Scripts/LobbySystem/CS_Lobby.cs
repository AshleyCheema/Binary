using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;

public class CS_Lobby : Singleton<CS_Lobby>
{
    [SerializeField]
    GameObject mercLayout = null;

    [SerializeField]
    GameObject spyLayout = null;

    [SerializeField]
    GameObject unassignedLayout = null;

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
        CS_LobbyManager.Instance.Client.LocalPlayer.playerTeam = LLAPI.Team.Merc;
        CS_LobbyManager.Instance.SetPlayerTeam(CS_LobbyManager.Instance.Client.LocalPlayer);
        SendTeamChange(CS_LobbyManager.Instance.Client.LocalPlayer);
    }

    public void SpyTeamSelected()
    {
        CS_LobbyManager.Instance.Client.LocalPlayer.playerTeam = LLAPI.Team.Spy;
        CS_LobbyManager.Instance.SetPlayerTeam(CS_LobbyManager.Instance.Client.LocalPlayer);
        SendTeamChange(CS_LobbyManager.Instance.Client.LocalPlayer);
    }

    public void SetPlayerTeam(LocalPlayer aPlayer)
    {
        CS_LobbyManager.Instance.SetPlayerTeam(aPlayer);
    }

    public void SendTeamChange(LocalPlayer aPlayer)
    {
        Msg_ClientTeamChange ctc = new Msg_ClientTeamChange();
        ctc.ConnectionID = aPlayer.connectionId;
        ctc.Team = aPlayer.playerTeam;

        ClientManager.Instance?.client.Send(MSGTYPE.LOBBY_TEAM_CHANGE, ctc);

        PlayerStats.Instance.PlayerTeam = aPlayer.playerTeam;

        //NetMsg_TeamChangeLB lb = new NetMsg_TeamChangeLB();
        //lb.ConnectionID = a_p.connectionId;
        //lb.Team = a_p.team;
        //
        //CS_LobbyManager.Instance.Client.Send(lb);
    }
}
