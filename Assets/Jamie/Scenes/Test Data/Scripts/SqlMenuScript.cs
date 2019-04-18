using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SqlMenuScript : MonoBehaviour
{
    public Button registerButton;
    public Button loginButton;
    public Button playButton;

    public TMP_Text playerDisplay;

    private void Start()
    {
        if (DBManager.LoggedIn)
        {
            playerDisplay.text = "Player: " + DBManager.username;
        }

        registerButton.interactable = !DBManager.LoggedIn;
        loginButton.interactable = !DBManager.LoggedIn;
        playButton.interactable = DBManager.LoggedIn;
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene("sqlreg");
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("sqllogin");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("sqlgame");
    }
}