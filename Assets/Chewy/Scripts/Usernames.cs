using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Usernames : MonoBehaviour
{
    protected string username;
    public bool isSpy;

    // Update is called once per frame
    void Update()
    {
        if(!isSpy)
        {
            username = gameObject.GetComponentInChildren<TMP_InputField>().text;
            PlayerPrefs.SetString("MercName", username);
        }
        else
        {
            username = gameObject.GetComponentInChildren<TMP_InputField>().text;
            PlayerPrefs.SetString("SpyName", username);
        }
    }
}
