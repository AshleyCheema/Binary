using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    public float cooldown;
    public bool isCooldown;

    // Update is called once per frame
    protected virtual void Update()
    {
        if(isCooldown)
        {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0)
            {
                isCooldown = false;
            }
        }



    }
}
