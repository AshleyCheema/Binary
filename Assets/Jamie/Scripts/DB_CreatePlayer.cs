using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePlayer : MonoBehaviour
{
    public InputField nameField;

    private IEnumerator AddPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);

        WWW www = new WWW("http://retrogecko.studentsites.glos.ac.uk/binary/createplayer.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("User Created Successfully.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("sqlmenu");
        }
        else
        {
            Debug.Log("User Creation failed. Error #" + www.text);
        }
    }
}