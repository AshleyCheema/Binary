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
    private GameObject spyController;
    private SpyController spyControllerSc;
    public ParticleSystem flash;

    // Start is called before the first frame update
    void Start()
    {
        client = FindObjectOfType<Client>();
        stunG = GameObject.Find("StunG");
        spyController = GameObject.FindGameObjectWithTag("Spy");
        spyControllerSc = spyController.GetComponent<SpyController>();
        if (stunG != null)
        {
            stunG.SetActive(false);
        }
        flash = stunG.transform.GetChild(0).GetComponent<ParticleSystem>();
        trigger = stunG.GetComponent<Trigger>();
        cooldown = stunAbility.cooldown;
        abilityDuration = stunAbility.abilityDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (spyControllerSc.stunDrop)
        {
            if (!stunActive)
            {
                stunG.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                stunDropped = true;
                spyControllerSc.stunDrop = false;

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

        if(stunActive)
        {
            abilityDuration = stunAbility.abilityDuration;
            stunG.SetActive(false);
            stunDropped = false;
            flash.Stop();
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
            {
                stunActive = false;
            }

        }

        if (stunDropped)
        {
            abilityDuration -= Time.deltaTime;

            if (abilityDuration <= -1)
            {
                stunActive = true;
            }

            if (abilityDuration <= 0)
            {
                flash.Play();

                Collider[] coll = Physics.OverlapSphere(transform.position, stunG.GetComponent<SphereCollider>().radius);

                for (int i = 0; i < coll.Length; i++)
                {
                    foreach (var pKey in client.Players.Keys)
                    {
                        if (coll[i].gameObject == client.Players[pKey].avater)
                        {
                        }
                    }
                }
            }
        }
    }
}
