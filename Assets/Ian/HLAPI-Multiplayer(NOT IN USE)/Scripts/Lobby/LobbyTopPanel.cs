/*
 * Author: Ian Hudson
 * Description: 
 * Created: 14/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyTopPanel : MonoBehaviour
{
    private bool isInGame = false;
    public bool IsInGame
    { get { return isInGame; } set { isInGame = value; } }

    protected bool isDisplayed = true;

    [SerializeField]
    private Image panelImage;


    // Update is called once per frame
    void Update()
    {
        if(!isInGame)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleVisibility(!isDisplayed);
        }
    }

    public void ToggleVisibility(bool a_visable)
    {
        isDisplayed = a_visable;
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(isDisplayed);
        }

        if(panelImage != null)
        {
            panelImage.enabled = isDisplayed;
        }
    }
}
