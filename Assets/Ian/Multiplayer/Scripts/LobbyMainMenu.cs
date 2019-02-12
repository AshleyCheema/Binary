using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class LobbyMainMenu : MonoBehaviour
{
    /// <summary>
    /// Ref the lobby manager
    /// </summary>
    [SerializeField]
    private LobbyManager lobbyManager;
    public LobbyManager Lobbymanager => lobbyManager;

    /// <summary>
    /// Ref to the Rect transform of the server list
    /// </summary>
    [SerializeField]
    private RectTransform lobbyServerList;

    /// <summary>
    /// Ref to the lobby panel
    /// </summary>
    [SerializeField]
    private RectTransform lobbyPanel;

    /// <summary>
    /// IP input
    /// </summary>
    [SerializeField]
    private InputField ipInput;

    /// <summary>
    /// Game name input
    /// </summary>
    [SerializeField]
    private InputField gameNameInput;

    /// <summary>
    /// When this object is enabled this function is called
    /// </summary>
    private void OnEnable()
    {
        //show the top panel
        lobbyManager.TopPanel.ToggleVisibility(true);

        //Remove all the listeners to the input field of ipInput. Then add a new listener
        //to ipInput
        ipInput.onEndEdit.RemoveAllListeners();
        ipInput.onEndEdit.AddListener(onEndEditIP);

        //Remove all the listeners to the input field of matchNameInput. Then add a new listener
        //to matchNameInput
        gameNameInput.onEndEdit.RemoveAllListeners();
        gameNameInput.onEndEdit.AddListener(onEndEditGameName);
    }

    /// <summary>
    /// When the host button is clicked this function is called
    /// </summary>
    public void OnClickHost()
    {
        lobbyManager.StartHost();
    }

    /// <summary>
    /// When the join button is clicked this function is called
    /// </summary>
    public void OnClickJoin()
    {
        //Change the UI to show the lobby panel
        lobbyManager.ChangeTo(lobbyPanel);

        //edit the IP address and start a new client
        lobbyManager.networkAddress = ipInput.text;
        lobbyManager.StartClient();

        //Set the back delegate for when the back button is 
        lobbyManager.backDelegate = lobbyManager.StopClientCallback;
        lobbyManager.DisplayIsConnecting();

        lobbyManager.SetServerInfo("Client is connecting to: ", lobbyManager.networkAddress);
    }

    /// <summary>
    /// When the dedicated button is clicked this function is called
    /// </summary>
    public void OnClickDeedicated()
    {
        lobbyManager.ChangeTo(null);
        lobbyManager.SetServer();

        //Set the back delegate for when the back button is 
        lobbyManager.backDelegate = lobbyManager.StopServerCallback;

        lobbyManager.SetServerInfo("Dedicated server:", lobbyManager.networkAddress);
    }

    /// <summary>
    /// Create a matchmaking game
    /// </summary>
    public void OnClickCreateMatchmakingGame()
    {
        lobbyManager.StartMatchMaker();
        lobbyManager.matchMaker.CreateMatch
        (
            gameNameInput.text,
            lobbyManager.maxPlayer,
            true,
            "", "", "", 0, 0,
            lobbyManager.OnMatchCreate
        );

        lobbyManager.backDelegate = lobbyManager.StopHostCallback;
        lobbyManager.isMathcmaking = true;
        lobbyManager.DisplayIsConnecting();

        lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
    }

    /// <summary>
    /// open the server list of all available servers
    /// </summary>
    public void OnClickOpenServerList()
    {
        lobbyManager.StartMatchMaker();
        lobbyManager.backDelegate = lobbyManager.SimpleBackCallback;
        lobbyManager.ChangeTo(lobbyServerList);
    }

    /// <summary>
    /// When the IP input field has finished being edited call this
    /// </summary>
    /// <param name="a_text"></param>
    private void onEndEditIP(string a_text)
    {
        if(InputManager.GetReturn())
        {
            OnClickJoin();
        }
    }

    /// <summary>
    /// When the gameName input field has finished being edited call this
    /// </summary>
    /// <param name="a_text"></param>
    private void onEndEditGameName(string a_text)
    {
        if (InputManager.GetReturn())
        {
            OnClickCreateMatchmakingGame();
        }
    }
}
*/