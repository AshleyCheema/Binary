using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Abilities : ScriptableObject
{
    public new string name;
    public AudioClip aClip;
    public float cooldown;
    public bool isCooldown;
    public float abilityDuration;
    public Sprite uiImage;

    public virtual void Trigger()
    {
        Debug.Log("Ths has been triggered");
    }

    public virtual void Callback()
    {

    }
}
