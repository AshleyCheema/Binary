using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    public Image CooldownImage;
    public float CooldownTime;
    public Button Action;

    private float currentCooldown = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Action.interactable = false;
        currentCooldown = 0;
        CooldownImage.fillAmount = Mathf.InverseLerp(0, 1, CooldownTime);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentCooldown < CooldownTime)
        {
            CooldownImage.color = new Color32(102, 102, 102, 255);
            currentCooldown += Time.deltaTime;
            CooldownImage.fillAmount = currentCooldown / CooldownTime;
        }
        if (CooldownImage.fillAmount == 1 && Action.interactable == false)
        {
            Action.interactable = true;
            //Debug.Log("Cooldown finished");
            CooldownImage.color = new Color32(103, 201, 255, 255);
            Invoke("ResetColour", 0.25F);
        }
    }

    private void ResetColour()
    {
        CooldownImage.color = new Color32(255, 255, 255, 255);
    }

    public void ActionClicked()
    {
        //Debug.Log("Skill '" + Action.name + "' has been clicked");
        Action.interactable = false;
        currentCooldown = 0;
    }
}