using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SqlGame : MonoBehaviour
{
    public TMP_Text playerDisplay;
    public TMP_Text scoreDisplay;

    private void Awake()
    {
        if (DBManager.username == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        playerDisplay.text = "Player: " + DBManager.username;
        scoreDisplay.text = "Score: " + DBManager.score;
    }

    public void CallSaveData()
    {
        StartCoroutine(SavePlayerData());
    }

    private IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("score", DBManager.score);

        //WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        WWW www = new WWW("http://retrogecko.studentsites.glos.ac.uk/binary/savedata.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("Game Saved");
        }
        else
        {
            Debug.Log("Save Failed. Error #" + www.text);
        }

        DBManager.LogOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene("sqlmenu");
    }

    public void IncreaseScore()
    {
        DBManager.score++;
        scoreDisplay.text = "Score: " + DBManager.score;
    }
}