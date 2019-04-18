using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_AddMatchRecord : MonoBehaviour
{
    public string gameDuration;
    public string gameWinner;

    public void CallAddMatch()
    {
        StartCoroutine(addMatch());
    }

    private IEnumerator addMatch()
    {
        WWWForm form = new WWWForm();

        form.AddField("duration", gameDuration);
        form.AddField("winner", gameWinner);

        WWW www = new WWW("http://retrogecko.studentsites.glos.ac.uk/binary/addmatch.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Match stats added to DB successfully");
        }
        else
        {
            Debug.Log("Match stats failed. Error #" + www.text);
        }
    }
}