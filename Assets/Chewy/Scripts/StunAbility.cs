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

    public ParticleSystem flash;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!stunActive)
            {
                stunG.SetActive(true);
                stunG.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                stunDropped = true;
            }
        }

        if(stunActive)
        {
            abilityDuration = stunAbility.abilityDuration;
            stunG.SetActive(false);
            stunDropped = false;

            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                stunActive = false;
            }

        }

        if (stunDropped)
        {
            abilityDuration -= Time.deltaTime;
            if (abilityDuration <= 0)
            {
                flash.Play();
                #region NetMsg_Stun
                NetMsg_AB_Stun ab_Stun = new NetMsg_AB_Stun();
                ab_Stun.ConnectionID = client.ServerConnectionId;
                ab_Stun.StunObjectIndex = 4;
                //ab_Stun.StunParticle
                ab_Stun.Stunned = trigger.isStunned;
                client.Send(ab_Stun);
                #endregion
                //Flash Effect
            }
            if(abilityDuration <= -1)
            {
                stunActive = true;
            }
        }
    }
}
