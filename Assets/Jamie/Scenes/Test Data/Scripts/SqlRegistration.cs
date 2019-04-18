using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SqlRegistration : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        //WWW www = new WWW("http://localhost/sqlconnect/register.php", form);
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

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}