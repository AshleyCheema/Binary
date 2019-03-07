/*
 * Author: Ian Hudson
 * Description: HLAPI - This script is used for a mini game which is shown to the player.
 * The mini game is based on Simon Says.
 * Created: 20/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CapturePointMiniGame : NetworkBehaviour
{
    [SerializeField]
    private CapturePoint capturePoint;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private ScrollRect scroll;

    [SerializeField]
    private KeyCode[] inputsNeeded =
    {
        KeyCode.UpArrow,
        KeyCode.LeftArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.RightArrow,
        KeyCode.RightArrow,
        KeyCode.RightArrow,
    };

    [SerializeField]
    private int inputsIndex = 0;

    [SerializeField]
    private bool isCompleted = false;

    private Camera camera;

    [SerializeField]
    private GameObject billboardUI;

    /// <summary>
    /// Show this mini game
    /// </summary>
    public void Show()
    {
        parent.SetActive(true);
    }

    /// <summary>
    /// Hide this mini game
    /// </summary>
    public void Hide()
    {
        parent.SetActive(false);

        if(!isCompleted)
        {
            inputsIndex = 0;
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (hasAuthority)
        //{
            if (!isCompleted && Input.GetKeyDown(inputsNeeded[inputsIndex]))
            {
                inputsIndex++;
                if (inputsIndex == 1)
                {
                    scroll.horizontalNormalizedPosition = 0;
                }

                scroll.horizontalNormalizedPosition += 1f / inputsNeeded.Length;

                if (inputsIndex > inputsNeeded.Length - 1)
                {
                    //isCompleted = true;
                    inputsIndex = 0;
                    capturePoint.CmdIncreaseMultiplier();
                }
            }
            else if (!isCompleted && Input.anyKeyDown && CheckInput())
            {
                capturePoint.ResetMulitiplier();
                inputsIndex = 0;
                scroll.horizontalNormalizedPosition = 0;
            }
       // }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool CheckInput()
    {
        for (int i = 0; i < inputsNeeded.Length; i++)
        {
            if(Input.GetKeyDown(inputsNeeded[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a_array"></param>
    /// <param name="a_value"></param>
    /// <returns></returns>
    private bool ArrayContains<T>(T[] a_array, T a_value)
    {
        for (int i = 0; i < a_array.Length; i++)
        {
            if(a_array[i].Equals(a_value))
            {
                return true;
            }
        }
        return false;
    }
}
