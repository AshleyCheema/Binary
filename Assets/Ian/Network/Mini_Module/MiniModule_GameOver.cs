using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check if the game is over
/// </summary>
public class MiniModule_GameOver : Singleton<MiniModule_GameOver>
{
    //list all the spyies who have "exited" the level
    public List<int> spyiesCompleted = new List<int>();


    public void SpyExitedLevel(int a_spy)
    {
        bool newSpy = false;
        if(!spyiesCompleted.Contains(a_spy))
        {
            newSpy = true;
            spyiesCompleted.Add(a_spy);
        }

        // if false then all the spyies have exited the level
        if(!newSpy)
        {
            Debug.Log("Spyies have left the level");
        }
    }
}
