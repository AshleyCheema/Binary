using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyController : PlayerController
{
    [SerializeField]
    private GameObject bullet;
    private Trigger bulletTrigger;
    private bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        bulletTrigger = bullet.GetComponent<Trigger>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(bulletTrigger.hasShot)
        {
            //Change animation
            //Maybe drip blood?
            Debug.Log("Hurt State");
            isHurt = true;
            bulletTrigger.hasShot = false;
        }

        if(isHurt && bulletTrigger.hasShot)
        {
            //Die?
        }
    }
}
