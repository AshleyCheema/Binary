using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trackable", menuName = "Trackable")]
public class TracbleableTest : Abilities
{
    [SerializeField]
    private GameObject trackerPrefab;
    [SerializeField]
    private GameObject tracker;

    public override void Trigger()
    {
        if (tracker == null)
        {
            tracker = Instantiate(trackerPrefab);
            //tracker.GetComponent<Trigger>().parent = this;
        }
        Debug.Log("The tracker has been triggered Get out the way");
    }

    public override void Callback()
    {
        Debug.Log("Spy has been seen");
    }
}
