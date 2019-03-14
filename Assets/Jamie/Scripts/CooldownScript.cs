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
    public SpyController spyController;
    public MercControls mercControls;
    public TrackerAbility trackerAbility;

    private bool isSpy;
    private bool isMerc;
    public AbilityType abilityType;

    public enum AbilityType
    {
        //Spy
        SPRINT,
        STUN,
        //Merc
        TRACKER,
        BURST,
        FIRE
    }

    // Start is called before the first frame update
    private void Start()
    {

        if(mercControls != null)
        {
            isMerc = true;
        }
        if(spyController != null)
        {
            isSpy = true;
        }
        Action.interactable = false;
        currentCooldown = CooldownTime;
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

        if (isSpy)
        {
            //Debug.Log("Skill '" + Action.name + "' has been clicked");
            if (spyController.isRunning && Action.interactable == true && abilityType == AbilityType.SPRINT)
            {
                Action.interactable = false;
                currentCooldown = 0;
            }
            if(spyController.stunDrop && Action.interactable == true && abilityType == AbilityType.STUN)
            {
                Action.interactable = false;
                currentCooldown = 0;
            }
        }

        if (isMerc)
        {
            if (mercControls.noShoot && Action.interactable == true && abilityType == AbilityType.FIRE)
            {
                Action.interactable = false;
                currentCooldown = 0;
            }
            if (!mercControls.canSprint && Action.interactable == true && abilityType == AbilityType.BURST)
            {
                Action.interactable = false;
                currentCooldown = 0;
            }
            if (trackerAbility.trackerDown && Action.interactable == true && abilityType == AbilityType.TRACKER)
            {
                Action.interactable = false;
                currentCooldown = 0;
            }
        }
    }

    private void ResetColour()
    {
        CooldownImage.color = new Color32(255, 255, 255, 255);
    }

}