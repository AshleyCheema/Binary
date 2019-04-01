/*
 * Author: Ian Hudson
 * Description: HLAPI - This script is used for a mini game which is shown to the player.
 * The mini game is based on Simon Says. Now used in LLAPI
 * Created: 20/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointMiniGame : MonoBehaviour
{
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
    public bool IsCompleted
    { get { return isCompleted; } }

    [SerializeField]
    private NO_CapturePoint parentCapturePoint;

    private Camera camera;

    [SerializeField]
    private GameObject billboardUI;

    /// <summary>
    /// Show this mini game
    /// </summary>
    public void Show()
    {
        parent.SetActive(true);
        parent.transform.LookAt(parent.transform.position+ Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
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

    /// <summary>
    /// Update is called once per frame
    /// </summary>
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

                scroll.horizontalNormalizedPosition += 1.0f / (inputsNeeded.Length + 1);

                if (inputsIndex > inputsNeeded.Length - 1)
                {
                    isCompleted = true;
                    inputsIndex = 0;

                    //Mini Game is completed
                    transform.parent.gameObject.SetActive(false);

                    //incrase capturerate
                    parentCapturePoint.IncreaseCaptureAmount();
                }
            }
            else if (!isCompleted && Input.anyKeyDown && CheckInput())
            {
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
