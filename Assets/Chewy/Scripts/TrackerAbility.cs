using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerAbility : MonoBehaviour
{
    public Abilities tracker;
    private float cooldown;
    private bool isCooldown;
    private float a_Duration;
    private GameObject throwable;
    private bool trackerDown;
    //private bool isThrowing;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = tracker.cooldown;
        isCooldown = tracker.isCooldown;
        a_Duration = tracker.abilityDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCooldown && a_Duration == tracker.abilityDuration)
            {
                throwable.SetActive(true);
                isCooldown = true;
            }
            else
            {
                isCooldown = false;
                throwable.SetActive(false);
            }
        }

        if(trackerDown)
        {
            a_Duration -= Time.deltaTime;

            if(a_Duration <= 0)
            {
                trackerDown = false;
                a_Duration = tracker.abilityDuration;
            }
        }

        if (isCooldown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float radius = 10;
                Vector3 centerPosition = transform.position;
                float distance = Vector3.Distance(hit.point, centerPosition);

                if (distance > radius)
                {
                    Vector3 fromOrigin = hit.point - centerPosition;
                    fromOrigin *= radius / distance;
                    hit.point = centerPosition + fromOrigin;
                }

                throwable.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

                if (Input.GetMouseButtonDown(1))
                {
                    isCooldown = false;
                    trackerDown = true;
                }
            }
        }
    }
}
