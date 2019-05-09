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
    private AudioSource audioSource;
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

                if (!isSpawned)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                }
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
                IsActive = false;
            }
        }
        audioSource = gameObject.GetComponent<AudioSource>();
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
            if (!stunActive && !isCooldown)
            {
                IsActive = true;
                stunG.transform.position = new Vector3(spyController.transform.position.x,
                                                       0,
                                                       spyController.transform.position.z - 1);
                stunDropped = true;
                isCooldown = true;
                #region NetMsg_Stun

                Msg_AB_ClientStun ab_Stun = new Msg_AB_ClientStun();
                ab_Stun.ConnectionID = ClientManager.Instance.LocalPlayer.connectionId;
                ab_Stun.StunObjectIndex = 4;
                //ab_Stun.StunParticle
                ab_Stun.Stunned = trigger.isStunned;
                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_STUN, ab_Stun);

                #endregion
            }
        }
        else if (spyControllerSc == null)
        {
            spyController = GameObject.FindGameObjectWithTag("Spy");
            if (spyController)
            {
                spyControllerSc = spyController.GetComponentInChildren<SpyController>();
                if (stunG != null)
                {
                    spyControllerSc.stun = stunG;
                    //stunG.SetActive(false);

                    IsActive = false;
                }
            }
        }

        if (stunActive)
        {
            abilityDuration = stunAbility.abilityDuration;
            //stunG.SetActive(false);
            IsActive = false;
            stunDropped = false;
            flash.Stop();
            if (!isCooldown)
            {
                spyControllerSc.stunDrop = false;
                cooldown = stunAbility.cooldown;
                stunActive = false;
            }

        }

        if (stunDropped)
        {
            abilityDuration -= Time.deltaTime;

            if (abilityDuration <= 0)
            {
                //flash.Play();
                stunActive = true;
                audioSource.Play();
                if (!isSpawned)
                {
                    Collider[] coll = Physics.OverlapSphere(transform.position, stunG.GetComponent<SphereCollider>().radius);

                    for (int i = 0; i < coll.Length; i++)
                    {
                        foreach (var pKey in ClientManager.Instance.Players.Keys)
                        {
                            if (coll[i].gameObject == ClientManager.Instance.LocalPlayer.gameAvatar)
                            {

                            }
                            else if (coll[i].gameObject == ClientManager.Instance.Players[pKey].gameAvatar)
                            {
                                //Send message to player tell them that they are affected
                                Msg_ClientTrigger ct = new Msg_ClientTrigger();
                                ct.ConnectionID = pKey;
                                ct.Trigger = true;
                                ct.Type = TriggerType.Stun;

                                ClientManager.Instance.client.Send(MSGTYPE.CLIENT_AB_TRIGGER, ct);
                                //NetMsg_AB_Trigger ab_trigger = new NetMsg_AB_Trigger();
                                //ab_trigger.ConnectionID = client.Players[pKey].connectionId;
                                //ab_trigger.Trigger = true;
                                //ab_trigger.Type = LLAPI.TriggerType.STUN;
                                //
                                //client.Send(ab_trigger);
                            }
                        }
                    }
                }
                if (isSpawned)
                {
                    Destroy(gameObject);
                    //remove from list in clients
                }
            }
        }
    }

    public void SetShell()
    {
        flash = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        flash.Play();
    }
}
