using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if the game is over
/// </summary>
public class MiniModule_GameOver : Singleton<MiniModule_GameOver>
{
    //list all the spyies who have "exited" the level
    public List<int> spyiesCompleted = new List<int>();
    public List<int> spyiesDead = new List<int>();

    public void SpyDead(int a_spy)
    {
        if (!spyiesDead.Contains(a_spy))
        {
            spyiesDead.Add(a_spy);
        }

        //if one spy has died change the capture amount for the server rooms
        if(spyiesDead.Count == 1)
        {
            //send message to change capture amount for server rooms
            //this is for host
            if(HostManager.Instance != null)
            {
                foreach (var item in HostManager.Instance.capturePoints)
                {
                    if(item.Value.GetComponent<NO_CapturePoint>().currentSpies.Contains(HostManager.Instance.Players[a_spy].gameAvatar))
                    {
                        item.Value.GetComponent<NO_CapturePoint>().currentSpies.Remove(HostManager.Instance.Players[a_spy].gameAvatar);
                    }
                    item.Value.GetComponent<NO_CapturePoint>().baseCaptureAmount = 5.0f;
                    item.Value.GetComponent<NO_CapturePoint>().maxCaptureAmount = 5.0f;
                    item.Value.GetComponent<NO_CapturePoint>().ResetCaptureAmount();
                }
                Msg_ClientCaptureAmountOR cca = new Msg_ClientCaptureAmountOR();
                cca.ConnectID = a_spy;
                cca.BaseCaptureAmount = 5.0f;
                cca.MaxCaptureAmount = 5.0f;
                HostManager.Instance.SendAll(MSGTYPE.CLIENT_CAPTURE_AMOUNT_OR, cca);
            }
        }

        bool allSpyiesDead = true;
        foreach (var key in HostManager.Instance.Players.Keys)
        {
            if (HostManager.Instance.Players[key].playerTeam == LLAPI.Team.Spy)
            {
                if (!spyiesDead.Contains(HostManager.Instance.Players[key].connectionId))
                {
                    //there are more spyies in the level
                    allSpyiesDead = false;
                    break;
                }
            }
        }

        if(allSpyiesDead)
        {
            Debug.Log("All spys have diead");
            //Send mesage tell all clients to fade in game over screen
            //send all imformation if need be
            Msg_ClientGameOver cgo = new Msg_ClientGameOver();
            cgo.spiesWon = false;
            HostManager.Instance?.SendAll(MSGTYPE.CLIENT_GAME_OVER, cgo);

            HostManager.Instance?.OnGameLoadLobby();

            spyiesDead.Clear();
        }
    }

    public void SpyExitedLevel(int a_spy)
    {
        if(!spyiesCompleted.Contains(a_spy))
        {
            spyiesCompleted.Add(a_spy);

            //send message to other players
            Msg_ClientHideSpy chs = new Msg_ClientHideSpy();
            chs.SpyToHide = a_spy;
            HostManager.Instance.Send(a_spy, MSGTYPE.CLIENT_HIDE_SPY, chs, false);
        }

        // if false then all the spyies have exited the level
        if(spyiesCompleted.Count == 2)
        {
            Debug.Log("Spyies have left the level");
            Msg_ClientGameOver cgo = new Msg_ClientGameOver();
            cgo.spiesWon = true;
            HostManager.Instance?.SendAll(MSGTYPE.CLIENT_GAME_OVER, cgo);

            HostManager.Instance?.OnGameLoadLobby();

            spyiesCompleted.Clear();
        }

        if(spyiesDead.Count == 1)
        {
            Debug.Log("Spyies have left the level");
            Msg_ClientGameOver cgo = new Msg_ClientGameOver();
            cgo.spiesWon = true;
            HostManager.Instance?.SendAll(MSGTYPE.CLIENT_GAME_OVER, cgo);

            HostManager.Instance?.OnGameLoadLobby();

            spyiesCompleted.Clear();
        }
    }
}
