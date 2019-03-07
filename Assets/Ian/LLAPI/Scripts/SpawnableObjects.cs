using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Spawnable Objects")]
public class SpawnableObjects : ScriptableObject
{
    public GameObject[] ObjectsToSpawn;
}
