using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class StunAbility : MonoBehaviour
{
    private GameObject stunG;
    public bool stunActive;
    private bool stunDropped;
    private float cooldown;
    private float abilityDuration;
    private Trigger trigger;
    protected Client client;
    public Abilities stunAbility;

    // Start is called before the first frame update
    void Start()
    {
        trigger = gameObject.GetComponent<Trigger>();
        client = FindObjectOfType<Client>();
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
            if (!stunDropped)
            {
                stunG.SetActive(true);
                stunG.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                stunDropped = true;
            }
        }

        if(stunDropped)
        {
            abilityDuration -= Time.deltaTime;

            if (abilityDuration <= 0)
            {
                stunActive = true;
                abilityDuration = stunAbility.abilityDuration;
                #region NetMsg_Stun
                NetMsg_AB_Stun ab_Stun = new NetMsg_AB_Stun();
                ab_Stun.ConnectionID = client.ServerConnectionId;
                ab_Stun.StunObject = stunG;
                //ab_Stun.StunParticle
                ab_Stun.Stunned = trigger.isStunned;
                client.Send(ab_Stun);
                #endregion
                //Flash Effect
            }
        }

        if(stunActive)
        {
            stunG.SetActive(false);
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                stunDropped = false;
            }

        }
    }
}
