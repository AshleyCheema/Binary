using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_UpdatePlayer : MonoBehaviour
{
    public string playerName;
    public string matchResult;
    public string playerClass;

    public int stepsTaken;
    public int shotsFired;
    public int ablitiesUsed;
    public int pointsCaptured;

    public void CallUpdatePlayerStats()
    {
        StartCoroutine(updatePlayerStats());
    }

    private IEnumerator updatePlayerStats()
    {
        WWWForm form = new WWWForm();

        //form.AddField("duration", gameDuration);
        // form.AddField("winner", gameWinner);

        form.AddField("name", playerName);
        form.AddField("result", matchResult);
        form.AddField("class", playerClass);
        form.AddField("steps", stepsTaken);
        form.AddField("shots", shotsFired);
        form.AddField("abilities", ablitiesUsed);
        form.AddField("points", pointsCaptured);

        WWW www = new WWW("http://retrogecko.studentsites.glos.ac.uk/binary/updateplayer.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Player stats added to DB successfully");
        }
        else
        {
            Debug.Log("Player stats failed. Error #" + www.text);
        }
    }
}