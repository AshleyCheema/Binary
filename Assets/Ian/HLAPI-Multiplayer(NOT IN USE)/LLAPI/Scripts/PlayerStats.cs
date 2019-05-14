using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    public string PlayerName;

    public LLAPI.Team PlayerTeam;

    public bool HasWon;

    public int Steps;

    public int ShotsFired;

    public int AbililitesUsed;

    public float CaptureedAmount;
}
