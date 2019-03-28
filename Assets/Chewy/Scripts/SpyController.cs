using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpyState
{
    Normal,
    Hurt,
    Dead
}

public class SpyController : PlayerController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    public GameObject stun;
    private Trigger bulletTrigger;
    private SpyState currentState;
    public bool stunDrop;
    // Start is called before the first frame update
    public override void Start()
    {
        bullet = GameObject.Find("Bullet");
        if (stun == null)
        {
            stun = GameObject.Find("StunG");
        }
        bulletTrigger = bullet.GetComponent<Trigger>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(bulletTrigger.hasShot)
        {
            if (currentState == SpyState.Normal)
            {
                //Change animation
                //Maybe drip blood?
                Debug.Log("Hurt State");
                currentState = SpyState.Hurt;
                bulletTrigger.hasShot = false;
            }
            else
            {
                bulletTrigger.hasShot = false;
                currentState = SpyState.Dead;
                Debug.Log("Die");

                //Send message to host
                //spy is daed
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (stun.GetComponent<StunAbility>().IsActive == false)
            {
                stun.GetComponent<StunAbility>().IsActive = true;
                stunDrop = true;
            }
        }
    }
}
