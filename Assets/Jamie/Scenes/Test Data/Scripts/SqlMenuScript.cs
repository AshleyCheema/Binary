using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SqlMenuScript : MonoBehaviour
{
    public void GoToRegister()
    {
        SceneManager.LoadScene(1);
    }
}