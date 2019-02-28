using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LLAPI;
using UnityEngine.SceneManagement;

public class CS_LobbyMainMenu : MonoBehaviour
{
    [SerializeField]
    private Button serverHost;
    [SerializeField]
    private TMP_InputField ipInput;
    [SerializeField]
    private Button connect;

    [SerializeField]
    RectTransform lobbyPanel;

    private void Start()
    {
        // DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        string ip = ipInput.text;
        CS_LobbyManager.Instance.Client.Connect(ip);
        CS_LobbyManager.Instance.ChangeTo(lobbyPanel);
    }

    public void ServerHost()
    {
        CS_LobbyManager.Instance.Client.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
