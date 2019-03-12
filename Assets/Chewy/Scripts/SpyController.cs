using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyController : PlayerController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    public GameObject stun;
    private Trigger bulletTrigger;
    private bool isHurt;
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
            if (!isHurt)
            {
                //Change animation
                //Maybe drip blood?
                Debug.Log("Hurt State");
                isHurt = true;
                bulletTrigger.hasShot = false;
            }
            else
            {
                bulletTrigger.hasShot = false;
                Debug.Log("Die");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stun.GetComponent<StunAbility>().IsActive = true;
            stunDrop = true;
        }
    }
}
