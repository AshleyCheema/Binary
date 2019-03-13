using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLAPI;
public class StunAbility : Cooldown
{
    private GameObject stunG;
    public bool stunActive;
    public bool stunDropped;
    //private float cooldown;
    private float abilityDuration;
    private Trigger trigger;
    protected Client client;
    public Abilities stunAbility;
    private GameObject spyController;
    private SpyController spyControllerSc;
    public ParticleSystem flash;

    public bool IsActive
    {
        get
        {
            return GetComponent<MeshRenderer>().enabled;
        }

        set
        {
            if(value == false)
            {
                foreach (var item in GetComponents<Collider>())
                {
                    item.enabled = false;
                }

                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                foreach (var item in GetComponents<Collider>())
                {
                    item.enabled = true;
                }

                GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    public bool isSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        stunG = GameObject.Find("StunG");
        spyController = GameObject.FindGameObjectWithTag("Spy");
        if (spyController)
        {
            spyControllerSc = spyController.GetComponent<SpyController>();
            if (stunG != null)
            {
                stunG.SetActive(false);
            }
        }
        flash = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        trigger = gameObject.GetComponent<Trigger>();
        cooldown = stunAbility.cooldown;
        abilityDuration = stunAbility.abilityDuration;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (spyControllerSc != null && spyControllerSc.stunDrop)
        {
            if (!stunActive)
            {
                stunG.transform.position = new Vector3(spyController.transform.position.x,
                                                       spyController.transform.position.y,
                                                       spyController.transform.position.z - 1);
                stunDropped = true;
                spyControllerSc.stunDrop = false;

                isCooldown = true;
                #region NetMsg_Stun

                NetMsg_AB_Stun ab_Stun = new NetMsg_AB_Stun();
                ab_Stun.ConnectionID = client.ServerConnectionId;
                ab_Stun.StunObjectIndex = 4;
                //ab_Stun.StunParticle
                ab_Stun.Stunned = trigger.isStunned;
                client.Send(ab_Stun);

                #endregion
            }
        }
        else if(spyControllerSc == null)
        {
            spyController = GameObject.FindGameObjectWithTag("Spy");
            if (spyController)
            {
                spyControllerSc = spyController.GetComponent<SpyController>();
                if (stunG != null)
                {
                    spyControllerSc.stun = stunG;
                    //stunG.SetActive(false);
                    IsActive = false;
                }
            }
        }

        if(stunActive)
        {
            abilityDuration = stunAbility.abilityDuration;
            //stunG.SetActive(false);
            IsActive = false;
            stunDropped = false;
            flash.Stop();
            if(!isCooldown)
            {
                stunActive = false;
            }

        }

        if (stunDropped)
        {
            abilityDuration -= Time.deltaTime;

            if (abilityDuration <= -2)
            {
                stunActive = true;

                if(isSpawned)
                {
                    Destroy(gameObject);
                    //remove from list in clients
                }
            }

            if (abilityDuration <= 0)
            {
                flash.Play();

                if (!isSpawned)
                {
                    Collider[] coll = Physics.OverlapSphere(transform.position, stunG.GetComponent<SphereCollider>().radius);

                    for (int i = 0; i < coll.Length; i++)
                    {
                        foreach (var pKey in client.Players.Keys)
                        {
                            if (coll[i].gameObject == client.LocalPlayer.avater)
                            {

                            }
                            else if (coll[i].gameObject == client.Players[pKey].avater)
                            {
                                //Send message to player tell them that they are affected
                                NetMsg_AB_Trigger ab_trigger = new NetMsg_AB_Trigger();
                                ab_trigger.ConnectionID = client.Players[pKey].connectionId;
                                ab_trigger.Trigger = true;
                                ab_trigger.Type = LLAPI.TriggerType.STUN;

                                client.Send(ab_trigger);
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetShell()
    {
        flash = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
}
