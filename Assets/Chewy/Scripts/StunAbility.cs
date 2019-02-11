using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAbility : MonoBehaviour
{
    private GameObject stunG;
    private bool stunActive;
    private float cooldown;
    private float abilityDuration;

    public Abilities stunAbility;

    // Start is called before the first frame update
    void Start()
    {
        stunG = GameObject.Find("StunG");
        stunG.SetActive(false);
        cooldown = stunAbility.cooldown;
        abilityDuration = stunAbility.abilityDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!stunActive)
            {
                stunG.SetActive(true);
                stunG.transform.position = gameObject.transform.position;
                abilityDuration -= Time.deltaTime;

                if (abilityDuration <= 0)
                {
                    stunActive = true;
                    abilityDuration = stunAbility.abilityDuration;
                    stunG.SetActive(false);
                }
            }
        }

        if(stunActive)
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                stunActive = false;
            }
            //Flash Effect
            //Stun Animation
            //Slow Merc
        }
    }
}
