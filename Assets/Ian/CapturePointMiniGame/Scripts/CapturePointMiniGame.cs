/*
 * Author: Ian Hudson
 * Description: This script is used for a mini game which is shown to the player.
 * The mini game is based on Simon Says. 
 * Created: 20/02/2019
 * Edited By: Ian
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointMiniGame : MonoBehaviour
{
    //Parent ref
    [SerializeField]
    private GameObject parent = null;

    //Scroll rect ref
    [SerializeField]
    private ScrollRect scroll = null;

    //Define the inputs needed to complete the mini game
    [SerializeField]
    private KeyCode[] inputsNeeded;

    //Deifne which inputs are allowed
    [SerializeField]
    private KeyAllowed[] inputsAllowed;

    //The UI image elemetns 
    [SerializeField]
    private GameObject[] elements;

    //The Image for the feedback
    [SerializeField]
    private Image feedback;

    //Which input is the player at
    [SerializeField]
    private int inputsIndex = 0;

    //Has the mini game been completed
    [SerializeField]
    private bool isCompleted = false;
    public bool IsCompleted
    { get { return isCompleted; } }

    //Parent capture point this mini game is attahced to
    [SerializeField]
    private NO_CapturePoint parentCapturePoint = null;

    //Keep track if we are in a coroutine
    private bool inCoroutine = false;

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

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        SetInputs();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (!inCoroutine)
        {
            bool doneInput = false;
            if (!isCompleted && Input.GetKeyDown(inputsNeeded[inputsIndex]))
            {
                if(ClientManager.Instance != null && ClientManager.Instance.LocalPlayer.gameAvatar.tag == "Spy")
                {
                    ClientManager.Instance.LocalPlayer.gameAvatar.GetComponentInChildren<Animator>().SetBool("isHacking", true);
                }
                GameObject.FindGameObjectWithTag("Spy").GetComponentInChildren<Animator>().SetBool("isHacking", true);
                inCoroutine = true;
                //start coroutine to change colour
                StartCoroutine(Feedback(true));
                doneInput = true;
            }
            if (!doneInput && !isCompleted && CheckInput())
            {
                inCoroutine = true;
                StartCoroutine(Feedback(false));
                inputsIndex = 0;
                scroll.horizontalNormalizedPosition = 0;

                //error. Wrong key pressed
                Msg_ClientMercFeedback cmf = new Msg_ClientMercFeedback();
                cmf.Location = transform.position;

                ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_FEEDBACK, cmf);
            }

            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(item))
                {
                    Debug.Log("KeyCode Down: " + item);
                }
            }
        }
    }

    private void IncerrmentIndex()
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
            //transform.parent.gameObject.SetActive(false);

            //incrase capturerate
            parentCapturePoint.IncreaseCaptureAmount();

            //reset
            ResetGame();
        }
    }

    /// <summary>
    /// Feedback for the key press
    /// </summary>
    /// <returns></returns>
    private IEnumerator Feedback(bool aInput)
    {
        if (aInput)
        {
            feedback.color = Color.green;
        }
        else
        {
            feedback.color = Color.red;
        }
        float step = 0.5f;
        while(step >= 0.0f)
        {
            step -= Time.deltaTime;
            yield return null;
        }
        if (aInput)
        {
            IncerrmentIndex();
        }
        feedback.color = Color.white;
        inCoroutine = false;
    }

    /// <summary>
    /// Reset the mini game
    /// </summary>
    private void ResetGame()
    {
        isCompleted = false;
        scroll.horizontalNormalizedPosition = 0;

        SetInputs();
    }

    /// <summary>
    /// Set all the needed inputs for the mini game
    /// </summary>
    private void SetInputs()
    {
        inputsNeeded = new KeyCode[6];

        for (int i = 0; i < inputsNeeded.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, 4);
            inputsNeeded[i] = inputsAllowed[rand].KeyCode;
            elements[i].GetComponent<Image>().sprite = inputsAllowed[rand].Sprite;
        }
    }

    /// <summary>
    /// Check if the corect input has been processed
    /// </summary>
    /// <returns></returns>
    private bool CheckInput()
    {
        for (int i = 0; i < inputsAllowed.Length; i++)
        {
            if (Input.GetKeyDown(inputsAllowed[i].KeyCode))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if element is within an array 
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

/// <summary>
/// Struct: Define the keycode which is allowed with it's corisponding sprite
/// </summary>
[System.Serializable]
public struct KeyAllowed
{
    public KeyCode KeyCode;
    public Sprite Sprite;
}
