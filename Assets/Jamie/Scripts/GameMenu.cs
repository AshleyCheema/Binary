using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        //method to hide the menu scene.
    }

    public void QuitGame()
    {
        //Load the main menu when called
        Scene MainMenu = SceneManager.GetSceneByName("MainMenu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}