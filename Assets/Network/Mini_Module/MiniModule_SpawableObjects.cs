using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniModule_SpawableObjects : Singleton<MiniModule_SpawableObjects>
{
    [SerializeField]
    private SpawnableObjects spawnableObjects;
    public SpawnableObjects SpawnableObjects
    { get { return spawnableObjects; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
