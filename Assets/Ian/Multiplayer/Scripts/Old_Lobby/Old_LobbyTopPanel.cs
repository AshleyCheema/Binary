/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyTopPanel : MonoBehaviour
{
    private bool isInGame = false;
    public bool IsInGame
    { get { return isInGame; } set { isInGame = value; } }

    protected bool isDisplayed = true;
    protected Image panelImage;

    void Start()
    {
        panelImage = GetComponent<Image>();
    }


    void Update()
    {
        if (!isInGame)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleVisibility(!isDisplayed);
        }

    }

    public void ToggleVisibility(bool visible)
    {
        isDisplayed = visible;
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(isDisplayed);
        }

        if (panelImage != null)
        {
            panelImage.enabled = isDisplayed;
        }
    }
}
*/